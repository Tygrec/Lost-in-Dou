using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenInventory : Inventory
{
    public override void Initialize() {

        _slotType = InvTag.Kitchen;

        SetFoodStock();
    }

    public void SetFoodStock() {
        _size = Game.G.Inv.Get(InvTag.Player).GetMaxSize() + Game.G.Inv.Get(InvTag.Stock).GetMaxSize();
        _stock = new ItemInInventory[_size];

        int i = 0;

        foreach (var item in Game.G.Inv.Get(InvTag.Player).GetFoodStock()) {
            _stock[i++] = item;
        }
        foreach(var item in Game.G.Inv.Get(InvTag.Stock).GetFoodStock()) {
            _stock[i++] = item;
        }
    }
}
