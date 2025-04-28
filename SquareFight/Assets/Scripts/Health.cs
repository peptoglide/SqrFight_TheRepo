using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class Health : MonoBehaviour
{
    
    public float maxHealth;

    public bool isPlayer = true;
    public float currentHealth;

    public Team team;

    [Header("Telegraph")]
    public Slider slider;
    public bool spawnIndicato;
    public GameObject indicator;
    // Effect
    public GameObject effects;
    public float effLifetime;

    GameManager manager;
    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;

        manager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0f)
        {
            if(team == Team.Blue)
            {
                manager.CountRedScore(1);
                manager.Invoke("ReloadScene", 2f);
            }
            if(team == Team.Red)
            {
                manager.CountBlueScore(1);
                manager.Invoke("ReloadScene", 2f);
            }
            if(effects != null)
            {
                GameObject eff = Instantiate(effects, transform.position, Quaternion.identity);
                Destroy(eff, effLifetime);
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, bool spawnIndicator = true)
    {
        currentHealth -= damage;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        slider.value = currentHealth;
        if (spawnIndicato && spawnIndicator)
        {
            GameObject indic = Instantiate(indicator, transform.position, Quaternion.identity);
            if(indic.TryGetComponent(out DamageIndicator d))
            {
                d.UpdateText(damage.ToString("F1"));
            }
        }
    }
}
