using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] StateDisplay _playerStateDisplay;
    [SerializeField] StateDisplay _pnjStateDisplay;
    [SerializeField] CraftDisplay _craftDisplay;
    [SerializeField] CookingDisplay _cookingDisplay;
    [SerializeField] CollectionDisplay _collectionDisplay;

    [SerializeField] Image _showedItemImage;

    static public UiManager Instance;

    public Action<Inventory> OnInventoryChanged;

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
            display.ExitDisplay();
        }
        else
            display.Display(inventory);
    }

    public void DisplayTooltip(ItemData item) {
        _tooltip.Display(item);
    }
    public void DisplayTooltip(RecipeData recipe) {
        _tooltip.Display(recipe);
    }
    public void HideTooltip() {
        _tooltip.Hide();
    }
    public void DisplayCraft() {
        if (_craftDisplay.gameObject.activeSelf) {

            _craftDisplay.QuitDisplay();
        }
        else {
            _craftDisplay.DisplayCraftSelection();
        }
    }
    public void HideCraft() {
        _craftDisplay.QuitDisplay();
    }

    public void DisplayCollection() {
        _collectionDisplay.Display();
    }
    public void HideCollection() {
        _collectionDisplay.Hide();
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
    public void HideCooking() {
        _cookingDisplay?.QuitDisplay();
    }

    public void OpenInventoryToChose() {
        PlayerInventory inv = (PlayerInventory)Game.G.Inv.Get(InvTag.Player);
        inv.WaitForItem = true;
        DisplayInventory(inv);
    }
    public void CloseInventoryToChose() {
        PlayerInventory inv = (PlayerInventory)Game.G.Inv.Get(InvTag.Player);
        inv.WaitForItem = true;
        DisplayInventory(Game.G.Inv.Get(InvTag.Player));
    }

    public void DisplayItemShowedSprite(ItemData item) {
        _showedItemImage.transform.parent.gameObject.SetActive(true);
        _showedItemImage.sprite = item.ShowToPnjSprite();
    }
    public void HideItemShowedSprite() {
        _showedItemImage.transform.parent.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            DisplayInventory(Game.G.Inv.Get(InvTag.Player));
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            DisplayCraft();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            DisplayCollection();
        }

        _playerStateDisplay.Display();
        _pnjStateDisplay.Display();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            _craftDisplay.QuitDisplay();
            _cookingDisplay.QuitDisplay();
            _collectionDisplay.Hide();
        }
    }

    public void Clear(Transform t) {
        foreach (Transform child in t) {
            Destroy(child.gameObject);
        }
    }
}
