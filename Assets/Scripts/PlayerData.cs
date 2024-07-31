using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

    public ItemData EquippedItem = null;

    public float Hunger = 50;
    public float Thirst = 50;
    public float Energy = 50;
    public float Life = 100;

    public void ClampStats() {
        Hunger = Mathf.Clamp(Hunger, 0, 100);
        Thirst = Mathf.Clamp(Thirst, 0, 100);
        Energy = Mathf.Clamp(Energy, 0, 100);
        Life = Mathf.Clamp(Life, 0, 100);
    }
}
