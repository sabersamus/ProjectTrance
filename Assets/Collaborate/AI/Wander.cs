using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using Random = UnityEngine.Random;


/// <summary>
/// wanderers around a point
/// </summary>
public class Wander : MonoBehaviour
{

    public float radius;


    
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
        
        
        agent = GetComponent<NavMeshAgent>();

        if (agent is null) throw new Exception("NavMeshAgent is Null");
        else if (!agent.isOnNavMesh) Debug.Log("Agent is not on navmesh");


        agent.SetDestination(GetRandomLocation());


    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            Vector3 loc = GetRandomLocation();
           
            agent.SetDestination(loc);
        }
    }

    Vector3 GetRandomLocation()
    {

        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(transform.position + (Random.insideUnitSphere * radius), out hit, radius, 1 << NavMesh.GetAreaFromName("Walkable")))            
        {

           

            

           
            return hit.position;
        }       

        //fuck!
        return Vector3.zero;


    }

    void OnGUI()
    {
#if UNITY_EDITOR


#endif
    }
}

