using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum SlotType {
    PlayerInventory,
    Craft,
    OtherInventory,
    Kitchen,
    Preparation
}

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    private Inventory _inventory;
    private int _index;

    [SerializeField] Image _itemImg;
    [SerializeField] Image _check;
    [SerializeField] Image _equip;
    [SerializeField] Image _blocker;

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.3f;

    private bool selected = false;

    public void SetInventory(Inventory inventory, int index) {
        _inventory = inventory;
        _index = index;
    }

    public void DisplayItem(ItemInInventory itemValues) {
        if (itemValues == null) {
            return;
        }

        _itemImg.sprite = itemValues.Data.Sprite();

        if (itemValues.Equipped)
            _equip.gameObject.SetActive(true);

        if (_inventory.GetSlotType() == SlotType.Craft) {
            _blocker.gameObject.SetActive(!Game.G.Craft.CurrentCraft.NeedItemForRecipe(itemValues.Data));
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        var item = _inventory.GetItemAtIndex(_index);

        if (item == null) { return; }

        if (_inventory.GetSlotType() == SlotType.PlayerInventory) {
            CheckPlayerInventoryClick(item);
        }
        else if (_inventory.GetSlotType() == SlotType.Craft) {
            CheckCraftInventoryClick(item);
        }
        else if (_inventory.GetSlotType() == SlotType.OtherInventory) {
            CheckOtherInventoryClick(item);
        }
        else if (_inventory.GetSlotType() == SlotType.Kitchen) {
            CheckKitchenInventoryClick(item);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(_inventory == null) {
            Debug.LogError("On n'a pas associé d'inventaire au slot !");
            return;
        }
        if (_inventory.GetItemAtIndex(_index) == null)
            return;

        UiManager.Instance.DisplayTooltip(_inventory.GetItemAtIndex(_index).Data);
    }

    public void OnPointerExit(PointerEventData eventData) {
        UiManager.Instance.HideTooltip();
    }

    private void CheckPlayerInventoryClick(ItemInInventory itemValues) {
        ItemData item = itemValues.Data;


        if (Time.time - lastClickTime < doubleClickThreshold) {

            if (UiManager.Instance.StockInventoryIsOpen()) {
                if (StockInventory.Instance.AddItem(item))
                    PlayerInventory.Instance.RemoveItem(item);
            }

            if (item.Consommable) {
                if (_inventory is PlayerInventory) {

                    Game.G.Player.Eat(item);
                    _inventory.RemoveItem(item);
                }
            }
            else if (item.Equippable) {
                if (itemValues.Equipped) {

                    Game.G.Player.UnEquip(itemValues);
                    itemValues.Equipped = false;
                    _equip.gameObject.SetActive(false);
                }
                else if (Game.G.Player.Equipped() == null) {

                    Game.G.Player.Equip(itemValues);
                    itemValues.Equipped = true;
                    _equip.gameObject.SetActive(true);
                }

            }
        }
        else {
            lastClickTime = Time.time;
        }
    }
    private void CheckCraftInventoryClick(ItemInInventory item) {
        if (_blocker.gameObject.activeSelf) {
            return;
        }

        selected = !selected;

        if (selected)
            Game.G.Craft.CurrentIngredients.Add(item.Data);
        else
            Game.G.Craft.CurrentIngredients.Remove(item.Data);

        _check.gameObject.SetActive(selected);
    }

    private void CheckKitchenInventoryClick(ItemInInventory item) {
        selected = !selected;

        if (selected)
            Game.G.Cook.AddIngredientToCurrentPreparation(item.Data);
        else
            Game.G.Cook.RemoveIngredientFromCurrentPreparation(item.Data);

        _check.gameObject.SetActive(selected);
    }

    private void CheckOtherInventoryClick(ItemInInventory itemValues) {
        ItemData item = itemValues.Data;


        if (Time.time - lastClickTime < doubleClickThreshold) {
            StockInventory.Instance.RemoveItem(item);
            PlayerInventory.Instance.AddItem(item);
        }
        else {
            lastClickTime = Time.time;
        }
    }
}
