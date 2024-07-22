using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehavior : MonoBehaviour {
    [SerializeField] List<ItemData> _spawnables;

    [SerializeField] LayerMask _layerMask;
    public int Index;
    private void Start() {
        foreach (var item in _spawnables) {
            TrySpawn(item);
        }
    }

    private ItemManager TrySpawn(ItemData item) {

        Vector3 randomPosition = GetRandomPosition();
        if (randomPosition != Vector3.zero) {
            ItemManager obj = Instantiate(item.Prefab(), transform);
            obj.transform.position = randomPosition;
            return obj;
        }
        else {
            Debug.Log("Failed to find a valid position.");
            return null;
        }
    }

    private Vector3 GetRandomPosition() {
        for (int i = 0; i < 5; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * 2;
            randomDirection += transform.position;
            randomDirection.y = 0;

            // Check for collisions
            if (!Physics.CheckSphere(randomDirection, 0.5f, _layerMask)) {
                return randomDirection;
            }
        }
        // Return Vector3.zero if no valid position is found
        return Vector3.zero;
    }
}
