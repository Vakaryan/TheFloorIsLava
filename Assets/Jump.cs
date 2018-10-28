using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    private VerticalCollide vertPhysics;

    public string button;
    public float impulseForce;
    private int jumpCount;

    private void Start()
    {
        vertPhysics = GetComponent<VerticalCollide>();
        jumpCount = 0;
    }
    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown(button))
        {
            if(jumpCount < 2)
            {
                vertPhysics.VerticalSpeed = impulseForce;
                jumpCount++;
                Debug.Log("Jump count = " + jumpCount);
            }
            else
            {
                jumpCount = 0;
            }
        }
	}
}
