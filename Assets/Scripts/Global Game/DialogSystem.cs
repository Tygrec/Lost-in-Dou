using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    Dictionary<DialogId, Dialog> m_Dialogs = new Dictionary<DialogId, Dialog>();
    private void Start() {
        foreach (var dialog in Resources.LoadAll<Dialog>("Scriptables/Dialogs")) {
            m_Dialogs.Add(dialog.Id, dialog);
        }
    }

    public void StartDialog(DialogId id) {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        DialogDisplayManager.D.DisplayDialog(m_Dialogs[id]);
    }

    public void StopDialog() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
    }
}