using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectionEnemieTrigger : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovement>())
        {
            enemyManager.ChangeDirection();
        }
    }
}
