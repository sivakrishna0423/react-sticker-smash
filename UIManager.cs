using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI repetitionText;
    [SerializeField] private TextMeshProUGUI holdTimeCounterText;
    [SerializeField] private GameObject pillowConfirmationPanel;
    [SerializeField] private Button pillowStartButton;
    [SerializeField] private GameObject yogaMat;

    [Header("Settings")]
    [SerializeField] private bool pillowConfirmationRequired = false;
    [SerializeField] private bool yogaMatRequired = false;

    public event Action OnPillowConfirmationYes;

    public bool PillowConfirmationRequired => pillowConfirmationRequired;
    public bool YogaMatRequired => yogaMatRequired;
    public TextMeshProUGUI HoldTimeCounterText => holdTimeCounterText;

    private void Awake()
    {
        SetupButtonListeners();
    }

    public void DisplayRepetitionCounter(int currentCount)
    {
        if (repetitionText != null)
        {
            repetitionText.text = $"Repetition : {currentCount + 1}";
        }
    }

    public void ShowPillowConfirmation()
    {
        if (pillowConfirmationPanel != null)
        {
            pillowConfirmationPanel.SetActive(true);
        }
    }

    public void HidePillowConfirmation()
    {
        if (pillowConfirmationPanel != null)
        {
            pillowConfirmationPanel.SetActive(false);
        }
    }

    public void SetYogaMatVisibility(bool visible)
    {
        if (yogaMat != null)
        {
            yogaMat.SetActive(visible && yogaMatRequired);
        }
    }

    public void UpdateHoldTimeDisplay(float remainingTime)
    {
        if (holdTimeCounterText != null)
        {
            if (remainingTime > 0)
            {
                holdTimeCounterText.text = $"Hold Time : {remainingTime:F0}";
            }
            else
            {
                holdTimeCounterText.text = "";
            }
        }
    }

    private void SetupButtonListeners()
    {
        if (pillowStartButton != null)
        {
            pillowStartButton.onClick.AddListener(HandlePillowConfirmationYes);
        }
    }

    private void HandlePillowConfirmationYes()
    {
        HidePillowConfirmation();
        OnPillowConfirmationYes?.Invoke();
    }

    private void OnDestroy()
    {
        if (pillowStartButton != null)
        {
            pillowStartButton.onClick.RemoveListener(HandlePillowConfirmationYes);
        }
    }
}