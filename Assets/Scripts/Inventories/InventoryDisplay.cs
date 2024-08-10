using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour {

    [SerializeField]  Transform _slotsTransfom;
    public bool IsOpen = false;
    InvTag _invType;
    public void Display(Inventory inventory) {
        _invType = inventory.GetSlotType();
        gameObject.SetActive(true);
        IsOpen = true;

        Clear();

        for (int i = 0; i < inventory.GetMaxSize(); i++) {
            Slot slot = Instantiate(Resources.Load<Slot>("Prefabs/Ui/Slot"), _slotsTransfom);
            slot.SetInventory(inventory, i);

            slot.DisplayItem(inventory.GetItemAtIndex(i));
        }
    }

    public void Clear() {
        foreach (Transform child in _slotsTransfom) {
            Destroy(child.gameObject);
        }
    }

    public void ExitDisplay() {
        PlayerInventory inv = (PlayerInventory)Game.G.Inv.Get(InvTag.Player);

        if (_invType == InvTag.Player && inv.WaitForItem) {
            Game.G.Dialog.StopWaitingForItem();
        }

        IsOpen = false;
        gameObject.SetActive(false);
    }
}
