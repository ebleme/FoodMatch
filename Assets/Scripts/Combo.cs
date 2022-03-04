using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    [SerializeField]
    private float comboTime = 5f;

    [SerializeField]
    private Slider comboSlider;

    [SerializeField]
    private Text comboAmountText;


    MatchArea matchArea;

    private float comboLeftTime;
    private int comboAmount = 0;

    public event Action<int> OnComboAmountChanged;

    private void Awake()
    {
        matchArea = FindObjectOfType<MatchArea>();
        matchArea.OnPointGathered += MatchArea_OnPointGathered;
    }

    private void Start()
    {
        comboSlider.maxValue = comboTime;
    }

    private void MatchArea_OnPointGathered(Vector3 pos)
    {
        comboAmount += 1;
        
        OnComboAmountChanged.Invoke(comboAmount);

        comboAmountText.text = "x"+comboAmount.ToString();

        comboLeftTime = comboTime;
    }

    private void Update()
    {
        if (comboLeftTime > 0)
        {
            comboLeftTime -= Time.deltaTime;
            comboSlider.value = comboLeftTime;
        }
        else
        {
            comboAmount = 0;
            OnComboAmountChanged?.Invoke(comboAmount);
        }
    }
}
