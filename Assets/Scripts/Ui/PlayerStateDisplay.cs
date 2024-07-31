using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateDisplay : MonoBehaviour
{
    [SerializeField] Image _hungerBar;
    [SerializeField] Image _thirstBar;
    [SerializeField] Image _energyBar;
    [SerializeField] Image _lifeBar;

    [SerializeField] TextMeshProUGUI _hungerInfo;
    [SerializeField] TextMeshProUGUI _thirstInfo;
    [SerializeField] TextMeshProUGUI _energyInfo;
    [SerializeField] TextMeshProUGUI _lifeInfo;

    public void Display() {
        _hungerBar.fillAmount = Game.G.Player.Hunger() / 100;
        _thirstBar.fillAmount = Game.G.Player.Thirst() / 100;
        _energyBar.fillAmount = Game.G.Player.Energy() / 100;
        _lifeBar.fillAmount = Game.G.Player.Life() / 100;

        _hungerInfo.text = Game.G.Player.Hunger().ToString() + "%";
        _thirstInfo.text = Game.G.Player.Thirst().ToString() + "%";
        _energyInfo.text = Game.G.Player.Energy().ToString() + "%";
        _lifeInfo.text = Game.G.Player.Life().ToString() + "%";
    }
}
