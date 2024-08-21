using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerOnCollisionEvent : MonoBehaviour
{
    [SerializeField] Dialog _dialog;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] string _sceneName;
    [SerializeField] bool _needApollo;

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")|| !SceneManager.GetSceneByName(_sceneName).isLoaded)
            return;
        if (_needApollo && !Game.G.Pnj.IsFollowing())
            return;

        Game.G.Dialog.StartDialog(_dialog);

        if (_camera != null)
            StartCoroutine(IWaitForLastReplica());
        else
            Destroy(gameObject);
    }

    IEnumerator IWaitForLastReplica() {
        print("Attente de la fin du dialogue");
        yield return new WaitUntil(() => Game.G.Dialog.IsAtLastReplica());


        print("fin du dialogue");
        StartCoroutine(ISwitchCameras());
    }
    IEnumerator ISwitchCameras() {
        CinemachineBrain brain = FindAnyObjectByType<CinemachineBrain>();

        _camera.Priority = 1000;

        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);

        yield return new WaitUntil(() => Game.G.Dialog.IsFinished());

        _camera.Priority = 0;

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => !brain.IsBlending);
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
        Destroy(gameObject);
    }
}