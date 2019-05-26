using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.ObjectManipulation;

[RequireComponent(typeof(Rigidbody))]

public class PlayerScript : Manipulator
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public GameObject PlayerPrefab;
    public GameObject Ball;
    public GameObject ManipulatorPrefab;
    Vector3 velocity;
    [Range(0, 1)]
    public float speed = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void fixedUpdate()
    {
        velocity = velocity.normalized * speed;
        Ball.transform.parent = ManipulatorPrefab.transform;
        transform.position += velocity;
    }
}
