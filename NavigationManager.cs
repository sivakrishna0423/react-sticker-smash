using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager
{
    private readonly PhysioExercisesAIG physioExercisesAIG;
    private readonly int jumpToNextNum;

    public NavigationManager(PhysioExercisesAIG physioExercisesAIG, int jumpToNextNum)
    {
        this.physioExercisesAIG = physioExercisesAIG;
        this.jumpToNextNum = jumpToNextNum;
    }

    public void JumpToNextModule()
    {
        if (SceneManager.GetActiveScene().name == "Exercises_Allvarients")
        {
            HandleAllVariantsScene();
        }
        else
        {
            HandleMainScene();
        }
    }

    private void HandleAllVariantsScene()
    {
        if (physioExercisesAIG != null)
        {
            physioExercisesAIG.SelectingCanvas.SetActive(true);
            physioExercisesAIG.GetComponent<GameObject>().SetActive(false);
        }
    }

    private void HandleMainScene()
    {
        if (GameManager.instance.automatic)
        {
            if (jumpToNextNum >= 14)
            {
                GameManager.instance.activePanel(11);
            }
            else
            {
                physioExercisesAIG?.onExerciseToggleButton(jumpToNextNum);
            }
        }
        else
        {
            GameManager.instance.activePanel(4);
        }
    }
}