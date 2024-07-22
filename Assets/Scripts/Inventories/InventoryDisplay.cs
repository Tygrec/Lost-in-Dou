using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour {

    [SerializeField]  Transform _slotsTransfom;
    public bool IsOpen = false;
    public void Display(Inventory inventory) {

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
        IsOpen = false;
        gameObject.SetActive(false);
    }
}
