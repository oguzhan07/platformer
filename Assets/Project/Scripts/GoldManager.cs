using System;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GoldManager : MonoBehaviour
{
    private int coin;
    [SerializeField] private Gold gold;
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private Camera camera;
    public Image goldUiImage;
    public TextMeshProUGUI textMeshPro;
    [SerializeField] private Transform canvas;


    private void Start()
    {
        textMeshPro.text = "0";
    }
    
    public void ManagerGold(Vector2 goldPosition)
    {
        IncreaseCoin();
        MoveCoin(goldPosition);
    }
    

    private void IncreaseCoin()
    {
        coin += 1;
        textMeshPro.text = coin.ToString();
    }
    

    private void MoveCoin(Vector2 goldPosition)
    {
        Vector2 uiPos = goldUiImage.transform.position;
        GameObject movingGold = Instantiate(goldPrefab, camera.WorldToScreenPoint(goldPosition), Quaternion.identity, canvas);
        movingGold.transform.DOMove(uiPos, 1f).SetEase(Ease.InOutBack);//.SetLink(movingGold, LinkBehaviour.KillOnDestroy);
    }
    
}
