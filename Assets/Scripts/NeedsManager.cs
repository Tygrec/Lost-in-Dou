using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class NeedsManager : MonoBehaviour {
    HumanData _data;
    [SerializeField] Name _name;


    private void Start() {
        _data = Game.G.GameManager.GetHumanData(_name);
    }

    public float Hunger() {
        return _data.Hunger;
    }
    public float Thirst() {
        return _data.Thirst;
    }
    public float Energy() {
        return _data.Energy;
    }
    public float Life() {
        return _data.Life;
    }
    public void LoseEnergy() {
        _data.Energy -= 1;
    }
    public void Eat(ItemData food) {
        if (food.Type != ItemType.Food) {
            Debug.LogError("ERREUR : On essaye de manger quelque chose qui n'est pas de la nourriture");
        }

        _data.Hunger += food.SatietyValue;
        _data.Thirst += food.ThirstValue;

        _data.ClampStats();
    }
    public void Eat(Plate food) {
        _data.Hunger += food.SatietyValue;
        _data.Thirst += food.ThirstValue;

        _data.ClampStats();
    }

    public void Drink(ItemData item) {
        if (item.Type != ItemType.Food) {
            Debug.LogError("ERREUR : On essaye de boire quelque chose qui n'est pas de la nourriture");
        }

        _data.Thirst += item.ThirstValue;
    }
    public void Drink() {
        _data.Thirst = 100;
    }

    public void UpdateEnergy() {
        _data.UpdateEnergy();
    }
}
