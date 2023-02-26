using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public delegate void GameOverTrigger();
    public static event GameOverTrigger gameOverTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyMovement>())
        {
            gameOverTrigger?.Invoke();
        }
    }
}
