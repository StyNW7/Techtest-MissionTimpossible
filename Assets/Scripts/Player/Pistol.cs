//using StarterAssets;
//using UnityEngine;

//public class Weapon : MonoBehaviour
//{
//    [SerializeField] int damageAmount = 1;

//    StarterAssetsInputs starterAssetsInputs;

//    void Awake()
//    {
//        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
//    }

//    void Update()
//    {
//        if (starterAssetsInputs.shoot)
//            HandleShoot();
//    }

//    void HandleShoot()
//    {
//        if (!starterAssetsInputs.shoot) return;

//        RaycastHit hit;

//        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
//        {
//            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
//            enemyHealth?.TakeDamage(damageAmount);

//            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
//            {
//                Debug.Log(hit.collider.name);
//                starterAssetsInputs.ShootInput(false);
//            }
//            starterAssetsInputs.ShootInput(false);
//        }
//    }

//}
