using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    Dictionary<DialogId, Dialog> m_Dialogs = new Dictionary<DialogId, Dialog>();
    public bool ShowingSuccess = true;

    [HideInInspector] public ItemData ItemChosen = null;
    private void OnEnable() {
        EventHub.Listen(EventType.GiveItemToPnj, HandleGiveItem);
        EventHub.Listen(EventType.ShowObjectToPnj, HandleShowItem);
    }
    private void OnDisable() {
        EventHub.Unlisten(EventType.GiveItemToPnj, HandleGiveItem);
        EventHub.Unlisten(EventType.ShowObjectToPnj, HandleShowItem);
    }

    private void Start() {
        foreach (var dialog in Resources.LoadAll<Dialog>("Scriptables/Dialogs")) {
            m_Dialogs.Add(dialog.Id, dialog);
        }
    }

    public void StartDialog(DialogId id) {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        Game.G.Player.Controller.TalkAnimation(true);
        DialogDisplayManager.D.DisplayDialog(m_Dialogs[id]);
    }
    public void StartDialog(Dialog dialog) {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        Game.G.Player.Controller.TalkAnimation(true);
        Game.G.Pnj.SetAnimatorBool("isTalking", true);
        DialogDisplayManager.D.DisplayDialog(dialog);
    }

    public void StopDialog() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
        Game.G.Player.Controller.TalkAnimation(false);
        Game.G.Pnj.SetAnimatorBool("isTalking", false);

    }

    public void StopShowingGame() {
        if (ShowingSuccess)
            Game.G.Db.Analyze(ItemChosen);

        ItemChosen = null;
        ShowingSuccess = true;
    }

    public void StartShowingGame(ItemData item) {
        if (!item.Showable) {
            StartDialog(DialogId.NotShowable);
            return;
        }

        StartCoroutine(IShowSprite(item));
    }

    private void HandleGiveItem(object payload = null) {
        UiManager.Instance.OpenInventoryToChose();
        StartCoroutine(IWaitForItem(Game.G.Pnj.ReceiveItem));
    }
    private void HandleShowItem(object payload = null) {
        UiManager.Instance.OpenInventoryToChose();
        StartCoroutine(IWaitForItem(StartShowingGame));
    }
    private IEnumerator IWaitForItem(Action<ItemData> action) {
        ItemChosen = null;

        yield return new WaitUntil(() => ItemChosen != null);

        
        action.Invoke(ItemChosen);
    }
    public void StopWaitingForItem() {
        StopAllCoroutines();
        PlayerInventory inv = (PlayerInventory)Game.G.Inv.Get(InvTag.Player);
        inv.WaitForItem = false;
        StartDialog(DialogId.Surprised);
    }

    private IEnumerator IShowSprite(ItemData item) {
        UiManager.Instance.DisplayItemShowedSprite(item);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        UiManager.Instance.HideItemShowedSprite();
        DialogDisplayManager.D.DisplayDialog(Game.G.Db.GetShowing(item));
    }
}