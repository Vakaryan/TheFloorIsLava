using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    private VerticalCollide vertPhysics;

    public string button;
    public float impulseForce;

    private void Start()
    {
        vertPhysics = GetComponent<VerticalCollide>();
    }
    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown(button))
        {
            vertPhysics.VerticalSpeed += impulseForce;
        }
	}
}
