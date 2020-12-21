using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    [SerializeField] float maxSpeed = 0.0f;

    [SerializeField] float minSpeedArrowAngle;
    [SerializeField] float maxSpeedArrowAngle;

    [SerializeField] private TextMeshProUGUI speedTxt = null;
    [SerializeField] RectTransform arrow;

    private float speed = 0.0f;
    private void OnEnable()
    {
        EventManager.onSpeedUpdate += UpdateSpeedometer;
    }

    private void OnDisable()
    {
        EventManager.onSpeedUpdate -= UpdateSpeedometer;
    }

    private void UpdateSpeedometer(float currentSpeed)
    {
        speed = currentSpeed * 3.6f;

        if (speedTxt != null)
            speedTxt.text = (int)speed + " Km/h";

        if (arrow != null)
            arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
    }
}
