using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftInventory : Inventory
{
    public static CraftInventory Instance;

    public override void Initialize() {
        _size = 16;

        base.Initialize();

        Instance = this;
        _slotType = SlotType.Craft;

        _stock = PlayerInventory.Instance.GetStock();
    }
}
