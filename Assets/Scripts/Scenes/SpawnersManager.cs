using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersManager : MonoBehaviour {
    void Start() {
        if (!Game.G.Db.FillSpawnerDataMapping(transform)) {
            foreach (Transform child in transform) {
                SpawnerBehavior spawner = child.GetComponent<SpawnerBehavior>();
                spawner.InitializeId();
                spawner.Initialize();
            }
        }
    }
}
