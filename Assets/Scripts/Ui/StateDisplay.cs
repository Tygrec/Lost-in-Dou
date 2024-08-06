using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateDisplay : MonoBehaviour
{
    [SerializeField] Name _name;
    private HumanData _data;

    [SerializeField] TextMeshProUGUI _nameText;

    [SerializeField] Image _hungerBar;
    [SerializeField] Image _thirstBar;
    [SerializeField] Image _energyBar;
    [SerializeField] Image _lifeBar;

    [SerializeField] TextMeshProUGUI _hungerInfo;
    [SerializeField] TextMeshProUGUI _thirstInfo;
    [SerializeField] TextMeshProUGUI _energyInfo;
    [SerializeField] TextMeshProUGUI _lifeInfo;

    private void Start() {
        _data = Game.G.GameManager.GetHumanData(_name);
        _nameText.text = _name == Name.Player ? Game.G.Values.PLAYER_NAME : Game.G.Values.PNJ_NAME;
    }

    public void Display() {
        _hungerBar.fillAmount = _data.Hunger / 100;
        _thirstBar.fillAmount = _data.Thirst / 100;
        _energyBar.fillAmount = _data.Energy / 100;
        _lifeBar.fillAmount = _data.Life / 100;

        _hungerInfo.text = _data.Hunger.ToString() + "%";
        _thirstInfo.text = _data.Thirst.ToString() + "%";
        _energyInfo.text = _data.Energy.ToString() + "%";
        _lifeInfo.text = _data.Life.ToString() + "%";
    }
}
