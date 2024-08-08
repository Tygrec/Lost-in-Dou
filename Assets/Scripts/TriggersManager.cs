using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TriggersManager : MonoBehaviour
{
    [SerializeField] List<Trigger> _eventsTrigger;

    private void Start() {

    }
}

[Serializable]
public class Trigger {
    [HideInInspector] public TriggerEvent TriggerEvent;
    [HideInInspector] public Transform Transform;
    [HideInInspector] public bool WasTriggered = false;
    public string Scene;
}