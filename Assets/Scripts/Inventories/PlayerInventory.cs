using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : Inventory {

    public static PlayerInventory Instance;
    public void HandlePickUpItem(ItemData item, ItemManager itemManager) {
        if (AddItem(item)) {
            itemManager.RemoveFromSpawn();
            itemManager.DestroyItem();
        }
    }

    public override void Initialize() {
        _size = 16;

        base.Initialize();

        Game.G.GameManager.OnPickUpItem += HandlePickUpItem;

        Instance = this;
        _slotType = SlotType.PlayerInventory;
    }
}