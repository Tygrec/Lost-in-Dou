using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum GAMESTATE {
    RUNNING,
    PAUSE
}

public class GameManager : MonoBehaviour {

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

    public string CurrentScene = "MainScene";

    public void ChangeGameState(GAMESTATE gs) {
        _GameState = gs;
    }
}
