using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    Vector3 velocity;
    [Range(0, 1)]
    public float speed = 0.1f;
    void OnCollisionEnter(Collision collision)
    { 
        //Added by MA for collision with wall , position on the opposite
        if ((collision.gameObject.tag == "Ball")||(collision.gameObject.tag == "Player"))
        {

            var direction = (transform.position - collision.gameObject.transform.position).normalized;
            var force = direction * speed;
            collision.gameObject.GetComponent<Rigidbody>().velocity = force / 2;
        }
    }
}
