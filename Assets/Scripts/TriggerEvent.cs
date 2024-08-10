using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] Dialog _dialog;
    [SerializeField] string _sceneName;
    [SerializeField] bool _needApollo;

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")|| !SceneManager.GetSceneByName(_sceneName).isLoaded)
            return;
        if (_needApollo && !Game.G.GameManager.PnjIsFollowing())
            return;

        Game.G.Dialog.StartDialog(_dialog);
        Destroy(gameObject);
    }
}