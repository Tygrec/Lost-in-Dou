using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockInventory : Inventory {
    public static StockInventory Instance;
    public override void Initialize() {
        _size = 6;

        base.Initialize();

        Instance = this;
        _slotType = SlotType.OtherInventory;
    }
}
