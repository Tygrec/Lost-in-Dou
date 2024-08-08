using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] Dialog _dialog;

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player"))
            return;

        Game.G.Dialog.StartDialog(_dialog);
        Destroy(gameObject);
    }
}