using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.ObjectManipulation;
using System;

[RequireComponent(typeof(Rigidbody))]

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    // public GameObject Camera;
    //public GameObject Player;
    //public GameObject Ball;
    Vector3 velocity;
    [Range(0, 1)]
    public float speed = 0.1f;
    void Start()
    {

    }

    /* Update is called once per frame
    void fixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit)) //if hit a 3D Object
        {
            if (hit.collider.tag == "Player")
            {
                OnCollisionEnter(Player.GetComponent<Collision>());


            }

        }

    }*/

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
          
            var direction = (collision.gameObject.transform.position - transform.position).normalized;
            // normalize force vector to get direction only and trim magnitude
            //Ball.GetComponent<Rigidbody>().AddForce(direction * magnitude);
            var force = direction * speed;
            //Ball.GetComponent<Rigidbody>().velocity = force;*/
            collision.gameObject.GetComponent<Rigidbody>().velocity = force;

        }


        //Added by MA for collision with wall
        else if (collision.gameObject.tag == "Wall")
        {

            var direction1 = (transform.position - collision.gameObject.transform.position).normalized;
            var force1 = direction1 * speed;
            collision.gameObject.GetComponent<Rigidbody>().velocity = force1 ;
        }
    }

}









