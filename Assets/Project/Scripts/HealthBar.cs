using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image image;
    public GameObject whiteBar;
    
    
    private void Start()
    {
        image = GetComponent<Image>();
    }
    
    public void Bar(float percentValue)
    {
        Color endColor = new Color(255 - (255 * percentValue), 255 * percentValue, 0, 255);

        // endValue: 0 ile 1 arasında olmalı.
        transform.DOScaleX(percentValue, 1f);
        image.DOColor(endColor, 1f);

        StartCoroutine(WhiteBarDelay(percentValue));
    }

    IEnumerator WhiteBarDelay(float percentValue)
    {
        yield return new WaitForSeconds(1f);
        //ui'da layer: hiyerarşide altta olan sahnede üstte oluyormuş :/
        whiteBar.transform.DOScaleX(percentValue, 1.5f);
    }

    
}
