using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField]
    private StarSO starSO;

    [SerializeField]
    private int baseScore = 3;

    [SerializeField]
    private ParticleSystem starParticleEffect;

    MatchArea matchArea;
    Combo combo;

    private int comboAmount;

    private void Awake()
    {
        matchArea = FindObjectOfType<MatchArea>();
        matchArea.OnPointGathered += MatchArea_OnPointGathered;

        combo = FindObjectOfType<Combo>();
        combo.OnComboAmountChanged += Combo_OnComboAmountChanged;
    }

    private void Start()
    {
        starSO.ResetStartCount();
    }

    private void Combo_OnComboAmountChanged(int amount)
    {
        comboAmount = amount;
    }

    private void MatchArea_OnPointGathered(Vector3 pos)
    {
        int stars = baseScore + comboAmount;

        starSO.IncreaseStarCount(stars);

        starParticleEffect.transform.position = pos;
        starParticleEffect.Play();
    }

    public int GetStarCount()
    {
        return starSO.starCount;
    }
}
