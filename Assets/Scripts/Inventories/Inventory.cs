using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory {
    public int Id;

    protected ItemInInventory[] _stock;
    protected int _size;
    protected InvTag _slotType;

    public int GetMaxSize() {
        return _size;
    }
    public ItemInInventory GetItemAtIndex(int index) {
        return _stock[index];
    }
    public int GetCurrentSize() {
        int size = 0;
        foreach (var item in _stock) {
            if (item != null)
                size++;
        }
        return size;
    }
    public InvTag GetSlotType() {
        return _slotType;
    }
    public ItemInInventory[] GetStock() {
        return _stock;
    }

    public virtual void Initialize() {
        _stock = new ItemInInventory[_size];
    }

    // Retourne FALSE s'il n'y a plus de place dans l'inventaire
    public virtual bool AddItem(ItemData item) {

        if (!Utils.ArrayHasRoom(_stock)) {
            return false;
        }

        AddItemInInventory(_stock, item);

        UiManager.Instance.OnInventoryChanged.Invoke(this);
        if (_slotType == InvTag.Player)
            Game.G.Db.OnNewItem.Invoke(item);

        return true;
    }

    public virtual void AddItemAtIndex(ItemInInventory item, int i) {
        if (_stock[i] != null)
            return;

        _stock[i] = item;
    }

    // Retourne FALSE s'il n'y a pas assez d'item à retirer de l'inventaire
    public virtual bool RemoveItem(ItemData item) {

        if (!ItemExistsInInventory(item)) {
            return false;
        }


        RemoveItemFromInventory(_stock, item);

        UiManager.Instance.OnInventoryChanged.Invoke(this);
        return true;
    }

    public virtual bool RemoveItemAtIndex(ItemInInventory item, int i) {
        if (_stock[i] != item)
            return false;

        _stock[i] = null;
        return true;
    }
    // Fonctions utilitaires pour manipuler l'inventaire
    public bool ItemExistsInInventory(ItemData item) {
        foreach (var i in _stock) {
            if (i == null)
                continue;
            else if (i.Data == item)
                return true;
        }

        return false;
    }
    public bool ItemAvailableInInventory(ItemData item) {
        foreach (var i in _stock) {
            if (i == null || i.Selected)
                continue;
            else if (i.Data == item && !i.Selected)
                return true;
        }

        return false;
    }
    public List<ItemInInventory> GetFoodStock() {
        List<ItemInInventory> foodStock = new List<ItemInInventory>();

        foreach (var item in _stock) {
            if (item != null && item.Data.Type == ItemType.Food) {

                foodStock.Add(item);
            }
        }

        return foodStock;
    }

    protected static bool InventoryHasRoom(ItemInInventory[] stock) {
        foreach (var item in stock) {
            if (item == null)
                return true;
        }

        return false;
    }
    protected static ItemInInventory FindItemInInventory(ItemInInventory[] stock, ItemData item) {
        foreach (var i in stock) {
            if (i.Data == item)
                return i;
        }

        return null;
    }
    protected static void RemoveItemFromInventory(ItemInInventory[] stock, ItemData item) {
        for (int i = 0; i < stock.Length; i++) {
            if (stock[i]?.Data == item) {
                stock[i] = null;
                return;
            }
        }
    }
    protected static void AddItemInInventory(ItemInInventory[] stock, ItemData item) {
        for (int i = 0; i < stock.Length; i++) {
            if (stock[i] == null) {
                stock[i] = new ItemInInventory(item);
                return;
            }
        }
    }

    public void SetAllItemsSelected(List<ItemData> items) {
        foreach (var item in _stock) {
            if (item != null && items.Contains(item.Data)) {
                item.Selected = true;
                items.Remove(item.Data);
            }
        }

        UiManager.Instance.RefreshInventoryDisplay(this);
    }
    public void ClearInventory() {
        for (int i = 0; i < _size ; i++) {
            _stock[i] = null;
        }
    }
}

public class ItemInInventory {
    public ItemData Data;
    public bool Equipped;
    public bool Selected;

    public ItemInInventory(ItemData item) {
        Data = item;
        Equipped = false;
        Selected = false;
    }
}