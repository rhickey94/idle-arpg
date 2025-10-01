using TMPro;
using UnityEngine;
using UnityEngine.UI;
using IdleARPG.Managers;

namespace IdleARPG.UI
{
    public class ResearchUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI ResearchPointsTextTMP;
    public Transform ResearchButtonParent;
    public GameObject ResearchButtonPrefab;

    private ResearchManager _researchManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _researchManager = FindFirstObjectByType<ResearchManager>();

        ResearchManager.OnResearchPointsChanged += UpdateResearchPointsDisplay;
        ResearchManager.OnResearchUnlocked += OnResearchUnlocked;

        CreateResearchButtons();

        UpdateResearchPointsDisplay(_researchManager.CurrentResearchPoints);
    }

    void OnDestroy()
    {
        ResearchManager.OnResearchPointsChanged -= UpdateResearchPointsDisplay;
        ResearchManager.OnResearchUnlocked -= OnResearchUnlocked;
    }

    void UpdateResearchPointsDisplay(float points) 
    {
        string displayText = $"Research Points: {points:F1}";
        if (ResearchPointsTextTMP != null)
        {
            ResearchPointsTextTMP.text = displayText ;
        }
    }

    void CreateResearchButtons()
    {
        if (ResearchButtonParent == null || ResearchButtonPrefab == null) return;

        foreach (var node in _researchManager.ResearchNodes)
        {
            GameObject buttonObj = Instantiate(ResearchButtonPrefab, ResearchButtonParent);

            // Get button components
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonTextTMP = buttonObj.GetComponent<TextMeshProUGUI>();

            // Set button text
            string buttonTextContent = $"{node.DisplayName}\nCost: {node.Cost}";
            if (buttonTextTMP != null)
            {
                buttonTextTMP.text = buttonTextContent;
            }

            // Add click listener
            string nodeId = node.Id; // Capture for closure
            button.onClick.AddListener(() => TryUnlockResearch(nodeId));

            // Store reference for updates
            buttonObj.name = $"ResearchButton_{node.Id}";
        }

        UpdateButtonStates();
    }

    void TryUnlockResearch(string researchId)
    {
        if (_researchManager.UnlockResearch(researchId))
        {
            UpdateButtonStates();
        }
        else
        {
            Debug.Log("Cannot unlock research - insufficient points or already unlocked");
        }
    }

    void OnResearchUnlocked(string researchId)
    {
        Debug.Log($"Research unlocked UI notification: {researchId}");
        UpdateButtonStates();
    }

    void UpdateButtonStates() 
    {
        foreach (var node in _researchManager.ResearchNodes)
        {
            GameObject buttonObj = GameObject.Find($"ResearchButton_{node.Id}");
            if (buttonObj == null) continue;

            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonTextTMP = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            string displayText;
            Color buttonColor;

            if (node.IsUnlocked)
            {
                // Research is unlocked
                button.interactable = false;
                displayText = $"{node.DisplayName}\n UNLOCKED";
                buttonColor = Color.green;
            }
            else if (_researchManager.CanUnlockResearch(node.Id))
            {
                // Can afford this research
                button.interactable = true;
                displayText = $"{node.DisplayName} \nCost:  {node.Cost}";
                buttonColor = Color.white;
            }
            else
            {
                // Cannot afford this research
                button.interactable = false;
                displayText = $"{node.DisplayName} \nCost:  {node.Cost}";
                buttonColor = Color.gray;
            }

            // Set text
            if (buttonTextTMP != null)
            {
                buttonTextTMP.text = displayText;
            }

            // Set color
            buttonObj.GetComponent<Image>().color = buttonColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only update if the research panel is actually visible
        if (!transform.root.gameObject.activeSelf) return;
        UpdateButtonStates();
    }
    }
}
