using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _dayText;
    [SerializeField] TextMeshProUGUI _timeText;

    // Update is called once per frame
    void Update()
    {
        _dayText.text = $"Jour {TimeManager.Instance.GetDay()}";
        _timeText.text = $"{TimeManager.Instance.GetHourText()}:{TimeManager.Instance.GetMinText()}";
    }
}
