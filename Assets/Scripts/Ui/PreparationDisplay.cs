using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreparationDisplay : MonoBehaviour, IPointerDownHandler
{
    public Preparation Related;

    public void OnPointerDown(PointerEventData eventData) {
        print(Related);
        Game.G.Cook.ChangeCurrentPreparation(Related);
    }

    public void SetSelected(bool value) {
        GetComponent<Outline>().enabled = value;
    }
}