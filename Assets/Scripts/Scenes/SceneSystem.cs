using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour {
    [SerializeField] Transform _pnjSpawn;
    [SerializeField] private List<SceneTransitionPlayer> _playerSpawns;

    private void Start() {
        ManagePnj(Game.G.Values.MAIN_SCENE_NAME);
    }

    public void ChangeScene(string oldScene, string newScene) {
        Game.G.Db.SaveSpawnersId();
        StartCoroutine(IChangeScene(oldScene, newScene, GetSpawn(oldScene, newScene).position));
    }

    public void LoadMiniGame(string oldScene, string newScene) {
        Game.G.Db.SaveSpawnersId();
        StartCoroutine(ILoadMiniGame(oldScene, newScene));
    }

    IEnumerator IChangeScene(string oldScene, string newScene, Vector3 playerPosition) {

        Game.G.GameManager.ChangeGameState(GAMESTATE.PAUSE);

        SceneTransition.Instance.TransitionAnimation("Quit");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(oldScene);
        Game.G.Player.transform.position = playerPosition;

        SceneTransition.Instance.TransitionAnimation("Start");

        Game.G.GameManager.ChangeGameState(GAMESTATE.RUNNING);
        ManagePnj(newScene);
        Game.G.Sound.ChangeSceneSound(newScene);

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

    private void ManagePnj(string newScene) {
        PnjData pnj = (PnjData)Game.G.GameManager.GetHumanData(Name.Pnj);

        if (!pnj.Follow) {
            if (pnj.CurrentScene == newScene) {
                Game.G.GameManager.Pnj = Instantiate(Resources.Load<PnjManager>("Prefabs/Pnj"), _pnjSpawn);
            }
            else if (Game.G.GameManager.Pnj != null) {
                Destroy(Game.G.GameManager.Pnj.gameObject);
            }
        }
        else {
            Game.G.GameManager.ChangePnjScene(newScene);
        }
            
    }
}

[Serializable]
public class SceneTransitionPlayer {
    public string fromScene;
    public string toScene;
    public Transform position;
}