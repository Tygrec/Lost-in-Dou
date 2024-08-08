using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    Dictionary<DialogId, Dialog> m_Dialogs = new Dictionary<DialogId, Dialog>();
    public bool ShowingSuccess = true;
    private bool _isShowing = false;

    public ItemData ItemChosen = null;
    private void OnEnable() {
        EventHub.Listen(EventType.GiveItemToPnj, HandleGiveItem);
        EventHub.Listen(EventType.ShowObjectToPnj, HandleShowItem);
    }
    private void OnDisable() {
        
    }

    private void Start() {
        foreach (var dialog in Resources.LoadAll<Dialog>("Scriptables/Dialogs")) {
            m_Dialogs.Add(dialog.Id, dialog);
        }
    }

    public void StartDialog(DialogId id) {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        DialogDisplayManager.D.DisplayDialog(m_Dialogs[id]);
    }
    public void StartDialog(Dialog dialog) {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        DialogDisplayManager.D.DisplayDialog(dialog);
    }

    public void StopDialog() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
    }

    public void StopShowingGame() {
        print("Fin : " + ShowingSuccess);
        if (ShowingSuccess)
            Game.G.Db.Analyze(ItemChosen);

        _isShowing = false;
        ItemChosen = null;
        ShowingSuccess = true;
    }

    public void StartShowingGame(ItemData item) {
        if (!item.Showable) {
            Debug.LogError("Erreur : l'item ne peut pas être montré");
            return;
        }

        StartCoroutine(IShowSprite(item));
    }

    private void HandleGiveItem(object payload = null) {
        UiManager.Instance.OpenInventoryToChose();
        StartCoroutine(IWaitForItem(Game.G.GameManager.PnjReceiveItem));
    }
    private void HandleShowItem(object payload = null) {
        UiManager.Instance.OpenInventoryToChose();
        StartCoroutine(IWaitForItem(StartShowingGame));
    }
    private IEnumerator IWaitForItem(Action<ItemData> action) {
        yield return new WaitUntil(() => ItemChosen != null);

        action.Invoke(ItemChosen);

        ItemChosen = null;
    }

    private IEnumerator IShowSprite(ItemData item) {
        UiManager.Instance.DisplayItemShowedSprite(item);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        UiManager.Instance.HideItemShowedSprite();
        DialogDisplayManager.D.DisplayDialog(Game.G.Db.GetShowing(item));
        _isShowing = true;
    }
}