using UnityEngine;
using UnityEngine.UI;

public class StarView : MonoBehaviour
{
    [SerializeField]
    private StarSO starSO;

    [SerializeField]
    private Text starCountText;

    private void Awake()
    {
        starSO.OnStarCountChangeEvent.AddListener(ShowStars);
    }

    private void ShowStars(int amount)
    {
        starCountText.text = amount.ToString();
    }   
}
