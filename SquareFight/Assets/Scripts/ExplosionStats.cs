using UnityEngine;

[CreateAssetMenu()]
public class ExplosionStats : ScriptableObject
{
    public float force;
    public float radius;
    [Space(20)]
    public float minDamage;
    public float maxDamage;
}
