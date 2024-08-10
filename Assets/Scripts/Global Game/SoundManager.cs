using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] AudioSource _audioAmbient;
    [SerializeField] AudioSource _audioSounds;

    [SerializeField] List<SceneAmbientSound> _ambientSounds;

    [SerializeField] AudioClip _caveAmbientFire;
    private void Start() {
        _audioAmbient.Play();
    }

    public void ChangeSceneSound(string newScene) {
        _audioAmbient.clip = _ambientSounds.Find(sound => sound.SceneName == newScene).Audio;
        _audioAmbient.Play();
    }
    public void ActivateFireSound(bool value) {
        _audioSounds.clip = _caveAmbientFire;

        if (value) {
            _audioSounds.Play();
        }
        else
            _audioSounds.Stop();
    }
}
[Serializable]
public class SceneAmbientSound {
    public AudioClip Audio;
    public string SceneName;
}