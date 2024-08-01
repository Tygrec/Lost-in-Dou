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
        foreach (Transform child in  _recipesButtonTransform) {
            Destroy(child.gameObject);
        }

        foreach (var recipe in Game.G.Db.GetAllRecipes()) {

            var button = Instantiate(Resources.Load<Button>("Prefabs/Ui/Button"), _recipesButtonTransform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = recipe.Key.name;
        }
    }
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

    public void DisplayPlate(Plate plate) {

        _resultName.text = plate.Name;
        _resultImage.sprite = plate.Sprite;

        _result.SetActive(true);
    }

    public void QuitDisplay() {
        gameObject.SetActive(false);
    }

    public void AbandonCooking() {
        gameObject.SetActive(false);
        Game.G.Cook.StopCooking();
    }
}
