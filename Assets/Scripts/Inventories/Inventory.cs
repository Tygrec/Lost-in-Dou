using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory {

    protected ItemInInventory[] _stock;
    protected int _size;
    protected SlotType _slotType;

    public int GetMaxSize() {
        return _size;
    }
    public ItemInInventory GetItemAtIndex(int index) {
        return _stock[index];
    }
    public int GetCurrentSize() {
        return _stock.Length;
    }
    public SlotType GetSlotType() {
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

        return true;
    }

    // Retourne FALSE s'il n'y a pas assez d'item à retirer de l'inventaire
    public virtual bool RemoveItem(ItemData item) {

        if (!ItemExistsInInventory(_stock, item)) {
            return false;
        }


        RemoveItemFromInventory(_stock, item);

        UiManager.Instance.OnInventoryChanged.Invoke(this);
        return true;
    }

    // Fonctions utilitaires pour manipuler l'inventaire
    protected static bool ItemExistsInInventory(ItemInInventory[] stock, ItemData item) {
        foreach (var i in stock) {
            if (i == null)
                continue;
            else if (i.Data == item)
                return true;
        }

        return false;
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
}

public class ItemInInventory {
    public ItemData Data;
    public bool Equipped;

    public ItemInInventory(ItemData item) {
        Data = item;
        Equipped = false;
    }
}