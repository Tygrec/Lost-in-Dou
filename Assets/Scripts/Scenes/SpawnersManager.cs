using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersManager : MonoBehaviour {
    void Start() {
        SpawnerBehavior[] spawners = (SpawnerBehavior[])FindObjectsOfType(typeof(SpawnerBehavior));

        if (!Game.G.Db.FillSpawnerDataMapping(spawners)) {
            foreach (SpawnerBehavior spawner in spawners) {
                spawner.InitializeId();
                spawner.Initialize();
            }
        }
    }
}
