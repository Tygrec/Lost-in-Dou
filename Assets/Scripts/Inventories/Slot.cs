using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    private Inventory _inventory;
    private int _index;
    private int _prepId = 0;

    [SerializeField] Image _itemImg;
    [SerializeField] Image _check;
    [SerializeField] Image _equip;
    [SerializeField] Image _blocker;

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.3f;

    public void SetInventory(Inventory inventory, int index) {
        _inventory = inventory;
        _index = index;
    }

    public void DisplayItem(ItemInInventory itemValues) {
        if (itemValues == null) {
            return;
        }

        _itemImg.sprite = itemValues.Data.Sprite();

        _equip.gameObject.SetActive(itemValues.Equipped);
        _check.gameObject.SetActive(itemValues.Selected);

        if (_inventory.GetSlotType() == InvTag.Craft) {
            _blocker.gameObject.SetActive(!Game.G.Craft.CurrentCraft.NeedItemForRecipe(itemValues.Data));
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        var item = _inventory.GetItemAtIndex(_index);

        if (item == null) { return; }

        if (_inventory.GetSlotType() == InvTag.Player) {
            CheckPlayerInventoryClick(item);
        }
        else if (_inventory.GetSlotType() == InvTag.Craft) {
            CheckCraftInventoryClick(item);
        }
        else if (_inventory.GetSlotType() == InvTag.Stock) {
            CheckStockInventoryClick(item);
        }
        else if (_inventory.GetSlotType() == InvTag.Kitchen) {
            CheckKitchenInventoryClick(item);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (_inventory == null) {
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
                if (Game.G.Inv.Get(InvTag.Stock).AddItem(item))
                    Game.G.Inv.Get(InvTag.Player).RemoveItem(item);
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

        item.Selected = !item.Selected;

        if (item.Selected)
            Game.G.Craft.CurrentIngredients.Add(item.Data);
        else
            Game.G.Craft.CurrentIngredients.Remove(item.Data);

        _check.gameObject.SetActive(item.Selected);
    }

    private void CheckKitchenInventoryClick(ItemInInventory item) {
        int currentPrepId = Game.G.Cook.GetCurrentPreparationId();

        // Aucune préparation sélectionnée et l'item n'est dans aucune préparation
        if (currentPrepId == -1 && !item.Selected)
            return;

        // Préparation sélectionnée et l'item n'est dans aucune préparation
        if (currentPrepId != -1 && !item.Selected) {
            Game.G.Cook.AddIngredientToCurrentPreparation(item.Data);
            GetComponent<Outline>().effectColor = Game.G.Cook.GetCurrentPreparationColor();
            _prepId = currentPrepId;
        }

        // L'item est dans une préparation
        if (item.Selected) {
            Game.G.Cook.RemoveIngredientFromPreparation(item.Data, _prepId);
            _prepId = -1;
        }

        item.Selected = !item.Selected;

        GetComponent<Outline>().enabled = item.Selected;
    }


    private void CheckStockInventoryClick(ItemInInventory itemValues) {
        ItemData item = itemValues.Data;


        if (Time.time - lastClickTime < doubleClickThreshold) {
            Game.G.Inv.Get(InvTag.Stock).RemoveItem(item);
            Game.G.Inv.Get(InvTag.Player).AddItem(item);
        }
        else {
            lastClickTime = Time.time;
        }
    }
}
