using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private int coin;
    public TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject healthBar;
    private float endValue;
    private float maxHealth;

    private void Start()
    {
        textMeshPro.text = "0";
    }

    public void IncreaseCoin()
    {
        coin += 1;
        textMeshPro.text = coin.ToString();
    }

    public void ReduceHealth(int health)
    {
        Color startColor = new Color(0, 255, 0, 255);
        Color endColor = new Color(255, 0, 0, 255);
        if (health > 0)
        {
            endValue = (health * 100 / maxHealth) / 1000;
        }
        else
        {
            endValue = 0;
        }
        
        healthBar.transform.DOScaleX(endValue, 0.4f);
        //healthBar.DOColor(endColor,0.4f);
    }
}
