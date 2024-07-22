using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum GAMESTATE {
    RUNNING,
    PAUSE
}

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private GAMESTATE _GameState = GAMESTATE.RUNNING;
    public GAMESTATE GetGameState() {
        return _GameState;
    }
    // ACTIONS & EVENEMENTS
    public Action<ItemData, ItemManager> OnPickUpItem;
    public Action OnNewDay;

    private PlayerData _playerData = new PlayerData();
    public PlayerData GetPlayerData() 
        {
        return _playerData;
    }
    private List<Inventory> _inventories = new List<Inventory>();

    public string CurrentScene = "MainScene";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        _inventories.Add(new PlayerInventory());
        _inventories.Add(new CraftInventory());
        _inventories.Add(new StockInventory());

        foreach (var inventory in _inventories) {
            inventory.Initialize();
        }

        UiManager.Instance.Init();
    }

    public void ChangeGameState(GAMESTATE gs) {
        _GameState = gs;
    }
}
