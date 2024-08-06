using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterThoughts : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _thoughts;

    public void Display(string thinking) {
        gameObject.SetActive(true);
        _thoughts.text = thinking;
    }
    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Update() {
        transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
    }
}
