using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHp : Hp
{
    private EnemyManager enemyManager;
    private ObjectPooling pointPooling;
    private Points enemyPoints;
    private AudioSource audioSource;
    
    [SerializeField] private AudioClip deathClip;
    public EnemyManager EnemyManager { get => enemyManager; set => enemyManager = value; }
    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }

    public delegate void OnEnemyDeath(int value);
    public event OnEnemyDeath onEnemyDeath;
    public delegate void OnEnemyDeathGameOver();
    public event OnEnemyDeathGameOver onEnemyDeathGameOver;

    void Start()
    {
        GetPointsPooling();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyPoints = GetComponent<Points>();
    }

    public void GetPointsPooling()
    {
        foreach (ObjectPooling pool in GameManager.instance.pooling)
        {
            if (pool.GetPooledObj().GetComponent<Canvas>())
            {
                pointPooling = pool;
                break;
            }
        }
    }

    public override void RemoveHp(int damage)
    {
        currentHp -= damage;
       
        if (currentHp <= 0)
        {
            audioSource.PlayOneShot(deathClip);

            GetPointsCanvas();

            enemyManager.AddSpeed();
            enemyManager.Enemies.Remove(this.gameObject);

            currentHp = maxHp;
            gameObject.SetActive(false);

            onEnemyDeath?.Invoke(enemyPoints.ObjectPoints);
            onEnemyDeathGameOver?.Invoke();
        }
    }
    private void GetPointsCanvas()
    {
        GameObject canvasObj = pointPooling.GetPooledObj();
        canvasObj.GetComponentInChildren<TMP_Text>().text = enemyPoints.ObjectPoints.ToString();
        canvasObj.transform.position = this.transform.position;
        canvasObj.SetActive(true);
    }
}
