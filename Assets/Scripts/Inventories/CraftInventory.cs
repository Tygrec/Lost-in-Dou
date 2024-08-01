using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftInventory : Inventory
{
    public override void Initialize() {
        _size = 16;

        base.Initialize();

        _slotType = InvTag.Craft;

        _stock = Game.G.Inv.Get(InvTag.Player).GetStock();
    }
}
