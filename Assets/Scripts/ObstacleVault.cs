using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleVault : MonoBehaviour
{
    [SerializeField] BoxCollider barrier;
    public void DisableBarriers() {
        barrier.isTrigger = true;
    }
    public void EnableBarriers() {
        barrier.isTrigger = false;
    }
}
