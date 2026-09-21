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
    
    public void ManagerGold(GameObject paramGameObject)
    {
        IncreaseCoin();
        MoveCoin();
        DestroyCoin(paramGameObject);
    }
    

    private void IncreaseCoin()
    {
        coin += 1;
        textMeshPro.text = coin.ToString();
    }
    

    private void MoveCoin()
    {
        Vector2 uiPos = camera.ScreenToWorldPoint(goldUiImage.transform.position);
        Vector2 goldPos = gold.GoldPos();

        GameObject movingGold = Instantiate(goldPrefab, goldPos, Quaternion.identity, canvas);
        movingGold.transform.DOMove(uiPos, 2f);//.SetLink(movingGold, LinkBehaviour.KillOnDestroy);
    }

    private void DestroyCoin(GameObject paramGameObject)
    {
        Destroy(paramGameObject);
    }
    
}
