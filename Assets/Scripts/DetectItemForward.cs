using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectItemForward : MonoBehaviour {
    [SerializeField] LayerMask _layerMask;
    [SerializeField] LayerMask _vaultLayer;

    [SerializeField] float _maxVaultingDistance;
    [SerializeField] float _detectionDistance;

    [SerializeField] CapsuleCollider _collider;

    private void Start() {
    }
    void Update() {
        if (Game.G.GameManager.GetGameState() != GAMESTATE.RUNNING)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionDistance, _layerMask);

        foreach (var hit in hitColliders) {

            if (hit.transform.CompareTag("Item")) {
                ItemBehavior(hit);
                return;
            }
            else if (hit.transform.CompareTag("Bed")) {
                BedBehavior(hit);
                return;
            }
            else if (hit.transform.CompareTag("Water")) {
                WaterBehavior(hit);
                return;
            }
            else if (hit.transform.CompareTag("Stock")) {
                StockBehavior(hit);
                return;
            }
            else if (hit.transform.CompareTag("Fire")) {
                FireBehavior();
                return;
            }
            else if (hit.transform.CompareTag("Kitchen")) {
                UiManager.Instance.DisplayPressEInfo("Cuisiner");

                if (Input.GetKeyDown(KeyCode.E)) {
                    Game.G.Cook.StartCooking();
                }
                return;
            }
            else if (hit.transform.CompareTag("PNJ")) {
                PnjBehavior();
            }
            else if (hit.gameObject.layer == 6) { // TODO : Mettre "vault" à la place de 6
                VaultBehavior(hit);
                return;
            }
        }

        if (hitColliders.Length == 0)
            UiManager.Instance.HidePressEInfo();
    }

    IEnumerator LerpVault(Vector3 targetPosition, float duration, Collider hit) {

        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration) {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        _collider.isTrigger = false;
    }

    private void VaultBehavior(Collider vault) {
        UiManager.Instance.DisplayPressEInfo("Grimper");

        if (Input.GetKeyDown(KeyCode.E)) {

            Game.G.Player.Needs.LoseEnergy();
            float obstacleHeight = vault.GetComponent<Renderer>().bounds.size.y;
            // La position à laquelle le joueur essaye de grimper :
            // sa position actuelle + Son rayon * 2 devant lui + la distance max à laquelle il peut grimper en hauteur
            Vector3 position = transform.position +
                (transform.forward * _collider.radius * 2) +
                (Vector3.up * _maxVaultingDistance);

            var value = Physics.Raycast(position, Vector3.down, out var secondHit, _vaultLayer);

            // On tire un raycast de la position trouvé vers le bas, de la taille du joueur pour voir s'il peut atterrir quelque part
            if (value && Vector3.Distance(transform.position, secondHit.point) < _maxVaultingDistance) {
                _collider.isTrigger = true;
                StartCoroutine(LerpVault(secondHit.point, 0.5f, vault));
            }
        }
    }
    private void BedBehavior(Collider bed) {

        UiManager.Instance.DisplayPressEInfo("Dormir");

        if (Input.GetKeyDown(KeyCode.E)) {
            if(Game.G.Player.Needs.Hunger() <= 0) {
                Game.G.Player.Controller.ThinkSomething("J'ai trop faim pour dormir...");
            }
            else if (Game.G.Db.Fire.GetFireState() <= 0) {
                Game.G.Player.Controller.ThinkSomething("Il fait trop froid pour dormir...");
            }
            else {
                Game.G.Player.Controller.Sleep();
            }
        }
    }
    private void ItemBehavior(Collider item) {
        var currentItem = item.transform.gameObject.GetComponent<ItemManager>();
        UiManager.Instance.DisplayPressEInfo("Ramasser");

        if (Input.GetKeyDown(KeyCode.E))
            Game.G.GameManager.OnPickUpItem.Invoke(currentItem.GetItem(), currentItem);
    }
    private void WaterBehavior(Collider water) {

        if(Game.G.Player.Controller.Equipped()?.name == "Lance") {
            UiManager.Instance.DisplayPressEInfo("Pêcher");

            if (Input.GetKeyDown(KeyCode.E))
                Game.G.Player.Controller.Fishing(water.GetComponent<Fishing>().GetFish());
        }
        else {
            UiManager.Instance.DisplayPressEInfo("Boire");

            if (Input.GetKeyDown(KeyCode.E))
                Game.G.Player.Needs.Drink();
        }
        
    }
    private void StockBehavior(Collider stock) {
        UiManager.Instance.DisplayPressEInfo("Ouvrir le stockage");

        if (Input.GetKeyDown(KeyCode.E)) {
            UiManager.Instance.DisplayInventory(Game.G.Inv.Get(InvTag.Player));
            UiManager.Instance.DisplayInventory(Game.G.Inv.Get(InvTag.Stock));

        }
    }

    private void FireBehavior() {
        UiManager.Instance.DisplayPressEInfo("Ajouter du bois");

        if (Input.GetKeyDown(KeyCode.E)) {
            if (Game.G.Inv.Get(InvTag.Player).ItemExistsInInventory(ItemData.Wood())) {
                Game.G.Db.Fire.AddFuel();
                Game.G.Inv.Get(InvTag.Player).RemoveItem(ItemData.Wood());
            }
            else
                Game.G.Player.Controller.ThinkSomething("Je n'ai pas de bois sur moi.");
        }
    }
    
    private void PnjBehavior() {
        UiManager.Instance.DisplayPressEInfo("Parler");
        if (Input.GetKeyDown(KeyCode.E)) {
            Game.G.Dialog.StartDialog(DialogId.TEST);
        }
        else if (Input.GetKeyDown(KeyCode.T)) {
            StartCoroutine(StartFollowing());
        }
    }

    IEnumerator StartFollowing() {
        yield return new WaitForSeconds(0.1f);
        Game.G.GameManager.SwitchPnjFollow();
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
