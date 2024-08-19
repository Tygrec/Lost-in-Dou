using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingDisplay : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _title;

    [SerializeField] GameObject _result;
    [SerializeField] TextMeshProUGUI _resultName;
    [SerializeField] Image _resultImage;

    [SerializeField] Transform _recipesButtonTransform;
    [SerializeField] Transform _ingredientsSlotsTransform;

    [SerializeField] List<PreparationDisplay> _preparationDisplays;

    public void Display() {
        gameObject.SetActive(true);
        DisplayRecipes();
        DisplayIngredients();
        DisplayPreparations();

    }
    private void DisplayRecipes() {
        foreach (Transform child in _recipesButtonTransform) {
            Destroy(child.gameObject);
        }

        foreach (var pair in Game.G.Db.GetAllRecipes()) {
            if (!pair.Value)
                continue;

            var button = Instantiate(Resources.Load<Button>("Prefabs/Ui/Button"), _recipesButtonTransform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = pair.Key.name;
            button.onClick.AddListener(() => { SetRecipeInPreparation(pair.Key); });
        }
    }
    // Affiche des slots contenant toutes la nourriture que possède le joueur (changer l'inventaire)
    private void DisplayIngredients() {
        KitchenInventory kitchen = (KitchenInventory)Game.G.Inv.Get(InvTag.Kitchen);
        kitchen.SetFoodStock();

        UiManager.Instance.DisplayInventory(Game.G.Inv.Get(InvTag.Kitchen));
    }

    
    private void DisplayPreparations() {

        for (int i = 0; i < _preparationDisplays.Count; i++) {
            _preparationDisplays[i].SetPreparation(Game.G.Cook.Preparations[i]);
            UiManager.Instance.DisplayInventory(Game.G.Cook.Preparations[i].Inventory);
        }
    }

    public void SetCurrentPreparation(Preparation preparation) {

        foreach (var display in _preparationDisplays) {
            display.SetSelected(display.GetPreparation() == preparation);
        }
    }

    private void SetRecipeInPreparation(RecipeData recipe) {
        Preparation freePrep = null;
        foreach (var prep in Game.G.Cook.Preparations) {
            if (prep.Inventory.GetCurrentSize() == 0)
                freePrep = prep;
        }

        if (freePrep == null) {
            Debug.Log("Pas de préparation disponible");
            return;
        }

        Game.G.Cook.ChangeCurrentPreparation(freePrep);

        List<ItemData> items = new List<ItemData>();
        foreach (var item in recipe.Ingredients) {
            if (Game.G.Inv.Get(InvTag.Kitchen).ItemExistsInInventory(item))
                items.Add(item);
            else {
                Debug.Log("Il manque au moins un item pour la recette");
                return;
            }
        }

        Game.G.Cook.AddIngredientsToPreparation(items, freePrep);
        Game.G.Inv.Get(InvTag.Kitchen).SetAllItemsSelected(items);


    }

    public void DisplayPlate(Plate plate) {

        _resultName.text = plate.Name;
        _resultImage.sprite = plate.Sprite;

        _result.SetActive(true);
    }
    public void HidePlate() {
        _result.SetActive(false);
    }
    // Clic sur le bouton Valider
    public void Validate() {
        Game.G.Cook.CookAndEat();
    }
    public void QuitDisplay() {
        gameObject.SetActive(false);
        _ingredientsSlotsTransform.gameObject.SetActive(false);
        foreach (var prep in _preparationDisplays)
            prep.gameObject.SetActive(false);
    }
    public void AbandonCooking() {
        gameObject.SetActive(false);
        Game.G.Cook.StopCooking();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            AbandonCooking();
        }
    }
}