using System;
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
    [SerializeField] private CollectibleManager collectibleManager;
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
            collectibleManager.GoldManager(transform.position);
            Destroy(gameObject);
            transform.DOKill(gameObject);
        }
    }

    
    private void GoldAnimation()
    {
        transform.DOMoveY(transform.position.y + 0.25f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
