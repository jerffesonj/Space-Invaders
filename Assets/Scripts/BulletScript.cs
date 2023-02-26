using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float maxTimeAlive = 5;
    [SerializeField] private float bulletspeed;
    [SerializeField] private int damage;
    [SerializeField] private GameObject owner;
    [SerializeField] private LaserFxScript laserType;

    public enum BulletNames
    {
        Bullet,
        UFOBullet
    }

    public BulletNames thisBulletName;

    private Rigidbody2D rb;
    private float timeAlive;
    private ObjectPooling fxPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        foreach (ObjectPooling pool in GameManager.instance.pooling)
        {
            GameObject gameObject = pool.GetPooledObj();
            if (gameObject.GetComponent<LaserFxScript>())
            {
                if (gameObject.GetComponent<LaserFxScript>().thisLaserName == laserType.thisLaserName)
                {
                    fxPool = pool;
                    break;
                }
            }
        }
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > maxTimeAlive)
        {
            timeAlive = 0;
            gameObject.SetActive(false);
        }
    }
    public void AddForce(Transform bulletLocation, int damage, GameObject owner)
    {
        transform.position = bulletLocation.position;
        this.owner = owner;
        this.damage = damage;
        gameObject.SetActive(true);
        rb.AddForce(bulletLocation.up * bulletspeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject fx = fxPool.GetPooledObj();

        if (collision.gameObject.GetComponent<Hp>())
        {
            if (collision.gameObject.CompareTag("Blocks"))
            {
                BlockCollision(collision, fx);
                return;
            }

            if (owner.CompareTag("Player"))
            {
                EnemyDamage(collision, fx);
            }
            else if (owner.CompareTag("Enemy"))
            {
                if (!collision.gameObject.CompareTag("Enemy"))
                {
                    EnemyDamage(collision, fx);
                }
            }
        }
    }

    private void BlockCollision(Collider2D collision, GameObject fx)
    {
        collision.gameObject.GetComponent<Hp>().RemoveHp(damage);
        fx.transform.position = transform.position;
        fx.SetActive(true);
        if (thisBulletName != BulletNames.UFOBullet)
            StartCoroutine(TurnOffBullet());
    }

    private void EnemyDamage(Collider2D collision, GameObject fx)
    {
        collision.gameObject.GetComponent<Hp>().RemoveHp(damage);
        fx.transform.position = collision.transform.position;
        fx.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator TurnOffBullet()
    {
        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        timeAlive= 0;
    }
}
