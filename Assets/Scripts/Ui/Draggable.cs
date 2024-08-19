using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    [HideInInspector] public Transform TransformAfterDrag;

    public void OnBeginDrag(PointerEventData eventData) {
        TransformAfterDrag = transform.parent;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.parent = TransformAfterDrag;
    }
}
