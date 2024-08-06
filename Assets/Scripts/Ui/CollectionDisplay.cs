using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionDisplay : MonoBehaviour {
    [SerializeField] Transform _layoutItemDisplay;

    public void Display() {
        gameObject.SetActive(true);
        _layoutItemDisplay.gameObject.SetActive(false);
    }

    public void DisplayRecipes() {
        UiManager.Instance.Clear(_layoutItemDisplay);

        _layoutItemDisplay.gameObject.SetActive(true);

        foreach (var pair in Game.G.Db.GetAllRecipes()) {
            CollectionSlot slot = Instantiate(Resources.Load<CollectionSlot>("Prefabs/Ui/Collection Slot"), _layoutItemDisplay);
            slot.Display(pair.Key, pair.Value);
        }
    }
    public void DisplayCrafts() {
        UiManager.Instance.Clear(_layoutItemDisplay);

        _layoutItemDisplay.gameObject.SetActive(true);

        foreach (var pair in Game.G.Db.GetAllCrafts()) {
            CollectionSlot slot = Instantiate(Resources.Load<CollectionSlot>("Prefabs/Ui/Collection Slot"), _layoutItemDisplay);
            slot.Display(pair.Key, pair.Value);
        }
    }

    public void Back() {
        if (!_layoutItemDisplay.gameObject.activeSelf)
            Hide();
        else
            Display();
    }

    public void Hide() {
        _layoutItemDisplay.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
