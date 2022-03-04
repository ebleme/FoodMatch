using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StarSO", menuName = "My Scriptables/Star SO")]
public class StarSO : ScriptableObject
{
    public int starCount = 0;

    public UnityEvent<int> OnStarCountChangeEvent;

    private void OnEnable()
    {
        if (OnStarCountChangeEvent == null)
        {
            OnStarCountChangeEvent = new UnityEvent<int>();
        }
    }

    public void IncreaseStarCount(int amount)
    {
        starCount += amount;
        OnStarCountChangeEvent.Invoke(starCount);
    }

    public void ResetStartCount()
    {
        starCount = 0;
        OnStarCountChangeEvent.Invoke(starCount);
    }

}
