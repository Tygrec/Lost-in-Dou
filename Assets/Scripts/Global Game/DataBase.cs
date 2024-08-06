using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public FireData Fire;

    private Dictionary<int, SpawnerData> _spawnerDataMapping = new Dictionary<int, SpawnerData>();
    static public int SpawnerId = 0;

    private Dictionary<RecipeData, bool> _discoveredRecipes = new Dictionary<RecipeData, bool>();
    public Dictionary<RecipeData, bool> GetAllRecipes() { return _discoveredRecipes; }
    public SpawnerData GetSpawnerData(int id) {
        return _spawnerDataMapping[id];
    }
    private void OnEnable() {
        Game.G.GameManager.OnNewDay += HandleNewDay;
    }
    private void OnDisable() {
        Game.G.GameManager.OnNewDay -= HandleNewDay;
    }
    private void Start() {
        Fire = new FireData();

        foreach (var recipe in Resources.LoadAll<RecipeData>("Scriptables/Recipes")) {
            _discoveredRecipes.Add(recipe, true);
        }
    }
    public bool FillSpawnerDataMapping(Transform spawnersTransform) {
        if (_spawnerDataMapping.Count > 0)
            return false;

        foreach (Transform child in spawnersTransform) {
            SpawnerBehavior spawner = child.GetComponent<SpawnerBehavior>();

            InitializeSpawnerId(spawner);

            SpawnerData data = new SpawnerData(spawner);
            _spawnerDataMapping.Add(spawner.Id, data);

            InitializeSpawner(spawner);
        }
        return true;
    }

    public void InitializeSpawner(SpawnerBehavior spawner) {
        spawner.Initialize();
    }
    public void InitializeSpawnerId(SpawnerBehavior spawner) {
        spawner.InitializeId();
    }

    public void SaveSpawnersId() {
        SpawnerId = 0;
    }
    private void HandleNewDay() {
        foreach (var spawner in _spawnerDataMapping) {
            foreach (var item in spawner.Value.Spawnables) {
                spawner.Value.TryAdd(item);
            }
        }
    }
}
