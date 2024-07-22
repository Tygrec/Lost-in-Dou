using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplay : MonoBehaviour
{
    [SerializeField] Image _itemImg;
    [SerializeField] TextMeshProUGUI _itemNameTxt;
    [SerializeField] TextMeshProUGUI _itemDescriptionTxt;

    public void Display(ItemData item) {

        gameObject.SetActive(true);

        _itemImg.sprite = item.Sprite();
        _itemNameTxt.text = item.name;
        _itemDescriptionTxt.text = item.Description;
    }

    private void Update() {
        transform.position = Input.mousePosition;
    }
}
