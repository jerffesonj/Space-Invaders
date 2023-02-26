using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    
    [SerializeField] protected Transform bulletLocation;
    
    [SerializeField] private float maxTimeToAttack;
    [SerializeField] protected int damage;
    
    [SerializeField] private AudioClip[] shootClips;

    private ObjectPooling bulletPool;
    private AudioSource audioSource;
    private float timeToAttack;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetBulletPooling();

        timeToAttack = maxTimeToAttack;
    }

    protected void GetBulletPooling()
    {
        foreach (ObjectPooling pool in GameManager.instance.pooling)
        {
            GameObject gameObject = pool.GetPooledObj();

            BulletScript bullet = gameObject.GetComponent<BulletScript>();

            if (bullet)
            {
                if (bullet.thisBulletName == bulletPrefab.GetComponent<BulletScript>().thisBulletName)
                {
                    bulletPool = pool;
                    break;
                }
            }
        }
    }
    
    public void ShootSound()
    {
        int randomAudio = Random.Range(0, shootClips.Length);

        audioSource.PlayOneShot(shootClips[randomAudio]);
    }

    protected void Update()
    {
        timeToAttack += Time.deltaTime;
        if (timeToAttack >= maxTimeToAttack)
            timeToAttack = maxTimeToAttack;
    }

    protected void Shoot()
    {
        if (timeToAttack < maxTimeToAttack)
            return;

        GameObject bulletClone = bulletPool.GetPooledObj();
        bulletClone.GetComponent<BulletScript>().AddForce(bulletLocation, damage, this.gameObject);
        ShootSound();
        timeToAttack = 0;
    }
}
