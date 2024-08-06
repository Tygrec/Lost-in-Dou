using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (gameObject.scene.name == "CaveScene")
                Game.G.Scene.ChangeScene("CaveScene", "MainScene");
            else
                Game.G.Scene.ChangeScene("MainScene", "CaveScene");
        }
    }
}
