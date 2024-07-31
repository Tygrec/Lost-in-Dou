using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    [SerializeField] GameObject _flames;
    private void Update() {
        float value = Game.G.Db.Fire.GetFireState() / 100;

        _flames.transform.localScale = new Vector3(value, value, value);
    }
    
    public void AddFuel() {
        if (Game.G.Db.Fire.GetFireState() == 0)
            LightFire();
        else
            Game.G.Db.Fire.AddFuel();
    }
    public void LightFire() {

    }
}
