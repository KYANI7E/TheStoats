using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    private GameObject target;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private GameObject barrel;
    [SerializeField]
    private Transform barrelTip;

    [SerializeField]
    private float fireRate;
    private float coolDown;

    [SerializeField]
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TurnBarrel();
        FireProjectile();
    }

    private void TurnBarrel()
    {
        if (target == null)
            return;

        Vector2 direction = ((Vector2)target.transform.position - (Vector2)barrel.transform.position).normalized;
        transform.up = direction;
    }

    private void FireProjectile()
    {
        coolDown += Time.deltaTime;
        if (target == null)
            return;
        if (coolDown < fireRate)
            return;

        coolDown = 0;
        Instantiate(projectile, barrelTip.position, Quaternion.identity).GetComponent<Projectile>().Init(damage, target);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null)
            return;

        if(collision.GetComponent<IHealth>() != null) {
            target = collision.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (target == null)
            return;

        target = null;
    }
}
