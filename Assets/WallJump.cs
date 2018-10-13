using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {

    private VerticalCollide vertPhysics;

    public string button;
    public float impulseForce;
    public enum Direction { RIGHT = 1, LEFT = -1};
    private Direction jumpDirection;
    public Direction JumpDirection
    {
        get { return jumpDirection;}
        set { jumpDirection = value; }
    }

    private void Start()
    {
        vertPhysics = GetComponent<VerticalCollide>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(button))
        {
            vertPhysics.VerticalSpeed += impulseForce;
            vertPhysics.HorizontalSpeed += (jumpDirection == Direction.RIGHT ? 1 : -1) * impulseForce;
            GetComponent<Jump>().enabled = true;
            enabled = false;
        }
    }
}
