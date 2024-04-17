using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private float damage;
    private GameObject target;

    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(float _damge, GameObject _target)
    {
        damage = _damge;
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) {
            Destroy(this.gameObject);
        } else {
            TurnFacingTarget();
            MoveTowards();
            CheckHitTarget();
        }
    }

    private void CheckHitTarget()
    {
        if (Vector2.Distance(this.transform.position, target.transform.position) > 0.1f)
            return;

        target.GetComponent<IHealth>().DoDamage(damage);
        Destroy(this.gameObject);
    }

    private void MoveTowards()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

    }

    private void TurnFacingTarget()
    {
        Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        transform.up = direction;
    }
}
