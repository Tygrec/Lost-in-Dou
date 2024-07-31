using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public enum CookingState {
    CHOICE,
    RECIPE,
    INGREDIENTS,
    RESULT,
    EATING
}

public class CookManager : MonoBehaviour {

    static public int Index = 0;

    public Preparation[] Preparations;
    private Preparation _currentPreparation;

    private void Start() {
        Preparations = new Preparation[Game.G.Values.MAX_PREPARATION];
        for (int i = 0; i < Preparations.Length; i++) {
            Preparations[i] = new Preparation();
            UiManager.Instance.MapPreparationDisplay(Preparations[i].Inventory, i);
        }

        print("Préparations initialisées");
    }
    public void StartCooking() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        UiManager.Instance.DisplayCooking();
    }

    public void StopCooking() {
        print("Fin de la séance de cuisine");
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
    }

    public void CookAndEat() {
        List<Plate> plates = new List<Plate>();
        List<Preparation> okPrep = Preparations.Where(prep => prep.Inventory.GetStock().Any(element => element != null)).ToList();
        
        print($"On mange {okPrep.Count} plats");

        foreach (var preparation in okPrep) {
            plates.Add(new Plate(preparation.Inventory.GetStock().Where(item => item != null).Select(item => item.Data).ToList(), SuccessRate.Success));
        }
        StartCoroutine(IEat(plates, 0));
    }

    private IEnumerator IEat(List<Plate> foods, int index) {
        yield return new WaitForSeconds(0.5f);

        print("On mange " + foods[index]);

        if (foods.Count > index)
            StartCoroutine(IEat(foods, index + 1));
        else
            StopCooking();
    }

    public RecipeData FindRecipeForCurrentIngredients(List<ItemData> ingredients) {

        foreach (var recipe in Game.G.Db.GetAllRecipes()) {
            if (recipe.Key.Ingredients.All(item => ingredients.Contains(item)))
                return recipe.Key;
        }

        return null;
    }

    public void ChangeCurrentPreparation(Preparation preparation) {
        
        _currentPreparation = preparation;

        // TODO : à changer, ce n'est pas à l'UI manager de gérer ça
        UiManager.Instance.SetCurrentPreparation(preparation);
    }
    public void AddIngredientToCurrentPreparation(ItemData item) {
        if (_currentPreparation == null)
            return;

        _currentPreparation.Inventory.AddItem(item);
    }
    public void RemoveIngredientFromCurrentPreparation(ItemData item) {
        if (_currentPreparation == null)
            return;

        _currentPreparation.Inventory.RemoveItem(item);
    }
}

public class Preparation {
    public PreparationInventory Inventory;
    public int Index;

    public Preparation() {
        Inventory = new PreparationInventory();
        Inventory.Initialize();
        Index = CookManager.Index++;
    }
}
public class Plate {
    public int ThirstValue;
    public int SatietyValue;
    public Sprite Sprite;
    public string Name;

    public Plate(RecipeData recipe, SuccessRate success) {
        SetPlate(recipe, success);
    }
    public Plate(List<ItemData> ingredients, SuccessRate success) {
        if (Game.G.Cook.FindRecipeForCurrentIngredients(ingredients) != null) {
            SetPlate(Game.G.Cook.FindRecipeForCurrentIngredients(ingredients), success);
            return;
        }

        foreach (ItemData item in ingredients) {
            SatietyValue += item.SatietyValue * (int)success;
            ThirstValue += item.ThirstValue * (int)success;
        }
        Sprite = ingredients[0].Sprite();
        Name = $"Brochettes de {ingredients[0].name}";
    }

    private void SetPlate(RecipeData recipe, SuccessRate success) {
        ThirstValue = recipe.GetThirst(success);
        SatietyValue = recipe.GetSatiety(success);
        Sprite = recipe.Sprite;
        Name = recipe.name;
    }
}