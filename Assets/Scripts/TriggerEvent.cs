using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] Dialog _dialog;
    [SerializeField] string _sceneName;

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")|| !SceneManager.GetSceneByName(_sceneName).isLoaded)
            return;

        Game.G.Dialog.StartDialog(_dialog);
        Destroy(gameObject);
    }
}