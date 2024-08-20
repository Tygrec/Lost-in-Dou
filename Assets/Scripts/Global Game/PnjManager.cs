using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PnjManager : MonoBehaviour
{
    public PnjController Manager;
    private PnjData _pnjData;

    private void Start() {
        _pnjData = (PnjData)Game.G.GameManager.GetHumanData(Name.Pnj);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.T) && _pnjData.Follow)
            SwitchFollow();
    }

    public bool IsFollowing() {
        return _pnjData.Follow;
    }

    public void Sleep(bool value) {
        _pnjData.IsSleeping = value;
    }
    public void Eat(ItemData item) {
        if (Manager == null) return;

        Manager?.GetComponent<NeedsManager>().Eat(item);
    }
    public void Eat(Plate plate) {
        if (Manager == null) return;

        Manager?.GetComponent<NeedsManager>().Eat(plate);
    }
    public void Drink() {
        if (Manager == null) return;

        Manager?.GetComponent<NeedsManager>().Drink();
    }

    public void ReceiveItem(ItemData item) {
        if (item.Consommable) {
            Manager?.GetComponent<NeedsManager>().Eat(item);
            Game.G.Inv.Get(InvTag.Player).RemoveItem(item);
            Game.G.Dialog.StartDialog(DialogId.ThankfulForEat);
        }
        else {
            Game.G.Dialog.StartDialog(DialogId.NotFood);
        }
    }
    public void SwitchFollow() {
        _pnjData.Follow = !_pnjData.Follow;

        if (!_pnjData.Follow) {
            _pnjData.CurrentPosition = Manager.transform.position;
            SetAnimatorBool("isWalking", false);
        }
    }
    public void ChangeScene(string newScene) {
        if (_pnjData.Follow) {
            _pnjData.CurrentScene = newScene;
            Manager.transform.position = Game.G.Player.transform.position - Game.G.Player.transform.forward * 2;
        }
    }
    public void SetAnimatorBool(string boolName, bool value) {
        if (Manager == null)
            return;

        Manager?.GetComponent<Animator>().SetBool(boolName, value);
    }
}
