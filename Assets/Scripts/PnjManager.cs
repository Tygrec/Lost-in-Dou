using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjManager : MonoBehaviour
{
    private PnjData _data;

    private void Start() {
        _data = (PnjData)Game.G.GameManager.GetHumanData(Name.Pnj);
        transform.position = _data.Follow ? Game.G.Player.transform.position - Game.G.Player.transform.forward * GetComponent<CapsuleCollider>().radius * 3 :
            _data.CurrentPosition;
    }

    private void Update() {
        if (_data.Hunger <= 0)
            StopSleeping();

        GetComponent<Collider>().isTrigger = _data.Follow;
        if (_data.Follow)
            FollowPlayer();

        _data.CurrentPosition = transform.position;
        transform.LookAt(Game.G.Player.transform);
    }

    private void FollowPlayer() {
        
        var player = Game.G.Player.transform;
    //    transform.position =  + Vector3.back * GetComponent<CapsuleCollider>().radius * 2;
        Vector3 followPosition = player.position - player.forward * GetComponent<CapsuleCollider>().radius * 3;
        followPosition.y = transform.position.y; // Maintenir la hauteur actuelle

        // Déplacer le GameObject vers la position souhaitée avec interpolation
        transform.position = Vector3.Lerp(transform.position, followPosition, Game.G.Values.PLAYER_SPEED * Time.deltaTime);

        // Optionnel : orienter le GameObject vers le joueur
        transform.LookAt(player);
    }

    public void StartSleeping() {
        _data.IsSleeping = true;
    }
    public void StopSleeping() {
        _data.IsSleeping = false;
    }

    public void SavePosition() {
        _data.CurrentPosition = transform.position;
    }
}
