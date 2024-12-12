using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{

    [Header("weapon, bullet, and shooting")]
    private float fireRate;
    private float lifeTime;
    private float lastShoot;


    [Header("Bullet")]
    [SerializeField] BulletScript bullet;
    [SerializeField] Transform barrelPos;

    [Header("Player")]
    [SerializeField] Transform playerHitTarget;

    [Header("Misc")]
    RaycastHit hit;

    public void ShootToPlayer()
    {
        if (Time.time > lastShoot + 1f / fireRate)
        {
            lastShoot = Time.time;
            barrelPos.LookAt(playerHitTarget);
            BulletScript currBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            currBullet.InitBoss(barrelPos);
        }
    }

    void Start()
    {
        fireRate = 2;
        lastShoot = 0;
    }

}