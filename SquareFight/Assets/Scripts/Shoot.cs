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
    void Start()
    {
        manager = GameManager.instance;
        red = GameObject.FindGameObjectWithTag("Player 2").transform;
        blue = GameObject.FindGameObjectWithTag("Player").transform;
        isShooting = false;
        cooldown = 0;

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
        if (isShooting && cooldown >= stats.shootDelay && currentMag > 0 && !isReloading)
        {
            Fire();
            // Reset variables
            cooldown = 0;
            currentMag--;
        }

        if(currentMag <= 0 && !isReloading)
        {
            /*
            isReloading = true;
            Invoke(nameof(Reload), stats.reloadDelay);
            */

            // Break gun
            GunBreak();
        }
        

        // Aim
        if (!autoAim) return;
        if(team == Team.Blue)
        {
            Aim(red);
            // Abandon weapon
            if (Input.GetKeyDown(KeyCode.Q) && stats != null)
            {
                GunBreak();
            }
        }
        else if(team == Team.Red)
        {
            Aim(blue);
            if (Input.GetKeyDown(KeyCode.Period) && stats != null)
            {
                GunBreak();
            }
        }

        if(team == Team.Red)
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                isShooting = true;
            }
            if (Input.GetKeyUp(KeyCode.Slash))
            {
                isShooting = false;
            }
        }

        if (team == Team.Blue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isShooting = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isShooting = false;
            }
        }

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
