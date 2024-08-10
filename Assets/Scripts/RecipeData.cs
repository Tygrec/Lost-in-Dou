using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Recette", menuName = "Nouvelle recette")]
public class RecipeData : ScriptableObject, ITooltipDisplay {
    public List<ItemData> Ingredients;
    public int SatietyValue;
    public int ThirstValue;

    public Sprite Sprite() {
        return Resources.Load<Sprite>($"Sprites/Recipes/{name}");
    }
    public int GetSatiety(SuccessRate rate) {
        return SatietyValue *= (int)rate;
    }
    public int GetThirst(SuccessRate rate) {
        return ThirstValue *= (int)rate;
    }
    public bool HasEveryIngredientsDiscovered() {
        if (Ingredients.TrueForAll(item => Game.G.Db.GetAllDiscoveredItems().Contains(item)))
            return true;

        return false;
    }
}
