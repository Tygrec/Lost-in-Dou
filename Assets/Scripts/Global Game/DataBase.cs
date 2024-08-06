using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public FireData Fire;

    private Dictionary<int, SpawnerData> _spawnerDataMapping = new Dictionary<int, SpawnerData>();
    static public int SpawnerId = 0;

    private Dictionary<RecipeData, bool> _discoveredRecipes = new Dictionary<RecipeData, bool>();
    private Dictionary<ItemData, bool> _discoveredCrafts = new Dictionary<ItemData, bool>();
    private Dictionary<ItemData, bool> _discoveredItems = new Dictionary<ItemData, bool>();


    [SerializeField] List<ItemData> _availableCraftsAtStart;
    [SerializeField] List<RecipeData> _availableRecipesAtStart;
    public Dictionary<RecipeData, bool> GetAllRecipes() { return _discoveredRecipes; }
    public Dictionary<ItemData, bool> GetAllCrafts() {  return _discoveredCrafts; }
    public Dictionary<ItemData, bool> GetAllItems() {  return _discoveredItems; }
    public List<ItemData> GetAllDiscoveredItems() {
        return _discoveredItems.Where(pair => pair.Value == true).Select(pair => pair.Key).ToList(); 
    }

    public bool IsDiscovered(ItemData item) {
        return _discoveredCrafts[item];
    }
    public bool IsDiscovered(RecipeData recipe) {
        return _discoveredRecipes[recipe];
    }

    public Action<ItemData> OnNewItem;

    public SpawnerData GetSpawnerData(int id) {
        return _spawnerDataMapping[id];
    }
    private void OnEnable() {
        Game.G.GameManager.OnNewDay += HandleNewDay;
        OnNewItem += HandleNewItem;
    }
    private void OnDisable() {
        Game.G.GameManager.OnNewDay -= HandleNewDay;
        OnNewItem -= HandleNewItem;
    }
    private void Start() {
        Fire = new FireData();

        foreach (var recipe in Resources.LoadAll<RecipeData>("Scriptables/Recipes")) {
            _discoveredRecipes.Add(recipe, false);
        }
        foreach (var item in Resources.LoadAll<ItemData>("Scriptables/Items/").Where(i => i.Craftable).ToList()) {
            _discoveredCrafts.Add(item, false);
        }
        foreach (var item in Resources.LoadAll<ItemData>("Scriptables/Items/")) {
            _discoveredItems.Add(item, false);
        }

        foreach (var item in _availableCraftsAtStart) {
            _discoveredCrafts[item] = true;
        }
        foreach (var recipe in _availableRecipesAtStart) {
            _discoveredRecipes[recipe] = true;
        }
    }

    public void Discover(ItemData item) {
        _discoveredCrafts[item] = true;
        Game.G.Player.ThinkSomething("Hé, avec ça je pourrais fabriquer un(e) " + item.name + " !");
    }
    public void Discover(RecipeData recipe) {
        _discoveredRecipes[recipe] = true;
        Game.G.Player.ThinkSomething("Je pense que je devrais pouvoir cuisiner " + recipe.name + " à présent.");
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

    public Sprite GetUnknownSprite() {
        return Resources.Load<Sprite>("Sprites/unknown");
    }

    private void HandleNewItem(ItemData item) {
        _discoveredItems[item] = true;

        List<ItemData> toDiscover = new List<ItemData>();

        foreach (var craft in _discoveredCrafts) {
            if (craft.Value)
                continue;
            
            if (craft.Key.HasEveryIngredientsDiscovered()) {
                toDiscover.Add(craft.Key);  
            }
        }
        foreach (var d in toDiscover)
            Discover(d);

        List<RecipeData> toDiscover2 = new List<RecipeData>();
        foreach (var recipe in _discoveredRecipes) {
            if (recipe.Value)
                continue;

            if (recipe.Key.HasEveryIngredientsDiscovered())
                toDiscover2.Add(recipe.Key);
        }
        foreach (var d in toDiscover2)
            Discover(d);
    }
}