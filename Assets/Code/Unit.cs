using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IHealth
{

    [SerializeField]
    private float maxHealth;
    private float curHealth;

    [SerializeField]
    private Slider slider;

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

        float g = Mathf.InverseLerp(0, maxHealth, curHealth);
        slider.value = g;
    }

    private void Die()
    {
        Spawning.instance.UnitDied();
        Destroy(this.gameObject);
    }
}
