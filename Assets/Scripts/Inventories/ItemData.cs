using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITooltipDisplay {

}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject, ITooltipDisplay {
    public ItemType Type;
    public string Description;

    public bool Consommable;
    public bool Craftable;
    public bool Equippable;

    public List<Recipe> Recipes;

    public int SatietyValue; // Uniquement pour la nourriture
    public int ThirstValue; // Uniquement pour la nourriture et l'eau

    public ItemManager Prefab() {
        return Resources.Load<ItemManager>($"Prefabs/Items/{name}");
    }
    public Sprite Sprite() {
        return Resources.Load<Sprite>($"Sprites/Items/{name}");
    }

    public bool NeedItemForRecipe(ItemData item) {

        foreach (Recipe recipe in Recipes) {
            if (recipe.Ingredients.Contains(item)) {
                return true;
            }
        }

        return false;
    }
    public bool HasEveryIngredientsDiscovered() {
        foreach (var recipe in Recipes) {
            if (recipe.Ingredients.All(item => Game.G.Db.GetAllDiscoveredItems().Contains(item)))
                return true;
        }

        return false;
    }

    public static ItemData Wood() {
        return Resources.Load<ItemData>("Scriptables/Items/Bois");
    }
    public static ItemData Spear() {
        return Resources.Load<ItemData>("Scriptables/Items/Lance");
    }
    public static ItemData Fish() {
        return Resources.Load<ItemData>("Scriptables/Items/Poisson");
    }
}

[Serializable]
public class Recipe {
    public List<ItemData> Ingredients;
}