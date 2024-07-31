using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour {
    [SerializeField] ItemData _data;

    private SpawnerBehavior _spawner;
    private void Start() {
        if (_data == null) {
            _data = Resources.Load<ItemData>($"Scriptables/Items/{name}");
        }

        if (_data == null) {
            Debug.LogError("Attention, le prefab " + gameObject.name + " n'a pas de data associé");
        }
    }

    public ItemData GetItem() {
        return _data;
    }
    public void SetSpawner(SpawnerBehavior spawner, Vector3 position) {
        _spawner = spawner;
        transform.position = position;
    }
    public void RemoveFromSpawn() {
        if (_spawner == null)
            return;

        _spawner.GetData().ItemsToSpawn.Remove(_data);
    }

    public void DestroyItem() {
        Destroy(gameObject);
    }
}