using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IHealth, IHealBuff
{

    [SerializeField]
    private float maxHealth;
    private float curHealth;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private ParticleSystem ps;
    private float healTimer;
    private float healCoolDown;
    private float healAmount;

    public int soulCost;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        Heal();
    }

    private void Heal()
    {
        healCoolDown += Time.deltaTime;
        if(healCoolDown > healTimer) {
            ps.Stop();
        } else {
            curHealth += healAmount * Time.deltaTime;
            if (curHealth > maxHealth)
                curHealth = maxHealth;
        }
        float g = Mathf.InverseLerp(0, maxHealth, curHealth);
        slider.value = g;
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

    public void HealBuffed(float health, float healT)
    {
        ps.Play();
        healAmount = health;
        healTimer = healT;
        healCoolDown = 0;
    }
}
