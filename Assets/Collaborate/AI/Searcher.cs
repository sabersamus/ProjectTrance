using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

using System.Linq;
using System;

using UnityEngine.SceneManagement;



using Random = UnityEngine.Random;

/// <summary>
/// the style of seaching the searcher will use to find its target
/// </summary>
public enum SearchStyle
{
    /// <summary>
    /// search for the first target in the list [0]
    /// </summary>
    [Tooltip("search for the first target in the list [0]")]
    IndexFirst,
    /// <summary>
    /// search for the last object in the list [Count -1]
    /// </summary>
    [Tooltip("search for the last object in the list [Count -1]")]
    IndexLast,
    /// <summary>
    /// search for the object closest to the searcher.
    /// </summary>
    [Tooltip("search for the object closest to the searcher.")]
    Closest,
    /// <summary>
    /// search for the farthest  object away from the searcher
    /// </summary>
    [Tooltip("search for the farthest  object away from the searcher")]
    Farthest,
    /// <summary>
    /// search for anything in the list.
    /// </summary>
    [Tooltip("search for anything in the list.")]
    Any
}

/// <summary>
/// search for the actual object stored in the list of targets or just any object 
/// </summary>
public enum SearchTargetType
{
    /// <summary>
    /// search for any object in the scene that matches types in the list
    /// </summary>
    [Tooltip("search for any object in the scene that matches types in the list")]
    Type,
    /// <summary>
    /// Search the scene for the actual object in the list 
    /// </summary>
    [Tooltip("Search the scene for the actual object in the list ")]
    Object
}

/// <summary>
/// searches the navmesh for the target
/// </summary>
[RequireComponent(typeof(NavMeshAgent),typeof(Collider))]
public class Searcher : MonoBehaviour
{


    /// <summary>
    /// the list of targets the searcher will search for
    /// </summary>
    [SerializeField, Tooltip("the list of targets the searcher will search for")]
    List<GameObject> targets;

    [Header("Seach Settings")]

    /// <summary>
    /// "the distance the boxcast searches"
    /// </summary>
    [SerializeField, Tooltip("the distance the boxcast searches")]
    float lookdistance = 15f;

    /// <summary>
    /// "the style of seaching the searcher will use to find its target"
    /// </summary>
    [SerializeField, Tooltip("the style of seaching the searcher will use to find its target")]
    SearchStyle search_style = SearchStyle.Any;

    /// <summary>
    /// "search for the actual object stored in the list of targets or just any object"
    /// </summary>
    [SerializeField, Tooltip("search for the actual object stored in the list of targets or just any object")]
    SearchTargetType search_target_type = SearchTargetType.Type;

    /// <summary>
    /// "the distance the searcher may travel at a time"
    /// </summary>
    [SerializeField, Tooltip("the distance the searcher may travel at a time")]
    float wander_distance = 20f;




    /// <summary>
    /// fires when the searcher finds what it is looking for
    /// </summary>
    [Header("Events"), Tooltip("fires when the searcher finds what it is looking for")]
    public UnityEvent<SeacherEventArgs> found_target;
    void OnFoundTarget(SeacherEventArgs e) { found_target?.Invoke(e); }// ?. does nothing if found_target is null.


    #region inside fields

    NavMeshAgent agent;
    GameObject target;
    bool has_target = false;


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if(targets == null)targets = new List<GameObject>();


        agent.SetDestination(GetRandomLocation());

    }




    // Update is called once per frame
    void Update()
    {

        //are we there yet
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)//why doesn't unity do this?
        {
            //if we actually found what we were looking for
            if (has_target)
            {
                has_target = false;

                OnFoundTarget(new SeacherEventArgs(target));
            }

            //what ever move on
            agent.SetDestination(GetRandomLocation());
        }

        // this sections needs optimized with a bunch of "inside" fields and outside gameobject changes like useing interfaces or scripts in potential target objects
        // i use "Linq" to filter Root game objects from the active scene and to filter the targets.

    }

    /// <summary>
    /// gets a random location on the nav mesh
    /// </summary>
    /// <returns>a vector3 that is a locatin on the navhesh we can traverse to </returns>
    Vector3 GetRandomLocation()
    {

        NavMeshHit hit;

        if (NavMesh.SamplePosition(transform.position + (Random.insideUnitSphere * wander_distance), out hit, wander_distance, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            return hit.position;
        }

        //fuck!
        return Vector3.zero;


    }




    /// <summary>
    /// adds a terget to the list
    /// </summary>
    /// <param name="target_object">the game object you wish to be a target of the searcher</param>
    public void AddTarget(GameObject target_object)
    {
        targets.Add(target_object);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!has_target)
        {

            //find the target based on the search style and target type
            switch (search_style)
            {
                case SearchStyle.Any:

                    switch (search_target_type)
                    {
                        case SearchTargetType.Object:


                            
                            if (targets.Contains(other.transform.gameObject))
                            {
                                agent.SetDestination(other.transform.position);
                                target = other.transform.gameObject;
                                has_target = true;


                            }



                            break;
                        case SearchTargetType.Type:

                            
                            if (targets.Where(b =>  b.name.Equals(other.transform.gameObject.name)).Count() > 0)
                            {
                                
                                agent.SetDestination(other.transform.position);
                                target = other.transform.gameObject;
                                has_target = true;

                            }


                            break;
                    }

                    break;
                case SearchStyle.Closest:

                    switch (search_target_type)
                    {
                        case SearchTargetType.Object:
                            {
                                //you will have to remove the game object from targets when it reaches it or it will never move on.
                                GameObject[] close_objects = targets.OrderBy(a => Vector3.Distance(transform.position, a.transform.position)).ToArray();

                                if (close_objects.Length > 0)
                                {


                                    agent.SetDestination(other.transform.position);
                                    target = other.transform.gameObject;
                                    has_target = true;



                                }

                                break;
                            }
                        case SearchTargetType.Type:
                            {

                                //must remove game object from game for this to move on or all targets with the same name
                                GameObject[] close_objects = SceneManager.GetActiveScene().GetRootGameObjects().Where(a => targets.Select(b => b.name).Contains(a.name)).OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();

                                if (close_objects.Length > 0)
                                {

                                    agent.SetDestination(other.transform.position);
                                    target = other.transform.gameObject;
                                    has_target = true;


                                }

                                break;
                            }
                    }

                    break;
                case SearchStyle.Farthest:

                    switch (search_target_type)
                    {
                        case SearchTargetType.Object:
                            {
                                //you will have to remove the game object from targets when it reaches it or it will never move on.
                                GameObject[] close_objects = targets.OrderByDescending(a => Vector3.Distance(transform.position, a.transform.position)).ToArray();

                                if (close_objects.Length > 0)
                                {

                                    agent.SetDestination(other.transform.position);
                                    target = other.transform.gameObject;
                                    has_target = true;


                                }

                                break;
                            }
                        case SearchTargetType.Type:
                            {

                                //must remove game object from game for this to move on or all targets with the same name
                                GameObject[] close_objects = SceneManager.GetActiveScene().GetRootGameObjects().Where(a => targets.Select(b => b.name).Contains(a.name)).OrderByDescending(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();

                                if (close_objects.Length > 0)
                                {


                                    agent.SetDestination(other.transform.position);
                                    target = other.transform.gameObject;
                                    has_target = true;


                                }

                                break;
                            }
                    }

                    break;
                case SearchStyle.IndexFirst:

                    switch (search_target_type)
                    {
                        case SearchTargetType.Object:
                            {


                                if (other.transform.gameObject == targets[0])
                                {
                                    agent.SetDestination(other.transform.position);
                                    target = other.transform.gameObject;
                                    has_target = true;

                                }



                                break;
                            }
                        case SearchTargetType.Type:


                            GameObject[] similiar_objects = SceneManager.GetActiveScene().GetRootGameObjects().Where(a => a.name == targets[0].name).ToArray();

                            if (similiar_objects.Length > 0)
                            {
                                agent.SetDestination(similiar_objects[0].transform.position);
                                target = other.transform.gameObject;
                                has_target = true;

                            }

                            break;
                    }

                    break;
                case SearchStyle.IndexLast:

                    switch (search_target_type)
                    {
                        case SearchTargetType.Object:
                            {


                                if (other.transform.gameObject == targets[targets.Count - 1])
                                {
                                    agent.SetDestination(other.transform.position);
                                    target = other.transform.gameObject;
                                    has_target = true;

                                }



                                break;
                            }
                        case SearchTargetType.Type:


                            GameObject[] similiar_objects = SceneManager.GetActiveScene().GetRootGameObjects().Where(a => a.name == targets[targets.Count - 1].name).ToArray();


                            if (similiar_objects.Length > 0)
                            {
                                agent.SetDestination(similiar_objects[targets.Count - 1].transform.position);
                                target = other.transform.gameObject;
                                has_target = true;

                            }

                            break;
                    }

                    break;
            }
        }
    }


}

/// <summary>
/// so far just the game object we found 
/// </summary>
public class SeacherEventArgs
{
    /// <summary>
    /// the object we were looking for
    /// </summary>
    public readonly GameObject go;
    /// <summary>
    /// Creataes a SearcherEventArgs
    /// </summary>
    /// <param name="_go">the GameObject we found</param>
    public SeacherEventArgs(GameObject _go)
    {
        go = _go;


    }

   

}