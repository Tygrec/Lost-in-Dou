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

    public PnjManager Pnj;

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

    public bool PnjIsFollowing() {
        return _pnjData.Follow;
    }
    public NeedsManager GetPnjNeedsManager() {
        return Pnj?.GetComponent<NeedsManager>();
    }

    public void PnjReceiveItem(ItemData item) {
        if (item.Consommable) {
            Pnj?.GetComponent<NeedsManager>().Eat(item);
            Game.G.Inv.Get(InvTag.Player).RemoveItem(item);
            Game.G.Dialog.StartDialog(DialogId.ThankfulForEat);
        }
        else {
            Game.G.Dialog.StartDialog(DialogId.NotFood);
        }
    }
    public void SwitchPnjFollow() {
        _pnjData.Follow = !_pnjData.Follow;
        
        if(!_pnjData.Follow) {
            _pnjData.CurrentPosition = Game.G.GameManager.Pnj.transform.position;
            print(_pnjData.CurrentPosition);
        }
    }
    public void ChangePnjScene(string newScene) {
        if (_pnjData.Follow) {
            _pnjData.CurrentScene = newScene;
        }
    }
}
