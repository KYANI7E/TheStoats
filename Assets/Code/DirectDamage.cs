using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectDamage : MonoBehaviour, IProjectileDamage
{

    public void TargetReached(float damage, GameObject target)
    {
        if(target.TryGetComponent<IHealth>(out IHealth h))
            h.DoDamage(damage);
            
    }
}
