using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleController : MonoBehaviour
{
    Vector3 velocity;
    [Range(0, 1)]
    public float speed = 0.1f;
    void OnCollisionEnter(Collision collision)
    {
        //Added by MA for collision with wall , position on the opposite
        if ((collision.gameObject.tag == "Ball") || (collision.gameObject.tag == "Player"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
