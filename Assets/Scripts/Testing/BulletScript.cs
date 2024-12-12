using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    // TEMPLATE DATA

    private string sourcePlayer, sourcePatrol, sourceBoss;
    private float damagePatrol, damageBoss;
    private float speedPatrol, speedBoss;

    // Atribute

    private string source;
    private string type;
    private float damage;
    private float lifeTime;

    // Player

    public PlayerManager playerManager;
    public GameObject playerBody;
    public DamageEffect dmg;

    private void Awake()
    {

        sourcePlayer = "Player";
        sourcePatrol = "Patrol";
        sourceBoss = "Boss";

        speedPatrol = 100f;
        speedBoss = 400f;

        damagePatrol = 1;
        damageBoss = 1;

    }
    public void Init(string source, float damage, float speed, Transform caller)
    {
        this.source = source;
        this.damage = damage;

        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(caller.forward * speed, ForceMode.Impulse);

    }
    public void InitPatrol(Transform caller)
    {
        Init(sourcePatrol, damagePatrol, speedPatrol, caller);
    }
    public void InitBoss(Transform caller)
    {
        Init(sourceBoss, damageBoss, speedBoss, caller);
    }

    private void OnCollisionEnter(Collision Collided)
    {

        Destroy(this.gameObject);
        Debug.Log("bullet collided with " + Collided.transform.tag);

        // enemy to player
        if (source != sourcePlayer && Collided.transform.CompareTag("Player"))
        {
            Debug.Log("player kena dmg");
            dmg.FlashRed();
            playerManager.TakeDamage(1);
        }


    }

}