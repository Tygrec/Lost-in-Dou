using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DayTime {
    Morning,
    Afternoon,
    Evening,
    Night
}
public class TimeManager : MonoBehaviour {

    public static TimeManager Instance;

    public float GlobalTimer;

    private float _timer = 0;
    private int _day = 1;
    private float _minute;
    private float _hour = 8;
    private float _timeSpeed;

    [SerializeField] DayTime _dayTime;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start() {
        GlobalTimer = _hour * 60;
        _timeSpeed = ConstGlobalValues.Instance.TIME_SPEED;
    }

    public int GetDay() {
        return _day;
    }
    public float GetMin() {
        return _minute;
    }
    public string GetMinText() {
        string text = "";

        if (_minute < 10)
            text += "0";
        
        text += _minute.ToString();

        return text;
    }
    public float GetHour() {
        return _hour;
    }
    public string GetHourText() {
        string text = "";

        if (_hour < 10)
            text += "0";

        text += _hour.ToString();

        return text;
    }
    // Pour accélérer l'écoulement du temps
    public void SetTimeSpeed(float timeSpeed) {
        _timeSpeed = timeSpeed;
    }
    // Prend en paramètre l'heure à laquelle commence la journée
    public void SetNewDay(int hour) {
        _day++;
        _hour = hour;
        _minute = 0;
        GlobalTimer = _hour * 60;
        _timer = 0;

        GameManager.Instance.OnNewDay?.Invoke();
    }

    void Update()
    {
        if (GameManager.Instance.GetGameState() != GAMESTATE.RUNNING)
            return;

        GlobalTimer += Time.deltaTime / _timeSpeed;
        _timer += Time.deltaTime / _timeSpeed;

        if (_timer >= 1) {
            _minute++;

            // Le joueur perd des stats toutes les 5 secondes
            if (_minute % 5 == 0) {
                PlayerController.Instance.UpdateStatsByTime();
            }

            _timer = 0;
        }

        if (_minute >= 60) {
            ChangeHour();
        }

        if (_hour >= 24) {
            SetNewDay(0);
        }
    }

    private void ChangeHour() {
        _hour++;
        _minute = 0;

        if (_hour > 6 && _hour <= 10)
            _dayTime = DayTime.Morning;
        else if (_hour > 10 && _hour <= 18)
            _dayTime = DayTime.Afternoon;
        else if (_hour > 18 && _hour <= 24)
            _dayTime = DayTime.Evening;
        else if (_hour >= 0 && _hour <= 6)
            _dayTime = DayTime.Night;
    }

}
