using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    Dictionary<InvTag, Inventory> _inventories = new Dictionary<InvTag, Inventory>();
    Dictionary<Inventory, InventoryDisplay> _inventoryDisplays = new Dictionary<Inventory, InventoryDisplay>();
    public Inventory Get(InvTag tag) {
        return _inventories[tag];
    }
    public InventoryDisplay GetDisplay(InvTag tag) {
        return _inventoryDisplays[_inventories[tag]];
    }
    public InventoryDisplay GetDisplay(Inventory inventory) {
        return _inventoryDisplays[inventory];
    }

    private void Start() {
        _inventories.Add(InvTag.Player, new PlayerInventory());
        _inventories.Add(InvTag.Stock, new StockInventory());
        _inventories.Add(InvTag.Craft, new CraftInventory());
        _inventories.Add(InvTag.Kitchen, new KitchenInventory());

        _inventoryDisplays.Add(_inventories[InvTag.Player], UiManager.Instance.PlayerInventoryDisplay);
        _inventoryDisplays.Add(_inventories[InvTag.Stock], UiManager.Instance.StockInventoryDisplay);
        _inventoryDisplays.Add(_inventories[InvTag.Craft], UiManager.Instance.CraftInventoryDisplay);
        _inventoryDisplays.Add(_inventories[InvTag.Kitchen], UiManager.Instance.KitchenInventoryDisplay);

        SetPreparations();

        foreach (var inventory in _inventories.Values) {
            inventory.Initialize();
        }
    }

    private void SetPreparations() {
        _inventories.Add(InvTag.Prep1, new PreparationInventory());
        _inventories.Add(InvTag.Prep2, new PreparationInventory());
        _inventories.Add(InvTag.Prep3, new PreparationInventory());
        _inventories.Add(InvTag.Prep4, new PreparationInventory());

        _inventories[InvTag.Prep1].Id = 1;
        _inventories[InvTag.Prep2].Id = 2;
        _inventories[InvTag.Prep3].Id = 3;
        _inventories[InvTag.Prep4].Id = 4;

        _inventoryDisplays.Add(_inventories[InvTag.Prep1], UiManager.Instance.Prep1Display);
        _inventoryDisplays.Add(_inventories[InvTag.Prep2], UiManager.Instance.Prep2Display);
        _inventoryDisplays.Add(_inventories[InvTag.Prep3], UiManager.Instance.Prep3Display);
        _inventoryDisplays.Add(_inventories[InvTag.Prep4], UiManager.Instance.Prep4Display);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            foreach (var inventory in _inventoryDisplays.Values) {
                inventory.ExitDisplay();
            }
        }
    }
}
