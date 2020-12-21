using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapTimeTxt = null;

    private void OnEnable()
    {
        EventManager.onShowResults += ShowLapTime;
    }

    private void OnDisable()
    {
        EventManager.onShowResults -= ShowLapTime;
    }

    private void ShowLapTime()
    {
        lapTimeTxt.text = "Your Time: " + GameController.Instance.GetTime();
    }
}
