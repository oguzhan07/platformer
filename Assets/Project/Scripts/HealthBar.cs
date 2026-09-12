using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image image;
    private Image imageWhite;
    public GameObject whiteBar;
    
    private void Start()
    {
        image = GetComponent<Image>();
        imageWhite = whiteBar.GetComponent<Image>();
    }
    
    public void Bar(float percentValue)
    {
        Color endColor = new Color(255 - (255 * percentValue), 255 * percentValue, 0, 255);
        Color whiteColor = new Color(255,255,255,255);

        // endValue: 0 ile 1 arasında olmalı.
        transform.DOScaleX(percentValue, 1f);
        image.DOColor(endColor, 1f);
        
        //ui'da layer: hiyerarşide altta olan sahnede üstte oluyormuş :/
        whiteBar.transform.DOScaleX(percentValue, 1.5f);
        imageWhite.DOColor(whiteColor, 1.5f);
        
    }

    
}
