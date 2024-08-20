using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler {
    private Inventory _inventory;
    private int _index;
    private int _prepId = 0;

    [SerializeField] Transform _itemImg;
    [SerializeField] Image _check;
    [SerializeField] Image _equip;
    [SerializeField] Image _blocker;

    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.3f;

    public bool Draggable = false;

    public void SetInventory(Inventory inventory, int index) {
        _inventory = inventory;
        _index = index;
    }
    public void RemoveItemFromInventory() {
        if (!_inventory.RemoveItemAtIndex(_inventory.GetItemAtIndex(_index), _index))
            Debug.LogError($"On essaye d'enlever l'item {_inventory.GetItemAtIndex(_index).Data} à l'index {_index} mais il n'existe pas");
    }
    private void AddItemToInventory(ItemInInventory item) {
        _inventory.AddItemAtIndex(item, _index);
    }

    public void DisplayItem(ItemInInventory itemValues) {
        if (itemValues == null) {
            return;
        }

        Clear();
        DraggableImg img = Instantiate(Resources.Load<DraggableImg>("Prefabs/Ui/Image Draggable"), _itemImg);
        img.SetImage(itemValues.Data.Sprite());
        img.Item = itemValues;

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
            PlayerInventory inv = (PlayerInventory)Game.G.Inv.Get(InvTag.Player);

            if (inv.WaitForItem) {
                UiManager.Instance.CloseInventoryToChose();
                Game.G.Dialog.ItemChosen = item;
                return;
            }
            else if (UiManager.Instance.StockInventoryIsOpen()) {
                if (Game.G.Inv.Get(InvTag.Stock).AddItem(item))
                    inv.RemoveItem(item);
            }
            else if (item.Consommable) {
                if (_inventory is PlayerInventory) {

                    Game.G.Player.Needs.Eat(item);
                    _inventory.RemoveItem(item);
                }
            }
            else if (item.Equippable) {
                if (itemValues.Equipped) {

                    Game.G.Player.Controller.UnEquip(itemValues);
                    itemValues.Equipped = false;
                    _equip.gameObject.SetActive(false);
                }
                else if (Game.G.Player.Controller.Equipped() == null) {

                    Game.G.Player.Controller.Equip(itemValues);
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

    private void OnDisable() {
        if (_inventory.GetItemAtIndex(_index) != null)
            _inventory.GetItemAtIndex(_index).Selected = false;
    }

    private void Clear() {
        if (_itemImg.childCount > 0)
            Destroy(_itemImg.GetChild(0));
    }

    public void OnDrop(PointerEventData eventData) {
        if (_itemImg.childCount > 0)
            return;

        DraggableImg dropped = eventData.pointerDrag.GetComponent<DraggableImg>();
        AddItemToInventory(dropped.Item);
        dropped.RemoveItselfFromOldInventory();
        dropped.TransformAfterDrag = _itemImg;
    }
}
