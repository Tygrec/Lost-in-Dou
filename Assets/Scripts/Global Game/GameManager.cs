using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    private GAMESTATE _GameState = GAMESTATE.RUNNING;
    public GAMESTATE GetGameState() {
        return _GameState;
    }
    // ACTIONS & EVENEMENTS
    public Action<ItemData, ItemManager> OnPickUpItem;
    public Action OnNewDay;

    private PlayerData _playerData = new PlayerData();
    private int _relationship = 50;

    public void ChangeRelationship(RelationshipModifier modifier) {
        switch (modifier) {
            case RelationshipModifier.Positive: 
                _relationship += 10; 
                break;
            case RelationshipModifier.Negative: 
                _relationship -= 10; 
                break;
        }
    }

    public PlayerData GetPlayerData() {
        return _playerData;
    }

    public string CurrentScene = "MainScene";

    public void ChangeGameState(GAMESTATE gs) {
        _GameState = gs;
    }
}
