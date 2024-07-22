using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : SceneController
{
    [SerializeField] List<Transform> _spawnsTransform;
    [SerializeField] List<string> _scenesName;

    private Dictionary<string, Transform> _playerSpawnMapping = new Dictionary<string, Transform>();

    private void Awake() {
        for (int i = 0; i < _spawnsTransform.Count; i++) {
            _playerSpawnMapping.Add(_scenesName[i], _spawnsTransform[i]);
        }
    }
}
