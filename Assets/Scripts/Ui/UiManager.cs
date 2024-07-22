using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _pressEInfoTxt;

    [SerializeField] InventoryDisplay _playerInventory;
    [SerializeField] InventoryDisplay _craftInventory;
    [SerializeField] InventoryDisplay _stockInventory;

    [SerializeField] TooltipDisplay _tooltip;
    [SerializeField] PlayerStateDisplay _playerStateDisplay;
    [SerializeField] CraftDisplay _craftDisplay;

    static public UiManager Instance;

    public Action<Inventory> OnInventoryChanged;
    //  public Action OnPlayerStateChanged;
    public Dictionary<Inventory, InventoryDisplay> InventoryDisplayMapping = new Dictionary<Inventory, InventoryDisplay>();

    public bool PlayerInventoryIsOpen() {
        return _playerInventory.IsOpen;
    }
    public bool StockInventoryIsOpen() {
        return _stockInventory.IsOpen;
    }

    private void OnEnable() {
        OnInventoryChanged += RefreshInventoryDisplay;
    }
    private void OnDisable() {
        OnInventoryChanged -= RefreshInventoryDisplay;
    }
    private void Awake() {
        Instance = this;
    }

    public void Init() {
        InventoryDisplayMapping.Add(PlayerInventory.Instance, _playerInventory);
        InventoryDisplayMapping.Add(CraftInventory.Instance, _craftInventory);
        InventoryDisplayMapping.Add(StockInventory.Instance, _stockInventory);
    }

    public void DisplayPressEInfo(string info) {
        _pressEInfoTxt.transform.parent.gameObject.SetActive(true);
        _pressEInfoTxt.text = $"E ({info})";
    }

    public void HidePressEInfo() {
        _pressEInfoTxt.transform.parent.gameObject.SetActive(false);
    }

    public void RefreshInventoryDisplay(Inventory inventory) {
        InventoryDisplay inventoryDisplay = InventoryDisplayMapping[inventory];

        if (!inventoryDisplay.gameObject.activeSelf)
            return;
        else {
            inventoryDisplay.Display(inventory);
        }
    }
    public void DisplayInventory(Inventory inventory) {

        var display = InventoryDisplayMapping[inventory];

        if (display.gameObject.activeSelf) {
            HideTooltip();
            display.gameObject.SetActive(false);
        }
        else
            display.Display(inventory);
    }

    public void DisplayTooltip(ItemData item) {
        _tooltip.Display(item);
    }
    public void HideTooltip() {
        _tooltip.gameObject.SetActive(false);
    }
    public void DisplayCraft() {
        if (_craftDisplay.gameObject.activeSelf) {

            _craftDisplay.QuitDisplay();
        }
        else {
            _craftDisplay.DisplayCraftSelection();
        }
    }
    public void QuitCraftDisplay() {
        _craftDisplay.QuitDisplay();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            DisplayInventory(PlayerInventory.Instance);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            DisplayCraft();
        }

        _playerStateDisplay.Display();
    }

    public void Clear(Transform t) {
        foreach (Transform child in t) {
            Destroy(child.gameObject);
        }
    }
}
