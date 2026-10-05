using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Sword : MonoBehaviour, ICollectible
{
    [SerializeField] private CollectibleManager collectibleManager;
    
    private void Start()
    {
        Animation();
    }

    public void Collect()
    {
        collectibleManager.ChangeCharacter(transform.position);
        Destroy(gameObject);
        transform.DOKill(gameObject);
    }

    private void Animation()
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
