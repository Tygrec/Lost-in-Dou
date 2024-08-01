using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class CraftManager : MonoBehaviour {

    private Dictionary<ItemData, bool> _craftables = new Dictionary<ItemData, bool>();
    [SerializeField] List<ItemData> _availableCraftsAtStart;

    public ItemData CurrentCraft;
    public List<ItemData> CurrentIngredients;

    public List<ItemData> GetCurrentlyCraftables() {
        List<ItemData> knownCraftables = new List<ItemData>();

        foreach (var pair in _craftables) {
            if (pair.Value)
                knownCraftables.Add(pair.Key);
        }

        return knownCraftables;
    }
    public int GetCraftablesCount() {
        return _craftables.Count;
    }
    public Dictionary<ItemData, bool> GetAllCraftables() {
        return _craftables;
    }

    private void Start() {
        var craftList = Resources.LoadAll<ItemData>("Scriptables/Items/").Where(i => i.Craftable).ToList();
        foreach (var item in craftList) {
            _craftables.Add(item, false);
        }

        foreach (var item in _availableCraftsAtStart) {
            _craftables[item] = true;
        }
    }

    public void DiscoverCraftable(ItemData item) {
        _craftables[item] = true;
    }

    public void TryCraft() {

        bool success = false;
        foreach (var recipe in CurrentCraft.Recipes) {
            success = !recipe.Ingredients.Except(CurrentIngredients).Any() && !CurrentIngredients.Except(recipe.Ingredients).Any();

            if (success)
                break;
        }

        if (success) {

            foreach (var item in CurrentIngredients) {
                Game.G.Inv.Get(InvTag.Player).RemoveItem(item);
            }

            Game.G.Inv.Get(InvTag.Player).AddItem(CurrentCraft);

            CurrentCraft = null;
            CurrentIngredients.Clear();
            UiManager.Instance.QuitCraftDisplay();
        }
        else {
            print("On ne peut pas craft " + CurrentCraft + " avec ces objets");
        }
    }
}
