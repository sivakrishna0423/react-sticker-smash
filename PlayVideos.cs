using System;
using System.Collections;
using UnityEngine;

public class PlayVideos : MonoBehaviour
{
    [Header("Core Components")]
    [SerializeField] private VideoPlayerController videoPlayerController;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private UIManager uiManager;
    
    [Header("Exercise Settings")]
    [SerializeField] private string moduleName;
    [SerializeField] private bool holdRequired = false;
    [SerializeField] private bool showVideos = true;
    [SerializeField] private bool isRightSideRequired = false;
    [SerializeField] private bool userStandUpRequired = false;
    
    [Header("References")]
    [SerializeField] private PhysioExercisesAIG physioExercisesAIG;
    [SerializeField] private PlayVideos rightPlayVideos;
    [SerializeField] private int jumpToNextNum = 0;
    
    // Events
    public Action<int> OnRepetition;
    
    // Private fields
    private ExerciseDataManager exerciseDataManager;
    private NavigationManager navigationManager;
    private int currentPlayCount = 0;
    private bool userStandUpHandled = false;

    #region Unity Lifecycle

    private void Start()
    {
        InitializeComponents();
    }

    private void OnEnable()
    {
        Debug.Log($"OnEnable called: {moduleName}");
        SetupExercise();
    }

    private void Update()
    {
        HandleUserStandUpDetection();
    }

    private void OnDestroy()
    {
        CleanupEventSubscriptions();
    }

    #endregion

    #region Initialization

    private void InitializeComponents()
    {
        // Initialize module name if not set
        if (string.IsNullOrEmpty(moduleName))
        {
            moduleName = gameObject.name;
        }

        // Initialize managers
        exerciseDataManager = new ExerciseDataManager(moduleName, physioExercisesAIG);
        navigationManager = new NavigationManager(physioExercisesAIG, jumpToNextNum);

        // Initialize components
        videoPlayerController?.Initialize();
        audioManager?.Initialize();

        // Subscribe to events
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        if (videoPlayerController != null)
        {
            videoPlayerController.OnVideoEnded += HandleVideoEnd;
            videoPlayerController.OnVideoPrepared += HandleVideoPrepared;
        }

        if (uiManager != null)
        {
            uiManager.OnPillowConfirmationYes += StartPlayingVideos;
        }
    }

    private void CleanupEventSubscriptions()
    {
        if (videoPlayerController != null)
        {
            videoPlayerController.OnVideoEnded -= HandleVideoEnd;
            videoPlayerController.OnVideoPrepared -= HandleVideoPrepared;
        }

        if (uiManager != null)
        {
            uiManager.OnPillowConfirmationYes -= StartPlayingVideos;
        }
    }

    #endregion

    #region Exercise Setup and Flow

    private void SetupExercise()
    {
        if (PatientPackageInfo.instance != null)
        {
            PatientPackageInfo.instance.GetVariationOfExercise(moduleName);
        }

        StartCoroutine(ExecuteExerciseFlow());
    }

    private IEnumerator ExecuteExerciseFlow()
    {
        // Play welcome audio
        yield return StartCoroutine(audioManager.PlayWelcomeAudio());
        Debug.Log("Welcome audios completed. Started Videos playing");
        
        yield return new WaitForSeconds(2);

        if (!userStandUpRequired)
        {
            HandleExerciseStart();
        }
    }

    private void HandleExerciseStart()
    {
        SetupUI();

        if (!uiManager.PillowConfirmationRequired)
        {
            StartPlayingVideos();
        }
        else
        {
            uiManager.ShowPillowConfirmation();
        }
    }

    private void SetupUI()
    {
        uiManager?.DisplayRepetitionCounter(currentPlayCount);
        uiManager?.SetYogaMatVisibility(uiManager.YogaMatRequired);
    }

    #endregion

    #region Video Playback

    public void StartPlayingVideos()
    {
        exerciseDataManager.InitializeExerciseData();
        
        if (showVideos && exerciseDataManager.MaxPlays > 0)
        {
            videoPlayerController?.PrepareAndPlayVideo();
        }
    }

    private void HandleVideoPrepared()
    {
        StartCoroutine(ExecuteVideoWithAudioAndHold());
    }

    private IEnumerator ExecuteVideoWithAudioAndHold()
    {
        // Play video audio
        yield return StartCoroutine(audioManager.PlayVideoAudio());
        
        // Handle hold if required
        yield return StartCoroutine(videoPlayerController.PlayVideoWithHold(
            exerciseDataManager.HoldTime,
            audioManager,
            uiManager.HoldTimeCounterText,
            currentPlayCount,
            holdRequired
        ));
    }

    private void HandleVideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        currentPlayCount++;

        if (currentPlayCount < exerciseDataManager.MaxPlays)
        {
            ContinueExercise();
        }
        else
        {
            CompleteExercise();
        }
    }

    private void ContinueExercise()
    {
        uiManager?.DisplayRepetitionCounter(currentPlayCount);
        OnRepetition?.Invoke(currentPlayCount);
        StartCoroutine(ExecuteVideoWithAudioAndHold());
    }

    private void CompleteExercise()
    {
        videoPlayerController?.StopVideo();
        audioManager?.StopAudio();
        
        currentPlayCount = 0;
        
        exerciseDataManager.SendDataToServer(currentPlayCount);

        if (isRightSideRequired && rightPlayVideos != null)
        {
            TransitionToRightSide();
        }
        else
        {
            navigationManager.JumpToNextModule();
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region User Interaction Handling

    private void HandleUserStandUpDetection()
    {
        if (!userStandUpRequired || userStandUpHandled)
            return;

        if (DetectUserStandUp())
        {
            HandleExerciseStart();
            userStandUpHandled = true;
        }
    }

    private bool DetectUserStandUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            return hit.collider.gameObject.name == "TurnTrigger";
        }
        return false;
    }

    #endregion

    #region Side Transition

    private void TransitionToRightSide()
    {
        if (rightPlayVideos != null)
        {
            rightPlayVideos.gameObject.SetActive(true);
            gameObject.SetActive(false);
            exerciseDataManager.SendDataToServer(currentPlayCount);
        }
    }

    #endregion
}