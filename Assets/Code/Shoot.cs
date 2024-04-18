using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    private GameObject target;
    private List<GameObject> targetList = new List<GameObject>();

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private GameObject barrel;
    [SerializeField]
    private Transform barrelTip;

    [SerializeField]
    private int range;

    [SerializeField]
    private float fireRate;
    private float coolDown;

    [SerializeField]
    private float damage;
    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private AudioClip shootSound;

    [SerializeField]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = range + .5f;
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
        barrel.transform.up = direction;
    }

    private void FireProjectile()
    {

        coolDown += Time.deltaTime;

        if (GameState.instance.state != State.Play)
            return;

        if (target == null) {
            while(targetList.Count != 0) {
                target = targetList[0];
                targetList.RemoveAt(0);
                if (target != null)
                    break;
            }
            if (target == null)
                return;
        }
        if (coolDown < fireRate)
            return;

        if (anim != null)
            anim.SetTrigger("Shoot");

        coolDown = 0;
        Instantiate(projectile, barrelTip.position, Quaternion.identity).GetComponent<Projectile>().Init(damage, projectileSpeed, target);
        PlayAudio.Instance.PlayClip(shootSound);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Unit")) {
            return;
        }
        if(collision.GetComponent<IHealth>() != null) {
            targetList.Add(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (targetList.Contains(collision.gameObject)) {
            targetList.Remove(collision.gameObject);
        }

    }
}
