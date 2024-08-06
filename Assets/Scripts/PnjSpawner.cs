using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PnjSpawner : MonoBehaviour
{
    
    void Start()
    {
        PnjData pnj = (PnjData)Game.G.GameManager.GetHumanData(Name.Pnj);
        if (!pnj.Follow && pnj.CurrentScene == SceneManager.GetActiveScene().name) {
            Instantiate(Resources.Load("Prefabs/Pnj"), transform);
        }
    }

}
