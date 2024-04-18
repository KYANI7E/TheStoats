using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileDamage
{
    public void TargetReached(float damage, GameObject target);
}
