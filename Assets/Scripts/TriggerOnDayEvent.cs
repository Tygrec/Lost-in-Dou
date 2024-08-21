using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnDayEvent : MonoBehaviour
{
    [SerializeField] int _day;
    [SerializeField] GameObject _toActivate;
    [SerializeField] GameObject _toDestroy;

    private void OnEnable() {
        Game.G.GameManager.OnNewDay += HandleNewDay;
    }
    private void OnDisable() {
        Game.G.GameManager.OnNewDay -= HandleNewDay;
    }
    private void HandleNewDay() {
        if (_day != Game.G.Time.GetDay())
            return;

        print("Le grand jour est arrivé !");

        if (_toActivate != null) {
            _toActivate.SetActive(true);
        }
        if (_toDestroy != null) {
            Destroy( _toDestroy );
        }
    }
}
