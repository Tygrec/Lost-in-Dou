using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectItemForward : MonoBehaviour {
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _detectionDistance;
    [SerializeField] float _detectionAngle;
    void Update() {
        if (GameManager.Instance.GetGameState() != GAMESTATE.RUNNING)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionDistance, _layerMask);

        foreach (var hit in hitColliders) {

            if (hit.transform.CompareTag("Item")) {

                var currentItem = hit.transform.gameObject.GetComponent<ItemManager>();
                UiManager.Instance.DisplayPressEInfo("Ramasser");

                if (Input.GetKeyDown(KeyCode.E))
                    GameManager.Instance.OnPickUpItem(currentItem.GetItem(), currentItem);
                return;
            }
            else if (hit.transform.CompareTag("Bed")) {

                UiManager.Instance.DisplayPressEInfo("Dormir");

                if (Input.GetKeyDown(KeyCode.E))
                    StartCoroutine(PlayerController.Instance.ISleep());
                return;
            }
            else if (hit.transform.CompareTag("Water")) {

                UiManager.Instance.DisplayPressEInfo("Boire");

                if (Input.GetKeyDown(KeyCode.E))
                    PlayerController.Instance.Drink();
                return;
            }
            else if (hit.transform.CompareTag("Stock")) {
                UiManager.Instance.DisplayPressEInfo("Ouvrir le stockage");

                if (Input.GetKeyDown(KeyCode.E)) {
                    UiManager.Instance.DisplayInventory(StockInventory.Instance);
                    UiManager.Instance.DisplayInventory(PlayerInventory.Instance);
                    return;
                }
                    
            }
        }

        if (hitColliders.Length == 0)
            UiManager.Instance.HidePressEInfo();
    }

 /*   private void OnDrawGizmos() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionDistance);
        Vector3 forward = transform.forward * _detectionDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, _detectionAngle / 2, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -_detectionAngle / 2, 0) * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, forward);
        Gizmos.DrawRay(transform.position, rightBoundary);
        Gizmos.DrawRay(transform.position, leftBoundary);
    } */
}
