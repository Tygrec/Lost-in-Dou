using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateDisplay : MonoBehaviour
{
    [SerializeField] Image _hungerBar;
    [SerializeField] Image _thirstBar;
    [SerializeField] Image _energyBar;
    [SerializeField] Image _lifeBar;

    public void Display() {
        _hungerBar.fillAmount = PlayerController.Instance.Hunger() / 100;
        _thirstBar.fillAmount = PlayerController.Instance.Thirst() / 100;
        _energyBar.fillAmount = PlayerController.Instance.Energy() / 100;
        _lifeBar.fillAmount = PlayerController.Instance.Life() / 100;
    }
}
