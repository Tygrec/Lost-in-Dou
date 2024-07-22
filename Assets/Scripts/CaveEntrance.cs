using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (gameObject.scene.name == "CaveScene")
                SceneController.Instance.ChangeScene("MainScene");
            else if (gameObject.scene.name == "MainScene")
                SceneController.Instance.ChangeScene("CaveScene");
        }
    }
}
