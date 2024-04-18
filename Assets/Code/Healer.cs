using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{

    [SerializeField]
    private float healPerSec;
    [SerializeField]
    private float healLast;

    [SerializeField]
    private float radius;

    [SerializeField]
    private float castRate;
    private float castCoolDown;

    [SerializeField]
    private GameObject blast;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Heal();
    }

    private void Heal()
    {
        castCoolDown += Time.deltaTime;
        if (castCoolDown < castRate)
            return;
        castCoolDown = 0;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radius);
        Instantiate(blast, transform.position, Quaternion.identity).transform.localScale = Vector3.one * radius * 2f;

        foreach (Collider2D c in collisions) {
            if (c.TryGetComponent<IHealBuff>(out IHealBuff h)) {
                h.HealBuffed(healPerSec, healLast);
            }
        }
    }
}
