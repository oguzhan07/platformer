using DG.Tweening;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public UiManager uiManager;
    
    private void Start()
    {
        GoldAnimation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Burada arttırma işlemini başka bir scriptte yapmak problemimi çözdü fakat başka bir problem doğurdu:
            // Her bir altın'a UiManager'ı elle tek tek tanıtmak. Bunu nasıl koddan yapabilirim?
            uiManager.IncreaseCoin();
            Destroy(gameObject);
            transform.DOKill();
        }
    }

    private void GoldAnimation()
    {
        transform.DOMoveY(transform.position.y + 0.25f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
