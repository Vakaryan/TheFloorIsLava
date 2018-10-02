using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {



    private VerticalCollide vertPhysics;

    public float gravityAcceleration;

    private void Start()
    {
        vertPhysics = GetComponent<VerticalCollide>(); 
    }

    // Update is called once per frame
    void Update () {

        float speed = vertPhysics.VerticalSpeed;
        speed -= gravityAcceleration;
        vertPhysics.VerticalSpeed = speed;


    }
}
