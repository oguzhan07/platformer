using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Type", menuName = "Enemy Types")]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public float enemyHealth;
    public float enemyMoveSpeed;
    public float enemyJumpSpeed;
    public float enemyFollowDistance;
    public float enemyAttackDistance;
    public Sprite enemySprite;
}
