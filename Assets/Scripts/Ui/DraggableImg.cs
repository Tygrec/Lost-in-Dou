using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImg : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    [HideInInspector] public Transform TransformAfterDrag;
    [HideInInspector] public ItemInInventory Item;

    public void SetImage(Sprite sprite) {
        GetComponent<Image>().sprite = sprite;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        TransformAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(TransformAfterDrag, false);
        GetComponent<Image>().raycastTarget = true;
    }

    public void RemoveItselfFromOldInventory() {
        TransformAfterDrag.parent.gameObject.GetComponent<Slot>().RemoveItemFromInventory();
    }
}
