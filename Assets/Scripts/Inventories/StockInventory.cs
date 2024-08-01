using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockInventory : Inventory {
    public override void Initialize() {
        _size = 6;

        base.Initialize();

        _slotType = InvTag.Stock;
    }
}
