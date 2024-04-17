using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IHealth
{

    [SerializeField]
    private float maxHealth;
    private float curHealth;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DoDamage(float damage)
    {
        curHealth -= damage;
        if(curHealth < 0) {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
