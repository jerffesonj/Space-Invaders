using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class EnemyManager : MonoBehaviour
{
    

    [SerializeField] private Vector2 startPos;
    [SerializeField] private int numLineEnemies;
    [SerializeField] private int numEnemiesInAline;
    [SerializeField] private float maxTimeAttack = 2;
    [SerializeField] private float maxTimeAttackUfo = 5;

    private ObjectPooling enemyPool;
    private ObjectPooling ufoPool;

    private float timeAttack;
    private float timeAttackUfo;

    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> ufos = new List<GameObject>();

    private AudioSource audioSource;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    private bool isChangingDirection = false;

    public List<GameObject> Enemies { get => enemies; }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GetEnemyPool();
        GetEnemyUFOPool();

        InitializeCommonEnemy();

        InitializeUFOEnemy();

        StartCoroutine(SelectRandomEnemy());
    }

    void GetEnemyPool()
    {
        foreach (ObjectPooling pool in GameManager.instance.pooling)
        {
            GameObject gameObject = pool.GetPooledObj();
            EnemyType enemy = gameObject.GetComponent<EnemyType>();

            if (enemy)
            {
                if (enemy.type == EnemyType.Type.Enemy)
                {
                    enemyPool = pool;
                    break;
                }
            }
        }
    }
    void GetEnemyUFOPool()
    {
        foreach (ObjectPooling pool in GameManager.instance.pooling)
        {
            GameObject gameObject = pool.GetPooledObj();
            EnemyType enemy = gameObject.GetComponent<EnemyType>();

            if (enemy)
            {
                if (enemy.type == EnemyType.Type.UFO)
                {
                    ufoPool = pool;
                    break;
                }
            }
        }
    }

    void InitializeCommonEnemy()
    {
        float xPos = startPos.x;
        float yPos = startPos.y;

        int enemyPoints = (numLineEnemies) * 10;

        for (int i = 0; i < numLineEnemies; i++)
        {
            int randomShip = UnityEngine.Random.Range(0, 20);

            for (int j = 0; j < numEnemiesInAline; j++)
            {
                GameObject enemy = enemyPool.GetPooledObj();

                InitializeEnemy(enemy, enemyPoints, randomShip, xPos, yPos);

                EnemyHp enemyHp = enemy.GetComponent<EnemyHp>();
                enemyHp.onEnemyDeathGameOver += GameOver;

                enemy.transform.position = new Vector2(xPos, yPos);
                enemies.Add(enemy);
                enemy.SetActive(true);

                xPos += 0.75f;
            }
            xPos = startPos.x;
            yPos -= 0.5f;
            enemyPoints = (numLineEnemies - i - 1) * 10;
        }
    }

    void InitializeUFOEnemy()
    {
        for (int i = 0; i < ufoPool.Pools.Count; i++)
        {
            GameObject enemy = ufoPool.Pools[i];

            int randomShip = UnityEngine.Random.Range(0, 4);
            int enemyPoints = UnityEngine.Random.Range(1, 10) * 1000;

            InitializeEnemy(enemy, enemyPoints, randomShip, 0, 0);

            ufos.Add(enemy);
        }
    }

    void InitializeEnemy(GameObject enemy, int enemyPoints, int randomShip, float xPos, float yPos)
    {
        enemy.GetComponent<Points>().ObjectPoints = enemyPoints;
        enemy.GetComponent<ChangeEnemyShip>().ChooseSprite(randomShip);

        EnemyHp enemyHp = enemy.GetComponent<EnemyHp>();
        enemyHp.AudioSource = audioSource;
        enemyHp.EnemyManager = this;
        enemyHp.onEnemyDeath += GameManager.instance.AddPoints;
    }

    private IEnumerator SelectRandomEnemy()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            UFOBehavior();

            EnemyBehavior();

            yield return null;
        }
    }

    #region UFO
    private void UFOBehavior()
    {
        timeAttackUfo += Time.deltaTime;
        if (timeAttackUfo >= maxTimeAttackUfo)
        {
            GameObject ufo = GetRandomUFOEnemy();
            SelectSideToSpawnUFO(ufo);

            StartCoroutine(WaitAndTurnOff(ufo));

            for (int i = 0; i < 5; i++)
            {
                ufo.GetComponent<EnemyAttack>().ShootRandomTime();
            }
            timeAttackUfo = 0;

            maxTimeAttackUfo = UnityEngine.Random.Range(10, 15);
        }
    }

    private GameObject GetRandomUFOEnemy()
    {
        int randomEnemy = UnityEngine.Random.Range(0, ufos.Count);
        for (int i = 0; i < 99; i++)
        {
            if (!ufos[randomEnemy].activeSelf)
            {
                randomEnemy = UnityEngine.Random.Range(0, ufos.Count);
                break;
            }
        }
        return ufos[randomEnemy];
    }

    private void SelectSideToSpawnUFO(GameObject ufo)
    {
        int randomPos = UnityEngine.Random.Range(0, 2);

        switch (randomPos)
        {
            case 0:
                ufo.transform.position = new Vector2(9.5f, 4);
                ufo.GetComponent<EnemyMovement>().direction = UnityEngine.Random.Range(25, 40);

                break;
            case 1:
                ufo.transform.position = new Vector2(-9.5f, 4);
                ufo.GetComponent<EnemyMovement>().direction = UnityEngine.Random.Range(-25, -40);

                break;
        }

        ufo.SetActive(true);
    }

    private IEnumerator WaitAndTurnOff(GameObject ufo)
    {
        yield return new WaitForSeconds(10);
        ufo.SetActive(false);
    }
    #endregion

    private void EnemyBehavior()
    {
        timeAttack += Time.deltaTime;
        if (timeAttack >= maxTimeAttack)
        {
            int randomEnemy = UnityEngine.Random.Range(0, enemies.Count);
            for (int i = 0; i < 99; i++)
            {
                if (!enemies[randomEnemy].activeSelf)
                {
                    randomEnemy = UnityEngine.Random.Range(0, enemies.Count);
                    break;
                }
            }
            enemies[randomEnemy].gameObject.GetComponent<EnemyAttack>().Shoot();
            timeAttack = 0;
        }
    }

    public void ChangeDirection()
    {
        if (isChangingDirection)
            return;

        StartCoroutine(WaitToCheckDirectionTrigger());
        foreach(GameObject enemy in enemies) 
        {
            enemy.GetComponent<EnemyMovement>().direction *= -1 * 1.1f;

            enemy.transform.DOMove(new Vector2(enemy.transform.position.x, enemy.transform.position.y - 0.5f), 0.5f);
        }
        AddSpeed();
    }

    public void AddSpeed()
    {
        foreach (GameObject enemy in enemies)
        {
            if (Mathf.Abs(enemy.GetComponent<EnemyMovement>().direction) < 6)
                enemy.GetComponent<EnemyMovement>().direction *= 1.05f;
        }
        maxTimeAttack -= maxTimeAttack/100;

    }

    IEnumerator WaitToCheckDirectionTrigger()
    {
        isChangingDirection = true;
        yield return new WaitForSeconds(2f) ;
        isChangingDirection = false;

    }

    public void GameOver()
    {
        if (enemies.Count <= 0)
            onGameOver?.Invoke();
    }
}
