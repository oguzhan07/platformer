using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDamageText : MonoBehaviour
{
    private Vector2 circleCenter;
    public float circleRadius = 2;
    private TextMeshPro textMeshPro;
    private Vector2 randomPos;
    [SerializeField] private GameObject enemyDamageNumber;
    public Enemy enemy;



    public void EnemyGetDamage(float amount)
    {
        circleCenter = enemy.transform.position + new Vector3(0, 2);
        print("circle center: " + circleCenter);
        print("enemy pos: " + enemy.transform.position);
        
        randomPos = new Vector3(Random.Range(circleCenter.x - circleRadius, circleCenter.x + circleRadius),
            Random.Range(circleCenter.y - circleRadius, circleCenter.y + circleRadius));
        
        GameObject damageNumber = Instantiate(enemyDamageNumber, randomPos, Quaternion.identity);
        textMeshPro = damageNumber.GetComponent<TextMeshPro>();
        textMeshPro.text = amount.ToString();
        damageNumber.transform.position = randomPos;
        textMeshPro.DOFade(0f, 2f);
        textMeshPro.transform.DOScale(new Vector3(0, 0, 0), 2f);
        textMeshPro.transform.DOMoveY(randomPos.y + 2f, 2f);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(circleCenter, circleRadius);
    }
}