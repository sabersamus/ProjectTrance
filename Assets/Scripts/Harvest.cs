using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Harvest : MonoBehaviour
{


    public AudioSource auidoSource;
    public float respawnTime = 10f;
    public float startingHealth;
    [SerializeField] public GameItem matType;
    public float randomRangeMin;
    public float randomRangeMax;

    private Collider targetCollider;
    private MeshRenderer render;
    private Animator anim;
    private float health;

    [HideInInspector]
    public float harvestAmount;

    void Start()
    {
        targetCollider = gameObject.GetComponent<Collider>();
        render = gameObject.GetComponent<MeshRenderer>();
        //render.transform.position = targetCollider.transform.position;
        health = startingHealth;
        anim = gameObject.GetComponent<Animator>();
    }


    public void harvestMaterials()
    {
        harvestAmount = Random.Range(randomRangeMin, randomRangeMax);
        if (health - harvestAmount <= 0)
        {
            health = 0;
            StartCoroutine(Respawn());
        }
        else
        {
            health -= harvestAmount;
        }
    }

    IEnumerator Respawn()
    {
        if (anim != null)
        {
            gameObject.GetComponent<Animator>().SetBool("chopped", true);
            yield return new WaitForSeconds(3f);
        }


        targetCollider.enabled = false;
        render.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        if (anim != null)
        {

            gameObject.GetComponent<Animator>().SetBool("chopped", false);
            yield return new WaitForSeconds(1f);
        }

        targetCollider.enabled = true;
        render.enabled = true;
        health = startingHealth;
    }

}