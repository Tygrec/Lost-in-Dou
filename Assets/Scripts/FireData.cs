using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireData
{
    private float _fireState = 0;

    public FireData() {
        Game.G.Time.RegisterRecurringCallback(FireDie, 2);
    }

    public float GetFireState() {
        return _fireState;
    }
    public void AddFuel() {
        if (_fireState <= 0)
            StartFire();
        else
            _fireState = 100;
    }
    private void StartFire() {
        _fireState = 100;
        // TODO : mini jeu
    }
    private void FireDie() {
        _fireState -= 1;
        _fireState = Mathf.Clamp(_fireState, 0, 100);
    }
}
