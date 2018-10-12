using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {
    public float speed;
    public float dashSpeed;
    public float dashDist;
    private bool dashing = false;
    private float oldPos;
    private int dir;


    private VerticalCollide vertPhysics;

    // Use this for initialization
    void Start () {
        oldPos = transform.position.x;
        dir = 1;
        resetTimer = timerDash;
	}
	
	// Update is called once per frame
	void Update () {
        if (!dashing)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * speed, 0, 0);

        }
        else
        {
            Dash();
        }
        if (Input.GetButtonDown("Dash"))
        {
            oldPos = transform.position.x;
            dashing = true;
        }
	}


    private void SetDir()
    {
        if(oldPos < transform.position.x) //to the right
        {
            dir = 1;
        }
        if(oldPos > transform.position.x) //to the left
        {
            dir = -1;
        }
    }


    private void Dash() { 
        Debug.Log("Dashing");
        if (dir == 1)   //moving to the right
        {
            transform.position += new Vector3(dashSpeed* Time.deltaTime, 0, 0);
            if (transform.position.x - oldPos >= dashDist)
            {
                oldPos = transform.position.x;
                dashing = false;
            }
        }
        if (dir == -1)  //moving to the left
        {
            transform.position -= new Vector3(dashSpeed* Time.deltaTime, 0, 0);
            if (oldPos - transform.position.x >= dashDist)
            {
                oldPos = transform.position.x;
                dashing = false;
            }
        }
    }

}



        vertPhysics = GetComponent<VerticalCollide>();
    }
            vertPhysics.HorizontalSpeed =  Input.GetAxis("Horizontal") * speed;
