using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject gunBreakEff;
    public GunStats stats;
    public Team team;

    public bool autoAim = true;

    [Header("Aiming")]
    public float aimTime;
    public float aimInaccuracy;
    public GameObject pivot;

    public bool isShooting;
    public float cooldown;

    bool isReloading;

    public int currentMag;
    public bool playSound = true;
    [Header("UI")]
    public TextMeshProUGUI ammoCounter;

    [Header("Upgrades")]
    public float damageMultiplier = 1f;

    Transform red;
    Transform blue;
    GameManager manager;
    InputManager input_manager;
    void Start()
    {
        Debug.Log("Hello?");
        manager = GameManager.instance;
        red = GameObject.FindGameObjectWithTag("Player 2").transform;
        blue = GameObject.FindGameObjectWithTag("Player").transform;
        input_manager = GetComponentInParent<InputManager>();
        cooldown = 0;

        // Abandon weapon on command
        input_manager.onDiscardWeapon += TryToBreakGun;
        Debug.Log($"My input manager is {input_manager.gameObject.transform.name}", gameObject);
        if (stats == null) return;
        currentMag = stats.clipSize;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (stats == null) manager.ClearGunUI(team); // Clear gun stats
        if (stats == null) return;

        manager.UpdateGunUI(currentMag, stats, team);
        cooldown += Time.deltaTime;
        if (input_manager.isShooting && cooldown >= stats.shootDelay && currentMag > 0 && !isReloading)
        {
            Fire();
            // Reset variables
            cooldown = 0;
            currentMag--;
        }

        if(currentMag <= 0 && !isReloading)
        {
            // Break gun
            GunBreak();
        }
        

        // Aim
        if (!autoAim) return;
        if(team == Team.Blue){
            Aim(red);
        }
        else if(team == Team.Red){
            Aim(blue);
        }
    }

    void TryToBreakGun(){
        if(stats == null) return;
        GunBreak();
    } 

    public void Fire()
    {
        for (int i = 0; i < stats.bulletPerClip; i++)
        {
            GameObject bullet = Instantiate(stats.bullet, transform.position, transform.rotation);
            if (bullet.TryGetComponent(out Rigidbody2D rb))
            {
                float offsetForce = Random.Range(-stats.offset, stats.offset);
                Vector2 force = transform.right * stats.force + Random.Range(-offsetForce, offsetForce) * transform.up;

                rb.AddForce(force, ForceMode2D.Impulse);
            }

            if (bullet.TryGetComponent(out Bullet bull))
            {
                bull.stats = stats;
                bull.damageMultiplier = damageMultiplier;
            }
        }
        if (playSound)
        {
            AudioManager.instance.PlaySound("Shoot");
        }
    }

    public void SetShoot(bool state)
    {
        isShooting = state;
    }

    void Reload()
    {
        currentMag = stats.clipSize;
        isReloading = false;
    }

    void GunBreak()
    {
        stats = null;
        if(gunBreakEff != null)
        {
            GameObject newEff = Instantiate(gunBreakEff, transform.position, Quaternion.identity);
            Destroy(newEff, 2.5f);
        }
        AudioManager.instance.PlaySound("WeaponBreak");
    }

    void Aim(Transform target)
    {
        if (target == null) return;
        Vector2 dir = target.position - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
