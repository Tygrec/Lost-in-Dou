using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

    public ItemData EquippedItem = null;

    public float Hunger = 50;
    public float Thirst = 50;
    public float Energy = 50;
    public float Life = 100;

    public void CheckMaxValues() {
        if (Hunger > 100)
            Hunger = 100;
        if (Thirst > 100)
            Thirst = 100;
        if (Energy > 100)
            Energy = 100;
        if (Life > 100)
            Life = 100;
    }
}
