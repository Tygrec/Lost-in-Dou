using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [SerializeField] protected Transform _playerSpawn;
    [SerializeField] private List<SceneTransitionPlayer> _playerSpawns;

    private Vector3 _playerPositionSave;

    public void ChangeScene(string oldScene, string newScene) {
        Game.G.Db.SaveSpawnersId();
        StartCoroutine(IChangeScene(oldScene, newScene, GetSpawn(oldScene, newScene).position));
    }

    public void LoadMiniGame(string oldScene, string newScene) {
        Game.G.Db.SaveSpawnersId();
        _playerPositionSave = Game.G.Player.transform.position;
        StartCoroutine(ILoadMiniGame(oldScene, newScene));
    }

    IEnumerator IChangeScene(string oldScene, string newScene, Vector3 playerPosition) {

        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);

        SceneTransition.Instance.TransitionAnimation("Quit");

        yield return new WaitForSeconds(1);

        UnityEngine.SceneManagement.SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(oldScene);
        Game.G.Player.transform.position = playerPosition;

        SceneTransition.Instance.TransitionAnimation("Start");

        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);

        yield return new WaitForSeconds(1);
    }
    IEnumerator ILoadMiniGame(string oldScene, string newScene) {
        yield return null;
    }
    public void Transition() {
        StartCoroutine(ITransition());
    }
    public IEnumerator ITransition() {
        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);
        SceneTransition.Instance.TransitionAnimation("Quit");

        yield return new WaitForSeconds(1);

        SceneTransition.Instance.TransitionAnimation("Start");
        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);

        yield return new WaitForSeconds(1);
    }

    private Transform GetSpawn(string fromScene, string toScene) {
        return _playerSpawns.Find(spawn => spawn.fromScene == fromScene && spawn.toScene == toScene).position;
    }
}

[Serializable]
public class SceneTransitionPlayer {
    public string fromScene;
    public string toScene;
    public Transform position;
}