using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjController : MonoBehaviour
{
    private PnjData _data;

    private void Start() {
        _data = (PnjData)Game.G.GameManager.GetHumanData(Name.Pnj);
        transform.position = _data.CurrentPosition;
    }

    private void Update() {
        if (_data.Hunger <= 0)
            StopSleeping();

        if (_data.Follow)
            FollowPlayer();

        transform.LookAt(Game.G.Player.transform);
    }

    private void FollowPlayer() {
        var player = Game.G.Player.transform;
    
        Vector3 followPosition = player.position - player.forward * GetComponent<CapsuleCollider>().radius * 3;
        followPosition.y = player.position.y;

        transform.position = Vector3.Lerp(transform.position, followPosition, Game.G.Values.PLAYER_SPEED * Time.deltaTime);
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
