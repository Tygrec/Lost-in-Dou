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

        KitchenInventory.Instance.SetFoodStock();

        UiManager.Instance.DisplayInventory(KitchenInventory.Instance);
    }

    private void DisplayPreparations() {
        int i = 0;

        foreach (var preparation in Game.G.Cook.Preparations) {

            _preparationDisplays[i].Related = preparation;
            UiManager.Instance.DisplayInventory(preparation.Inventory);
        }
    }

    public void SetCurrentPreparation(Preparation preparation) {
        print(preparation.Index);
        foreach (var prep in _preparationDisplays) {
            prep.SetSelected(prep.Related == preparation);
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
