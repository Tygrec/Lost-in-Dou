using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _mainCam;

    public void EnableMainCam(bool value) {
        _mainCam.enabled = value;
    }
}
