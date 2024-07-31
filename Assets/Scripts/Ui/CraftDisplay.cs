using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftDisplay : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _craftInfo;
    [SerializeField] Transform _craftButtonsTransform;
    [SerializeField] InventoryDisplay _craftInventoryDisplay;

    [SerializeField] Button _validateButton;
    private void Start() {
        _validateButton.onClick.AddListener(Game.G.Craft.TryCraft);
    }

    public void DisplayCraftInfo(string info) {
        _craftInfo.transform.parent.gameObject.SetActive(true);

        _craftInfo.text = info;
    }

    public void HideCraftInfo() {
        _craftInfo.transform.parent.gameObject.SetActive(false);
    }

    public void DisplayCraftSelection() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);

        UiManager.Instance.Clear(_craftButtonsTransform);
        gameObject.SetActive(true);

        foreach (var craftable in Game.G.Craft.GetAllCraftables()) {
            var button = Instantiate(Resources.Load<ButtonCraftBehavior>("Prefabs/Ui/Button Craft Item"), _craftButtonsTransform);
            button.Display(craftable.Key, craftable.Value, this);
        }
    }

    public void DisplayIngredientsSelection(ItemData item) {
        UiManager.Instance.DisplayInventory(CraftInventory.Instance);
    }

    public void QuitDisplay() {
        _craftInventoryDisplay.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
    }
}