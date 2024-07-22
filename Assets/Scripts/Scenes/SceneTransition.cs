using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] Animator _animator;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TransitionAnimation(string trigger) {
        _animator.SetTrigger(trigger);
    }
}
