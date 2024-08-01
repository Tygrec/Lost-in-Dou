using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreparationDisplay : MonoBehaviour, IPointerDownHandler
{
    private Preparation _related;

    public void SetPreparation(Preparation preparation) {
        _related = preparation;
    }
    public Preparation GetPreparation() {
        return _related;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Game.G.Cook.ChangeCurrentPreparation(_related);
    }

    public void SetSelected(bool value) {
        GetComponent<Outline>().enabled = value;
        GetComponent<Outline>().effectColor = _related.Color;
    }
}