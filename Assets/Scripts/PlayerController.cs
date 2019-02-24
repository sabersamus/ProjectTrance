using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{

    public float speed = 100;
    public float sidewaysForce = 60;
    public Player player;
    public Rigidbody rigid;


    void Start()
    {
        player = GetComponent<Player>();
        rigid = player.GetComponent<Rigidbody>();

        rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;



    }

    void resetVelocity(Rigidbody rg)
    {
        rg.velocity = Vector3.zero;
        rg.angularVelocity = Vector3.zero;

    }



    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetButton("Forward"))
        {
            rigid.AddForce(0, 0, speed * Time.deltaTime);
        }

        if (Input.GetButton("Right"))
        {
            //resetVelocity(rigid);
            rigid.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

        }

        if (Input.GetButton("Backwards"))
        {
            rigid.AddForce(0, 0, -speed * Time.deltaTime);
        }


        if (Input.GetButton("Left"))
        {
            //resetVelocity(rigid);
            rigid.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {

    }

}
