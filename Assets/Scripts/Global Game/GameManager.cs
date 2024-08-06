using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GAMESTATE _GameState = GAMESTATE.RUNNING;
    public GAMESTATE GetGameState() {
        return _GameState;
    }
    // ACTIONS & EVENEMENTS
    public Action<ItemData, ItemManager> OnPickUpItem;
    public Action OnNewDay;

    private PlayerData _playerData;
    private PnjData _pnjData;
    private Dictionary<Name, HumanData> _mappingHumanData = new Dictionary<Name, HumanData>();

    private int _relationship = 50;
    private void Awake() {
        _playerData = new PlayerData();
        _pnjData = new PnjData();

        _mappingHumanData.Add(Name.Player, _playerData);
        _mappingHumanData.Add(Name.Pnj, _pnjData);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T) && _pnjData.Follow)
            SwitchPnjFollow();
    }

    public HumanData GetHumanData(Name name) {
        return _mappingHumanData[name];
    }

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

    public void ChangeGameState(GAMESTATE gs) {
        _GameState = gs;
    }

    public void SwitchPnjFollow() {
        _pnjData.Follow = !_pnjData.Follow;
    }
    public void ChangePnjScene(string newScene) {
        if (_pnjData.Follow) {
            _pnjData.CurrentScene = newScene;
        }
    }
}
