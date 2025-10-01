using UnityEngine;
using UnityEngine.InputSystem;
using IdleARPG.Managers;

namespace IdleARPG.Player
{
    public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 5f;

    [Header("Combat")]
    public float CurrentXP = 0f;
    public int CurrentLevel = 1;
    public float XPToNextLevel = 100f;

    [Header("Automation")]
    public bool AutoLootEnabled = false;

    private ResearchManager _researchManager;
    private MenuManager _menuManager;
    private Vector2 _moveInput;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _gainXPAction;
    private InputAction _testAutoLootAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _researchManager = FindFirstObjectByType<ResearchManager>();
        _menuManager = FindFirstObjectByType<MenuManager>();

        ResearchManager.OnResearchUnlocked += OnResearchUnlocked;

        CheckUnlockedResearch();

        SetupInputActions();
    }

    void SetupInputActions()
    {
        // Create input actions programmatically
        _moveAction = new InputAction("Move", InputActionType.Value, "<Gamepad>/leftStick");
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");

        _gainXPAction = new InputAction("GainXP", InputActionType.Button, "<Keyboard>/space");
        _testAutoLootAction = new InputAction("TestAutoLoot", InputActionType.Button, "<Keyboard>/l");

        // Enable actions
        _moveAction.Enable();
        _gainXPAction.Enable();
        _testAutoLootAction.Enable();

        // Subscribe to events
        _gainXPAction.performed += OnGainXPPressed;
        _testAutoLootAction.performed += OnTestAutoLootPressed;
    }

    void OnDestroy() 
    {
        ResearchManager.OnResearchUnlocked -= OnResearchUnlocked;

        _moveAction?.Disable();
        _gainXPAction?.Disable();
        _testAutoLootAction?.Disable();


    }

    // Update is called once per frame
    void Update()
    {
        // Don't process input if menu is open
        if (_menuManager != null && _menuManager.ShouldBlockGameInput())
            return;

        HandleMovement();
    }

    void HandleMovement()
    {
        // Read movement input
        _moveInput = _moveAction.ReadValue<Vector2>();

        // Apply movement
        Vector3 movement = new(_moveInput.x, 0, _moveInput.y);
        transform.Translate(movement * MoveSpeed * Time.deltaTime);

        // Debug movement
        if (movement.magnitude > 0)
        {
            Debug.Log($"Moving: {movement} (Input: {_moveInput})");
        }
    }

    void OnGainXPPressed(InputAction.CallbackContext context)
    {
        GainXP(10);
    }

    void OnTestAutoLootPressed(InputAction.CallbackContext context)
    {
        if (AutoLootEnabled)
        {
            Debug.Log("Auto-loot activated!");
        }
        else
        {
            Debug.Log("Auto-loot not unlocked yet!");
        }
    }

    public void GainXP(float amount) 
    {
        float multiplier = 1f;
        if (_researchManager != null && _researchManager.IsResearchUnlocked("xp_boost"))
        {
            multiplier = 1.1f;
        }

        float actualXP = amount * multiplier;
        CurrentXP += actualXP;

        Debug.Log($"Gained {actualXP:F1} XP (base: {amount}, multiplier: {multiplier:F1}x)");

        // Check for level up
        while (CurrentXP >= XPToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp() 
    {
        CurrentXP -= XPToNextLevel;
        CurrentLevel++;
        XPToNextLevel *= 1.2f;

        Debug.Log($"Level up! Now level {CurrentLevel}");
    }

    void OnResearchUnlocked(string researchId) 
    {
        ApplyResearchEffect(researchId);
    }

    void CheckUnlockedResearch() 
    {
        if (_researchManager.IsResearchUnlocked("auto_loot"))
            ApplyResearchEffect("auto_loot");
        if (_researchManager.IsResearchUnlocked("xp_boost"))
            ApplyResearchEffect("xp_boost");
        if (_researchManager.IsResearchUnlocked("hp_boost"))
            ApplyResearchEffect("hp_boost");
    }

    void ApplyResearchEffect(string researchId) 
    {
        switch (researchId)
        {
            case "auto_loot":
                AutoLootEnabled = true;
                Debug.Log("Auto Loot Enabled");
                break;

            case "xp_boost":
                Debug.Log("XP Boost activated!");
                break;

            case "hp_boost":
                Debug.Log("HP Boost activated!");
                break;
        }
    }

    private void OnGUI()
    {
        // Simple on-screen info
        GUI.Label(new Rect(10, 100, 300, 20), $"Level: {CurrentLevel}");
        GUI.Label(new Rect(10, 120, 300, 20), $"XP: {CurrentXP:F1} / {XPToNextLevel:F1}");
        GUI.Label(new Rect(10, 140, 300, 20), $"Auto-Loot: {(AutoLootEnabled ? "ON" : "OFF")}");
        GUI.Label(new Rect(10, 160, 300, 20), $"Move Input: {_moveInput}");

        GUI.Label(new Rect(10, 200, 300, 20), "Controls:");
        GUI.Label(new Rect(10, 220, 300, 20), "WASD or Arrow Keys - Move");
        GUI.Label(new Rect(10, 240, 300, 20), "Space - Gain XP (test)");
        GUI.Label(new Rect(10, 260, 300, 20), "L - Test Auto-Loot");
    }
    }
}
