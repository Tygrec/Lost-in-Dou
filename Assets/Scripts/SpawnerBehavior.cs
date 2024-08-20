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
            ItemManager obj = Instantiate(item.Prefab(), transform.parent);
            obj.SetSpawner(this, randomPosition);
            return obj;
        }
        else {
            Debug.Log("Failed to find a valid position : " + gameObject.transform.parent.name);
            return null;
        }
    }

    private Vector3 GetRandomPosition() {

        for (int i = 0; i < 20; i++) {
            Vector3 center = transform.position;
            Vector3 size = transform.localScale;
            float itemRay = 0.2f;

            // Générer des coordonnées aléatoires dans les limites du cube
            float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
            float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

            Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);

            // Check for collisions
            if (!Physics.CheckBox(randomPosition + new Vector3(0, itemRay, 0), new Vector3(itemRay, itemRay, itemRay), Quaternion.identity, _layerMask)) {
                return randomPosition;
            }
        }
        // Return Vector3.zero if no valid position is found
        return Vector3.zero;
    }
}
