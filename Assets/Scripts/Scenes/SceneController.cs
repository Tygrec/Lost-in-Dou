using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] protected Transform _playerSpawn;

    void Start()
    {
        if (Instance == null) {
            Instance = this;
        }
        else
            Destroy(gameObject);

        Instantiate(Resources.Load<PlayerController>("Prefabs/Player"), _playerSpawn);
    }

    public void ChangeScene(string sceneName) {
        StartCoroutine(IChangeScene(sceneName));
    }

    IEnumerator IChangeScene(string sceneName) {
        GameManager.Instance.ChangeGameState(GAMESTATE.PAUSE);

        SceneTransition.Instance.TransitionAnimation("Quit");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(gameObject.scene.name);

        SceneTransition.Instance.TransitionAnimation("Start");

        GameManager.Instance.ChangeGameState(GAMESTATE.RUNNING);

        yield return new WaitForSeconds(1);
    }

    public IEnumerator ITransition() {
        GameManager.Instance.ChangeGameState(GAMESTATE.PAUSE);
        SceneTransition.Instance.TransitionAnimation("Quit");

        yield return new WaitForSeconds(1);

        SceneTransition.Instance.TransitionAnimation("Start");
        GameManager.Instance.ChangeGameState(GAMESTATE.RUNNING);

        yield return new WaitForSeconds(1);
    }
}
