using System;
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
        randomPos = new Vector2(Random.Range(circleCenter.x - circleRadius, circleCenter.x + circleRadius),
            Random.Range(circleCenter.y - circleRadius, circleCenter.y + circleRadius));
        Instantiate(enemyDamageNumber);
        textMeshPro.text = amount.ToString();
        textMeshPro.transform.position = randomPos;
    }
}