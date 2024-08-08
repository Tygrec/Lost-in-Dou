using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class CraftManager : MonoBehaviour {

    public ItemData CurrentCraft;
    public List<ItemData> CurrentIngredients;

    public List<ItemData> GetCurrentlyCraftables() {
        List<ItemData> knownCraftables = new List<ItemData>();

        foreach (var pair in Game.G.Db.GetAllCrafts()) {
            if (pair.Value)
                knownCraftables.Add(pair.Key);
        }

        return knownCraftables;
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

            CurrentCraft.Durability = CurrentIngredients.Sum(item => item.Durability);
            Game.G.Inv.Get(InvTag.Player).AddItem(CurrentCraft);

            CurrentCraft = null;
            CurrentIngredients.Clear();
            UiManager.Instance.HideCraft();
        }
        else {
            print("On ne peut pas craft " + CurrentCraft + " avec ces objets");
        }
    }
}
