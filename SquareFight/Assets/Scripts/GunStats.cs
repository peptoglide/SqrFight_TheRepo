using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    public string gunName;
    [Space(20)]
    public GameObject bullet;
    public float lifetime;
    public float force;
    public float damage;
    public float damageOffset = 2f;
    public float offset;
    public float verticalAdd = 0f;
    [Space(20)]
    public float shootDelay;
    public float reloadDelay;
    public int clipSize;
    public int bulletPerClip = 1;
    [Space(20)]
    public bool explosive;
    public ExplosionStats explosionStats;
    [Header("Tracker")]
    public bool tracking = false;
    public float trackDelay = 0f;
    public float trackTurnSpeed;
    public float trackSpeed;
}
