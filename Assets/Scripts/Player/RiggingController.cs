using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RiggingController : MonoBehaviour
{
    
    public Rig equipPistolRig;
    public Rig equipRifleRig;

    public float transitionSpeed = 2f;

    private Coroutine currentCoroutine;

    
    public void EquipPistol()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(TransitionRigWeight(equipPistolRig, 1f));
        StartCoroutine(TransitionRigWeight(equipRifleRig, 0f));
    }

    
    public void EquipRifle()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(TransitionRigWeight(equipRifleRig, 1f));
        StartCoroutine(TransitionRigWeight(equipPistolRig, 0f));
    }

    
    public void NoWeapon()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        StartCoroutine(TransitionRigWeight(equipPistolRig, 0f));
        StartCoroutine(TransitionRigWeight(equipRifleRig, 0f));
    }

    
    private IEnumerator TransitionRigWeight(Rig rig, float targetWeight)
    {
        float initialWeight = rig.weight;
        float elapsedTime = 0f;

        while (!Mathf.Approximately(rig.weight, targetWeight))
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            rig.weight = Mathf.Lerp(initialWeight, targetWeight, elapsedTime);
            yield return null;
        }

        rig.weight = targetWeight;
    }

}
