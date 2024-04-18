using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamage : MonoBehaviour, IProjectileDamage
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private GameObject blast;

    public void TargetReached(float damage, GameObject target)
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radius);
        Instantiate(blast, transform.position, Quaternion.identity).transform.localScale = Vector3.one * radius * 2f;

        foreach(Collider2D c in collisions) {
            if(c.TryGetComponent<IHealth>(out IHealth h)) {
                h.DoDamage(damage);
            }
        }
    }
}
