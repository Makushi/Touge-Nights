using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image energyFill = null;

    private void OnEnable()
    {
        EventManager.onEnergyUpdate += UpdateEnergyBar;
    }

    private void OnDisable()
    {
        EventManager.onEnergyUpdate -= UpdateEnergyBar;
    }

    public void UpdateEnergyBar(float currentEnergy)
    {
        energyFill.fillAmount = currentEnergy;
    }
}
