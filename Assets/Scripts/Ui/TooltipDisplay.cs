using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplay : MonoBehaviour
{
    [SerializeField] GameObject _item;
    [SerializeField] GameObject _recipe;

    [SerializeField] Image _itemImg;
    [SerializeField] TextMeshProUGUI _itemNameTxt;
    [SerializeField] TextMeshProUGUI _itemDescriptionTxt;

    [SerializeField] TextMeshProUGUI _recipeNameTxt;
    [SerializeField] Image _recipeImg;
    [SerializeField] Transform _ingredientsLayout;

    public void Display(ItemData item) {
        gameObject.SetActive(true);
        _item.SetActive(true);

        _itemImg.sprite = item.Sprite();
        _itemNameTxt.text = item.name;
        _itemDescriptionTxt.text = item.Description;
    }

    public void Display(RecipeData recipe) {
        gameObject.SetActive(true);
        _recipe.SetActive(true);

        _recipeImg.sprite = recipe.Sprite;
        _recipeNameTxt.text = recipe.name;

        foreach (var ingredient in recipe.Ingredients) {
            Image img = Instantiate(Resources.Load<Image>("Prefabs/Ui/Image"), _ingredientsLayout);
            img.sprite = ingredient.Sprite();
        }
    }

    public void Hide() {
        UiManager.Instance.Clear(_ingredientsLayout);
        _item.SetActive(false);
        _recipe.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update() {
        transform.position = Input.mousePosition;
    }
}
