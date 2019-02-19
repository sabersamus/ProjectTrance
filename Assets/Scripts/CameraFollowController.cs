using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    public Transform player;
    public float offsetDistance;
    public Vector3 offsetPosition;
    public Quaternion offsetRotation;
    Transform position;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        position = gameObject.GetComponent<Transform>();
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        position.position = ((player.position + offsetPosition) - (player.forward * offsetDistance));
        position.LookAt(player);
        position.Rotate(offsetRotation.eulerAngles);
        
    }
}
