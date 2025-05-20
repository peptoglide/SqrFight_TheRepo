using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LifetimedObject
{
    public GameObject obj;
    public float lifetime;
}

public class SpawnOnDestroyed : MonoBehaviour
{
    [SerializeField] LifetimedObject[] spawns;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        foreach (LifetimedObject obj in spawns)
        {
            GameObject spawned = Instantiate(obj.obj, transform.position, transform.rotation);
            Destroy(spawned, obj.lifetime);
        }
    }
}
