using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _itemImg;

    ItemData _item;
    RecipeData _recipe;

    public void Display(ItemData item, bool discovered) {
        _itemImg.sprite = discovered ? item.Sprite() : Game.G.Db.GetUnknownSprite();
        _item = item;
    }
    public void Display(RecipeData recipe, bool discovered) {
        _itemImg.sprite = discovered ? recipe.Sprite() : Game.G.Db.GetUnknownSprite();
        _recipe = recipe;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (_item != null && Game.G.Db.IsDiscovered(_item))
            UiManager.Instance.DisplayTooltip(_item);
        else if (_recipe != null && Game.G.Db.IsDiscovered(_recipe))
            UiManager.Instance.DisplayTooltip(_recipe);
    }

    public void OnPointerExit(PointerEventData eventData) {
        UiManager.Instance.HideTooltip();
    }
}
