using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {
    public float speed;
    public float dashSpeed;
    private bool dashing = false;
    private int oldDir = 1;
    private int dir;
    public int timerDash;
    private int timer;


    private VerticalCollide vertPhysics;

    // Use this for initialization
    void Start () {
        dir = 1;
        vertPhysics = GetComponent<VerticalCollide>();
        timer = timerDash;
	}
	
	// Update is called once per frame
	void Update () {
        if(vertPhysics.HorizontalSpeed != 0)
        {
            if(vertPhysics.HorizontalSpeed > 0)
            {
                oldDir = 1;
            }
            else
            {
                oldDir = -1;
            }
        }
        if (!dashing)
        {
            vertPhysics.HorizontalSpeed = Input.GetAxis("Horizontal") * speed;

        }
        else
        {
            if (timer >= 0)
            {
                Debug.Log("Dashing");
                vertPhysics.HorizontalSpeed += dashSpeed * dir * Time.deltaTime;
                timer--;
            }
            else
            {
                timer = timerDash;
                dashing = false;
            }
        }
        if (Input.GetButtonDown("Dash"))
        {
            SetDir();
            dashing = true;
        }
	}


    private void SetDir()
    {
        if(vertPhysics.HorizontalSpeed > 0) //to the right
        {
            dir = 1;
        }
        else if(vertPhysics.HorizontalSpeed < 0) //to the left
        {
            dir = -1;
        }
        else
        {
            dir = oldDir;
        }
    }


}

