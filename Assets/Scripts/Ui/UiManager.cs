using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _pressEInfoTxt;

    public InventoryDisplay PlayerInventoryDisplay;
    public InventoryDisplay CraftInventoryDisplay;
    public InventoryDisplay StockInventoryDisplay;
    public InventoryDisplay KitchenInventoryDisplay;

    public InventoryDisplay Prep1Display;
    public InventoryDisplay Prep2Display;
    public InventoryDisplay Prep3Display;
    public InventoryDisplay Prep4Display;

    [SerializeField] TooltipDisplay _tooltip;
    [SerializeField] PlayerStateDisplay _playerStateDisplay;
    [SerializeField] CraftDisplay _craftDisplay;
    [SerializeField] CookingDisplay _cookingDisplay;

    static public UiManager Instance;

    public Action<Inventory> OnInventoryChanged;
    //  public Action OnPlayerStateChanged;

    public bool PlayerInventoryIsOpen() {
        return Game.G.Inv.GetDisplay(InvTag.Player).IsOpen;
    }
    public bool StockInventoryIsOpen() {
        return Game.G.Inv.GetDisplay(InvTag.Stock).IsOpen;
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

    public void DisplayPressEInfo(string info) {
        _pressEInfoTxt.transform.parent.gameObject.SetActive(true);
        _pressEInfoTxt.text = $"E ({info})";
    }

    public void HidePressEInfo() {
        _pressEInfoTxt.transform.parent.gameObject.SetActive(false);
    }

    public void RefreshInventoryDisplay(Inventory inventory) {
        InventoryDisplay inventoryDisplay = Game.G.Inv.GetDisplay(inventory);

        if (!inventoryDisplay.gameObject.activeSelf)
            return;
        else {
            inventoryDisplay.Display(inventory);
        }
    }
    public void DisplayInventory(Inventory inventory) {

        var display = Game.G.Inv.GetDisplay(inventory);

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

    public void DisplayCooking() {
        _cookingDisplay.Display();
    }
    // TODO : à changer, ce n'est pas à l'UI manager de gérer ça
    public void SetCurrentPreparation(Preparation prep) {
        _cookingDisplay.SetCurrentPreparation(prep);
    }
    public void DisplayPlate(Plate plate) {
        _cookingDisplay.DisplayPlate(plate);
    }
    public void HidePlate() {
        _cookingDisplay.HidePlate();
    }
    public void QuitDisplayCooking() {
        _cookingDisplay?.QuitDisplay();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            DisplayInventory(Game.G.Inv.Get(InvTag.Player));
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
