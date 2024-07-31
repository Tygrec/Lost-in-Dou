using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 _offset;

    private void LateUpdate() {
        
        transform.position = Game.G.Player.transform.position + _offset;
    }
}
