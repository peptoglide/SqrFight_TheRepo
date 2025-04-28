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
    [Space(20)]
    public float shootDelay;
    public float reloadDelay;
    public int clipSize;
    public int bulletPerClip = 1;
    [Space(20)]
    public bool explosive;
    public ExplosionStats explosionStats;
}
