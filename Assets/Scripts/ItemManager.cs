using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour {
    [SerializeField] ItemData _data;

    private void Start() {
        if (_data == null) {
            _data = Resources.Load<ItemData>($"Scriptables/Items/{name}");
        }

        if (_data == null) {
            Debug.LogError("Attention, le prefab " + gameObject.name + " n'a pas de data associé");
        }
    }

    public ItemData GetItem() {
        return _data;
    }

    public void DestroyItem() {
        Destroy(gameObject);
    }
}