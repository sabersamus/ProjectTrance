using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject thing;
    public float delay;
    float current,last;

    public event EventHandler<SpawnEventArgs> ThingSpawned;
    void OnThingSpawned(SpawnEventArgs e) { if (ThingSpawned != null) ThingSpawned(this, e); }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current += Time.deltaTime;

        if(current > last + delay)
        {
            Instantiate(thing, transform.position, transform.rotation);

            last = current;

        }
    }
}

/// <summary>
/// information about what was spawned
/// </summary>
public class SpawnEventArgs : EventArgs
{
    /// <summary>
    /// the game object that was spawned
    /// </summary>
    public readonly GameObject thing;


    public SpawnEventArgs(GameObject _thing)
    {
        thing = _thing;
    }

}