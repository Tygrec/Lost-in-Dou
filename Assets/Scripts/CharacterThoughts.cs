using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ThoughtsSituation {
    TooColdToSleep,
    TooHungryToSleep
}
public class CharacterThoughts : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _thoughts;

    Dictionary<ThoughtsSituation, string> _thoughtsMapping = new Dictionary<ThoughtsSituation, string>();
    private void Awake() {

        _thoughtsMapping.Add(ThoughtsSituation.TooColdToSleep, "J'ai trop froid pour dormir...");
        _thoughtsMapping.Add(ThoughtsSituation.TooHungryToSleep, "J'ai trop faim pour dormir...");
    }
    public void Display(ThoughtsSituation situation) {
        gameObject.SetActive(true);
        _thoughts.text = _thoughtsMapping[situation];
    }
    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Update() {
        transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
    }
}
