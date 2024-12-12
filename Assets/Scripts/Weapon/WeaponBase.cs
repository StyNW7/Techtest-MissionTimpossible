using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{

    public Animator anim;

    public float bulletDamage;
    public float fireRate;
    public float bulletSpeed;
    public float bulletDrop;
    public int ammoCount;
    public int magazineSize;
    public int totalAmmo;

    protected float nextFireTime = 0f;

    protected bool isReloading = false;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public abstract void Shoot();
    public abstract bool CanShoot();
    //public abstract bool SetTotalAmmo();

    public void Reload()
    {
        if (isReloading)
        {
            Debug.Log("Already reloading!");
            return;
        }

        if (totalAmmo > 0 && ammoCount < magazineSize)
        {
            //Debug.Log("Mau Reload");
            isReloading = true;
            int bulletsToReload = Mathf.Min(magazineSize - ammoCount, totalAmmo);

            Debug.Log("Reloading...");
            StartCoroutine(PerformReload(bulletsToReload));
        }

        else
        {
            Debug.Log("No ammo left to reload!");
        }

    }

    // Reload ammo loading

    private IEnumerator PerformReload(int bulletsToReload)
    {
        anim.SetBool("Reload", true);
        Debug.Log("Mau Reload2");
        yield return new WaitForSeconds(2f); // Simulasi waktu reload
        ammoCount += bulletsToReload;
        totalAmmo -= bulletsToReload;
        Debug.Log($"Reload complete! Ammo: {ammoCount}/{totalAmmo}");
        isReloading = false; // Reset
        anim.SetBool("Reload", false);
    }

}
