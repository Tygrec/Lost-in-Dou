using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float GlobalTimer;

    private float _elapsedSecs = 8 * 60 * 60;
    private int _day = 1;
    private float _timeSpeed;

    List<RecurringCallback> _recurringCallbacks = new List<RecurringCallback>();

    //[SerializeField] DayTime _dayTime;
    private void Start() {
        GlobalTimer = GetDisplayedHours() * 60;
        _timeSpeed = Game.G.Values.TIME_SPEED;
    }

    public int GetDay() {
        return _day;
    }
    public int GetElapsedMinutesSinceDayStart() {
        return GetElapsedMinutesSinceDayStart(_elapsedSecs);
    }
    public int GetElapsedMinutesSinceDayStart(float elapsedSecs) {
        float secondsPerDay = 24 * 60 * 60;
        return (int)((elapsedSecs % secondsPerDay) / 60);
    }
    public int GetDayMinutes() {
        return GetElapsedMinutesSinceDayStart() % 60;
    }
    public int GetDisplayedHours() {
        return GetElapsedMinutesSinceDayStart() / 60;
    }
    public string GetDayMinText() {
        return GetDayMinutes().ToString().PadLeft(2, '0');
    }
    public string GetDayHourText() {
        return GetDisplayedHours().ToString().PadLeft(2, '0');
    }
    // Pour accélérer l'écoulement du temps
    public void SetTimeSpeed(float timeSpeed) {
        _timeSpeed = timeSpeed;
    }
    // Prend en paramètre l'heure à laquelle commence la journée
    public void SetNewDay(float hour) {
        AddElapsedSecs((24 + hour - GetDisplayedHours()) * 60 * 60);

        _day++;
        _elapsedSecs = hour * 60 * 60;
        GlobalTimer = GetDisplayedHours() * 60;
        Game.G.GameManager.OnNewDay?.Invoke();
    }

    void Update() {
        if (Game.G.GameManager.GetGameState() != GAMESTATE.RUNNING)
            return;

        GlobalTimer += Time.deltaTime * _timeSpeed;

        AddElapsedSecs(Time.deltaTime * _timeSpeed * 60);
    }

    private void AddElapsedSecs(float secs) {

        foreach(var callback in _recurringCallbacks) {
            TriggerActionIfNeeded(callback, secs);
        }
        
        _elapsedSecs += secs;
    }

    private void TriggerActionIfNeeded(RecurringCallback recurringCallback, float elapsedSinceLastUpdate) {
        float accumulatedTime = _elapsedSecs % recurringCallback.Interval + elapsedSinceLastUpdate;

        while (accumulatedTime > recurringCallback.Interval) {
            accumulatedTime -= recurringCallback.Interval;
            recurringCallback.Callback.Invoke();
        }
    }

    public void RegisterRecurringCallback(Action callback, float interval) {
        _recurringCallbacks.Add(new RecurringCallback(callback, interval * 60));
    }
    public void RemoveRecurringCallback(Action callback) {
        _recurringCallbacks.RemoveAll(cb => cb.Callback == callback);
    }
}

public class RecurringCallback {
    public Action Callback;
    public float Interval;

    public RecurringCallback(Action callback, float interval) {
        Callback = callback;
        Interval = interval;
    }
}