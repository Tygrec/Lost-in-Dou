using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : HumanData {
    public ItemData EquippedItem = null;

}

public class PnjData : HumanData {
    public string CurrentScene = Game.G.Values.STARTING_PNJ_SCENE;
    public bool Follow = false;
    public Vector3 CurrentPosition = Game.G.Values.INITIAL_PNJ_POSITION;

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


    private const int _intervalLoss = 10;

    public HumanData() {
        Game.G.Time.RegisterRecurringCallback(UpdateStatsByTime, _intervalLoss);
    }
    public void UpdateStatsByTime() {

        Hunger -= Game.G.Values.HUNGER_LOSS;

        Thirst -= Game.G.Values.THIRST_LOSS;

        if (Thirst <= 0 && !IsSleeping) {
            Life -= Game.G.Values.LIFE_LOSS;
        }

        Energy = IsNapping || IsSleeping ? Energy + Game.G.Values.ENERGY_LOSS : Energy - Game.G.Values.ENERGY_LOSS;

        ClampStats();
    }
    public void UpdateEnergy() {
        Energy -= Game.G.Values.ENERGY_LOSS;
    }
    public void ClampStats() {
        Hunger = Mathf.Clamp(Hunger, 0, 100);
        Thirst = Mathf.Clamp(Thirst, 0, 100);
        Energy = Mathf.Clamp(Energy, 0, 100);
        Life = Mathf.Clamp(Life, 0, 100);
    }
}