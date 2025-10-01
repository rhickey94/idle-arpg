using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace IdleARPG.Managers
{
    public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject MainHUD;
    public GameObject ResearchTreePanel;
    public GameObject InventoryPanel;
    public GameObject SettingsPanel;
    public GameObject FacilitiesPanel;
    public GameObject CharacterPanel;

    [Header("Menu Buttons")]
    public Button ResearchButton;
    public Button InventoryButton;
    public Button SettingsButton;
    public Button FacilitiesButton;
    public Button CharacterButton;

    [Header("Close Buttons")]
    public Button CloseResearchButton;
    public Button CloseInventoryButton;
    public Button CloseSettingsButton;
    public Button CloseFacilitiesButton;
    public Button CloseCharacterButton;

    [Header("Input Actions")]
    public InputAction researchAction;
    public InputAction inventoryAction;
    public InputAction facilitiesAction;
    public InputAction characterAction;
    public InputAction escapeAction;

    private readonly List<GameObject> _openMenus = new();
    private bool _isMenuOpen = false;

    public enum MenuType
    {
        None,
        Research,
        Inventory,
        Settings,
        Facilities,
        Character
    }

    private MenuType _currentMenu = MenuType.None;

    private void Start()
    {
        SetupButtonListeners();
        InitializeMenus();
    }

    void SetupButtonListeners()
    {
        // Open menu buttons
        if (ResearchButton != null)
            ResearchButton.onClick.AddListener(() => OpenMenu(MenuType.Research));
        if (InventoryButton != null)
            InventoryButton.onClick.AddListener(() => OpenMenu(MenuType.Inventory));
        if (SettingsButton != null)
            SettingsButton.onClick.AddListener(() => OpenMenu(MenuType.Settings));
        if (FacilitiesButton != null)
            FacilitiesButton.onClick.AddListener(() => OpenMenu(MenuType.Facilities));
        if (CharacterButton != null)
            CharacterButton.onClick.AddListener(() => OpenMenu(MenuType.Character));

        // Close menu buttons
        if (CloseResearchButton != null)
            CloseResearchButton.onClick.AddListener(() => CloseMenu(MenuType.Research));
        if (CloseInventoryButton != null)
            CloseInventoryButton.onClick.AddListener(() => CloseMenu(MenuType.Inventory));
        if (CloseSettingsButton != null)
            CloseSettingsButton.onClick.AddListener(() => CloseMenu(MenuType.Settings));
        if (CloseFacilitiesButton != null)
            CloseFacilitiesButton.onClick.AddListener(() => CloseMenu(MenuType.Facilities));
        if (CloseCharacterButton != null)
            CloseCharacterButton.onClick.AddListener(() => CloseMenu(MenuType.Character));
    }

    void InitializeMenus()
    {
        // Explicitly close all menu panels first
        if (ResearchTreePanel != null) ResearchTreePanel.SetActive(false);
        if (InventoryPanel != null) InventoryPanel.SetActive(false);
        if (SettingsPanel != null) SettingsPanel.SetActive(false);
        if (FacilitiesPanel != null) FacilitiesPanel.SetActive(false);
        if (CharacterPanel != null) CharacterPanel.SetActive(false);

        CloseAllMenus();

        if (MainHUD != null)
        {
            MainHUD.SetActive(true);
        }
    }

    void OnEnable()
    {
        // Subscribe to actions
        researchAction.performed += _ => ToggleMenu(MenuType.Research);
        inventoryAction.performed += _ => ToggleMenu(MenuType.Inventory);
        facilitiesAction.performed += _ => ToggleMenu(MenuType.Facilities);
        characterAction.performed += _ => ToggleMenu(MenuType.Character);
        escapeAction.performed += _ => HandleEscape();

        // Enable all actions
        researchAction.Enable();
        inventoryAction.Enable();
        facilitiesAction.Enable();
        characterAction.Enable();
        escapeAction.Enable();
    }

    void OnDisable()
    {
        // Unsubscribe and disable
        researchAction.performed -= _ => ToggleMenu(MenuType.Research);
        inventoryAction.performed -= _ => ToggleMenu(MenuType.Inventory);
        facilitiesAction.performed -= _ => ToggleMenu(MenuType.Facilities);
        characterAction.performed -= _ => ToggleMenu(MenuType.Character);
        escapeAction.performed -= _ => HandleEscape();

        researchAction.Disable();
        inventoryAction.Disable();
        facilitiesAction.Disable();
        characterAction.Disable();
        escapeAction.Disable();
    }

    void HandleEscape()
    {
        if (_isMenuOpen)
            CloseAllMenus();
        else
            OpenMenu(MenuType.Settings);
    }


    public void OpenMenu(MenuType menuType)
    {
        // Close any currently open menu first
        CloseAllMenus();

        GameObject menuToOpen = GetMenuPanel(menuType);
        if (menuToOpen != null)
        {
            menuToOpen.SetActive(true);
            _openMenus.Add(menuToOpen);
            _currentMenu = menuType;
            _isMenuOpen = true;

            // Pause game when menu is open (optional)
            // Time.timeScale = 0f;

            Debug.Log($"Opened {menuType} menu");
        }
    }

    public void CloseMenu(MenuType menuType)
    {
        GameObject menuToClose = GetMenuPanel(menuType);
        if (menuToClose != null && menuToClose.activeSelf)
        {
            menuToClose.SetActive(false);
            _openMenus.Remove(menuToClose);

            // Update menu state - if no menus left, mark as closed
            if (_openMenus.Count == 0)
            {
                _currentMenu = MenuType.None;
                _isMenuOpen = false;
            }

            Debug.Log($"Closed {menuType} menu");
        }
    }

    public void ToggleMenu(MenuType menuType) 
    {
        if (_currentMenu == menuType)
        {
            CloseMenu(menuType);
        }
        else
        {
            OpenMenu(menuType);
        }
    }

    public void CloseAllMenus() 
    {
        foreach (GameObject menu in _openMenus.ToArray())
        {
            if (menu != null)
            {
                menu.SetActive(false);
            }
        }

        _openMenus.Clear();
        _currentMenu = MenuType.None;
        _isMenuOpen = false;

        Debug.Log("Closed all menus");
    }

    GameObject GetMenuPanel(MenuType menuType) 
    {
        return menuType switch
        {
            MenuType.Research => ResearchTreePanel,
            MenuType.Inventory => InventoryPanel,
            MenuType.Settings => SettingsPanel,
            MenuType.Character => CharacterPanel,
            MenuType.Facilities => FacilitiesPanel,
            MenuType.None => null,
            _ => null,
        };
    }

    public bool IsMenuOpen() => _isMenuOpen;
    public MenuType GetCurrentMenu() => _currentMenu;

    public bool ShouldBlockGameInput()
    {
        return _isMenuOpen;
    }

    void OnGUI()
    {
        if (!_isMenuOpen)
        {
            GUI.Label(new Rect(10, Screen.height - 100, 200, 80),
                "Menu Shortcuts:\n" +
                "R - Research Tree\n" +
                "I - Inventory\n" +
                "F - Facilities\n" +
                "C - Character\n" +
                "ESC - Settings");
        }
    }
    }
}
