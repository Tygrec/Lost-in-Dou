using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour {
    [SerializeField] List<FishData> _fishs;
    [SerializeField] List<int> _dropRate;

    public FishData GetFish() {
        int total = 0;
        _dropRate.ForEach(x => total += x);
        int random = Random.Range(0, total);

        for(int i = 0 ; i < _fishs.Count; i++) {
            if (random < _dropRate[i])
                return _fishs[i];
            else
                random -= _dropRate[i];
        }

        Debug.LogError("Erreur : aucun poisson trouvé");
        return _fishs[0];
    }

}
