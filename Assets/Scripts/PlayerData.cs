using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : HumanData {
    public ItemData EquippedItem = null;

}

public class PnjData : HumanData {
    public string CurrentScene = "CaveScene";
    public bool Follow = false;
    public Vector3 CurrentPosition = Vector3.zero;

    public PnjData() {
        CurrentScene = SceneManager.GetActiveScene().name;
    }
    public void StartFollowing() {
        Follow = true;
    }
    public void StopFollowing() {
        Follow = false;
    }
}

public class HumanData {
    public float Hunger = 50;
    public float Thirst = 50;
    public float Energy = 50;
    public float Life = 100;

    public bool IsNapping = false;
    public bool IsSleeping = false;

    private const int _hungerLoss = 2;
    private const int _thirstLoss = 3;
    private const int _energyLoss = 1;
    private const int _lifeLoss = 1;

    private const int _intervalLoss = 10;

    public HumanData() {
        Game.G.Time.RegisterRecurringCallback(UpdateStatsByTime, _intervalLoss);
    }
    private void UpdateStatsByTime() {

        Hunger -= _hungerLoss;

        Thirst -= _thirstLoss;

        if (Thirst <= 0 && !IsSleeping) {
            Life -= _lifeLoss;
        }

        Energy = IsNapping || IsSleeping ? Energy + _energyLoss : Energy - _energyLoss;

        ClampStats();
    }
    public void UpdateEnergy() {
        Energy -= _energyLoss;
    }
    public void ClampStats() {
        Hunger = Mathf.Clamp(Hunger, 0, 100);
        Thirst = Mathf.Clamp(Thirst, 0, 100);
        Energy = Mathf.Clamp(Energy, 0, 100);
        Life = Mathf.Clamp(Life, 0, 100);
    }
}