using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenInventory : Inventory
{
    public static KitchenInventory Instance;

    public override void Initialize() {
        Instance = this;

        _slotType = SlotType.Kitchen;

        SetFoodStock();
    }

    public void SetFoodStock() {
        _size = PlayerInventory.Instance.GetMaxSize() + StockInventory.Instance.GetMaxSize();
        _stock = new ItemInInventory[_size];

        int i = 0;

        foreach (var item in PlayerInventory.Instance.GetFoodStock()) {
            _stock[i++] = item;
        }
        foreach(var item in StockInventory.Instance.GetFoodStock()) {
            _stock[i++] = item;
        }
    }
}
