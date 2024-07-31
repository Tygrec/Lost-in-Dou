using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationInventory : Inventory
{
    public override void Initialize() {
        _size = Game.G.Values.MAX_PREPARATION_SLOTS;

        base.Initialize();

        _slotType = SlotType.Preparation;
    }
}
