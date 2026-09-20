using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDamage : MonoBehaviour
{
    private Vector2 circleCenter;
    public float circleRadius = 2;
    private TextMeshPro textMeshPro;
    private Vector2 randomPos;
    [SerializeField] private GameObject enemyDamageNumber;
    public Enemy enemy;
    

    private void Awake()
    {
        textMeshPro = enemyDamageNumber.GetComponent<TextMeshPro>();
    }

    public void EnemyGetDamage(float amount)
    {
        circleCenter = enemy.transform.position + new Vector3(0, 2);
        print("circle center: " + circleCenter);
        print("enemy pos: " + enemy.transform.position);
        randomPos = new Vector3(Random.Range(circleCenter.x - circleRadius, circleCenter.x + circleRadius),
            Random.Range(circleCenter.y - circleRadius, circleCenter.y + circleRadius));
        Instantiate(enemyDamageNumber, randomPos, new Quaternion());
        textMeshPro.text = amount.ToString();
        enemyDamageNumber.transform.position = randomPos;

        /*Color transparentRed = new Color(255, 0, 0, 0);
        textMeshPro.DOColor(transparentRed, 2f);*/
        
        enemyDamageNumber.transform.DOScale(1f, 1f);
        
        /*textMeshPro.transform.DOMoveY(textMeshPro.transform.position.y + 0.5f, 1f)
            .SetEase(Ease.Linear);*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(circleCenter, circleRadius);
    }
}