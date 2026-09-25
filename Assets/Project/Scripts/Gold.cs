using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    [FormerlySerializedAs("uiManager")] public GoldManager goldManager;
    [SerializeField] private Image uiGold;
    private Vector3 uiGoldPos;
    
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
            goldManager.ManagerGold(transform.position);
            Destroy(gameObject);
            transform.DOKill(gameObject);
        }
    }

    /*public Vector2 GoldPos()
    {
        if (gameObject != null)
        {
            Vector2 goldPosition = transform.position; 
            return goldPosition;
        }
    }*/

    private void GoldAnimation()
    {
        transform.DOMoveY(transform.position.y + 0.25f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
