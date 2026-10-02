using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ScenesManager scenesManager;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scenesManager.ChangeScene("Outro");
        }
    }
}
