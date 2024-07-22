using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCraftBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] Image _itemImg;
    [SerializeField] Sprite _unknown;
    private CraftDisplay _craftDisplay;

    private ItemData _item;
    private bool _discovered;

    public void Display(ItemData item, bool discovered, CraftDisplay craftDisplay) {
        _craftDisplay = craftDisplay;
        _item = item;
        _discovered = discovered;

        if (discovered) {
            _itemImg.sprite = item.Sprite();
            GetComponent<Button>().onClick.AddListener(() => {
                CraftManager.Instance.CurrentCraft = item;
                _craftDisplay.DisplayIngredientsSelection(item);
            });
        }
        else
            _itemImg.sprite = _unknown;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (_discovered)
            _craftDisplay.DisplayCraftInfo($"Fabriquer {_item.name}");
        else
            _craftDisplay.DisplayCraftInfo("Craft pas encore découvert");
    }

    public void OnPointerExit(PointerEventData eventData) {
        _craftDisplay.HideCraftInfo();
    }


}
