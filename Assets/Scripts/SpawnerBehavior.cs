using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerData {
    public List<ItemData> ItemsToSpawn = new List<ItemData>();
    public List<ItemData> Spawnables = new List<ItemData>();
    private int _maxSpawn = 4;

    public SpawnerData(SpawnerBehavior spawner) { 
        foreach (var item in spawner.GetSpawnables()) {
            Spawnables.Add(item);
            ItemsToSpawn.Add(item);
        }
    }
    public void TryAdd(ItemData item) {
        if (ItemsToSpawn.Count < _maxSpawn)
            ItemsToSpawn.Add(item);
    }
}

public class SpawnerBehavior : MonoBehaviour {
    private SpawnerData _data;
    [SerializeField] private Renderer _renderer;
    public int Id;
    public SpawnerData GetData() {
        return _data;
    }
    [SerializeField] List<ItemData> _spawnables;
    public List<ItemData> GetSpawnables() {
        return _spawnables;
    }

    [SerializeField] LayerMask _layerMask;
    public int Index;

    public void InitializeId() {
        Id = DataBase.SpawnerId++;
    }
    public void Initialize() {
        _data = Game.G.Db.GetSpawnerData(Id);

        foreach (var item in _data.ItemsToSpawn) {
            TrySpawn(item);
        }
    }

    private ItemManager TrySpawn(ItemData item) {

        Vector3 randomPosition = GetRandomPosition();
        if (randomPosition != Vector3.zero) {
            ItemManager obj = Instantiate(item.Prefab(), transform);
            obj.SetSpawner(this, randomPosition);
            return obj;
        }
        else {
            Debug.Log("Failed to find a valid position.");
            return null;
        }
    }

    private Vector3 GetRandomPosition() {

        for (int i = 0; i < 20; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * 2;
            randomDirection += transform.position;
            randomDirection.x += _renderer.bounds.size.x / 2;
            randomDirection.z += _renderer.bounds.size.z / 2;
            randomDirection.y = 2;

            // Check for collisions
            if (!Physics.CheckSphere(randomDirection, 0.5f, _layerMask)) {
                return randomDirection;
            }
        }
        // Return Vector3.zero if no valid position is found
        return Vector3.zero;
    }
}
