using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDraggable : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        print("On Drop");
        Draggable obj = eventData.selectedObject.GetComponent<Draggable>();
        obj.TransformAfterDrag = transform;

    }
}
