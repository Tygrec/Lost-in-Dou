using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory {

    public bool WaitForItem = false;

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

        _slotType = InvTag.Player;
    }
}