using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour {
    [SerializeField] List<Fishable> _fishables;
    public bool Drinkable;

    public FishData GetFish() {
        int total = 0;
        _fishables.ForEach(x => total += x.DropRate);
        int random = UnityEngine.Random.Range(0, total);

        for(int i = 0 ; i < _fishables.Count; i++) {
            if (random < _fishables[i].DropRate)
                return _fishables[i].Fish;
            else
                random -= _fishables[i].DropRate;
        }

        Debug.LogError("Erreur : aucun poisson trouvé");
        return _fishables[0].Fish;
    }

}

[Serializable]
public class Fishable {
    public FishData Fish;
    public int DropRate;
}