using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    private int coin;
    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro.text = "0";
    }

    public void IncreaseCoin()
    {
        coin += 1;
        textMeshPro.text = coin.ToString();
    }
    
}
