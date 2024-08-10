using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class CookManager : MonoBehaviour {

    public Preparation[] Preparations;
    private Preparation _currentPreparation;

    private void Start() {
        Preparations = new Preparation[Game.G.Values.MAX_PREPARATION];
        for (int i = 0; i < Preparations.Length; i++) {
            Preparations[i] = new Preparation(i);
        }
    }
    public void StartCooking() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        UiManager.Instance.DisplayCooking();
    }

    public void StopCooking() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
        _currentPreparation = null;

        foreach (Preparation preparation in Preparations) {
            preparation.Inventory.ClearInventory();
        }

        UiManager.Instance.HideCooking();
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
        if (foods.Count > index) {
            UiManager.Instance.DisplayPlate(foods[index]);
            Game.G.Player.Needs.Eat(foods[index]);
            Game.G.GameManager.GetPnjNeedsManager().Eat(foods[index]);

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(IEat(foods, index + 1));
        }
        else {
            RemoveAllIngredientsFromInventories();
            StopCooking();
            UiManager.Instance.HidePlate();
        }
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
    public int GetCurrentPreparationId() {
        if (_currentPreparation == null)
            return -1;

        return _currentPreparation.Id;
    }
    public Color GetCurrentPreparationColor() {
        return _currentPreparation.Color;
    }
    public Preparation GetPreparationById(int id) {
        return Preparations[id-1];
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
    public void RemoveIngredientFromPreparation(ItemData item, int id) {
        Preparations[id].Inventory.RemoveItem(item);
    }
    public void AddIngredientToPreparation(ItemData item, Preparation preparation) {
        preparation.Inventory.AddItem(item);
    }
    public void AddIngredientsToPreparation(List<ItemData> items, Preparation preparation) {
        foreach (ItemData item in items)
            preparation.Inventory.AddItem(item);
    }

    private void RemoveAllIngredientsFromInventories() {
        foreach(var preparation in Preparations) {
            foreach (var item in preparation.Inventory.GetStock()) {
                if (item == null)
                    continue;

                if (Game.G.Inv.Get(InvTag.Player).ItemExistsInInventory(item.Data)) {
                    Game.G.Inv.Get(InvTag.Player).RemoveItem(item.Data);
                }
                else if (Game.G.Inv.Get(InvTag.Stock).ItemExistsInInventory(item.Data)) {
                    Game.G.Inv.Get(InvTag.Stock).RemoveItem(item.Data);
                }
                else
                    Debug.LogError("Erreur : aucun inventaire ne contenait l'ingrédient");
            }
        }
    }
}

public class Preparation {
    public PreparationInventory Inventory;
    public Color Color;
    public int Id;

    public Preparation(int index) {
        Inventory = (PreparationInventory)Game.G.Inv.Get(InvTag.Prep1 + index);
        Id = index;
        Color = Random.ColorHSV();
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
        Sprite = recipe.Sprite();
        Name = recipe.name;
    }
}