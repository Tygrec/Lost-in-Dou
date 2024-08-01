using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationInventory : Inventory
{
    public override void Initialize() {
        _size = Game.G.Values.MAX_PREPARATION_SLOTS;

        base.Initialize();

        if (Id == 1)
            _slotType = InvTag.Prep1;
        else if (Id == 2)
            _slotType = InvTag.Prep2;
        else if (Id == 3)
            _slotType = InvTag.Prep3;
        else if (Id == 4)
            _slotType= InvTag.Prep4;
    }
}
