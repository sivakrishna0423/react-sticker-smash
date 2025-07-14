using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoGroup;
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private int waitTimeForHold = 5;
    
    public event Action<VideoPlayer> OnVideoEnded;
    public event Action OnVideoPrepared;

    private bool isInitialized = false;

    public void Initialize()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += HandleVideoEnd;
            videoPlayer.prepareCompleted += HandleVideoPrepared;
            isInitialized = true;
        }
    }

    public void PrepareAndPlayVideo()
    {
        if (!isInitialized || videoClip == null)
        {
            Debug.LogError("VideoPlayerController not initialized or no video clip assigned");
            return;
        }

        StartCoroutine(PrepareVideoWithDelay());
    }

    public void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    public void ResumeVideo()
    {
        if (videoPlayer != null && videoPlayer.isPaused)
        {
            videoPlayer.Play();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoGroup.SetActive(false);
        }
    }

    public IEnumerator PlayVideoWithHold(int holdTime, AudioManager audioManager, 
        TMPro.TextMeshProUGUI holdTimeCounterText, int playCount, bool holdRequired)
    {
        if (!holdRequired) yield break;

        yield return new WaitForSeconds(waitTimeForHold);
        
        audioManager?.PlayHoldVoiceOver(playCount);
        PauseVideo();

        yield return StartCoroutine(DisplayHoldCountdown(holdTime, holdTimeCounterText));

        ResumeVideo();
        audioManager?.PlayRelaxAudio();
    }

    private IEnumerator PrepareVideoWithDelay()
    {
        yield return new WaitForSeconds(1);
        
        videoGroup.SetActive(true);
        videoPlayer.clip = videoClip;
        videoPlayer.Prepare();
    }

    private IEnumerator DisplayHoldCountdown(int holdTime, TMPro.TextMeshProUGUI holdTimeCounterText)
    {
        float remainingTime = holdTime;
        
        while (remainingTime > 0)
        {
            if (holdTimeCounterText != null)
            {
                holdTimeCounterText.text = "Hold Time : " + remainingTime;
            }
            yield return new WaitForSeconds(1);
            remainingTime -= 1.0f;
        }
        
        if (holdTimeCounterText != null)
        {
            holdTimeCounterText.text = "";
        }
    }

    private void HandleVideoPrepared(VideoPlayer vp)
    {
        if (vp.gameObject.GetComponent<RawImage>() != null)
        {
            vp.gameObject.GetComponent<RawImage>().texture = vp.targetTexture;
        }
        vp.Play();
        OnVideoPrepared?.Invoke();
    }

    private void HandleVideoEnd(VideoPlayer vp)
    {
        OnVideoEnded?.Invoke(vp);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= HandleVideoEnd;
            videoPlayer.prepareCompleted -= HandleVideoPrepared;
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= HandleVideoEnd;
            videoPlayer.prepareCompleted -= HandleVideoPrepared;
        }
    }
}