using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {

    private VerticalCollide vertPhysics;

    public string button;
    public float impulseForceH;
    public float impulseForceV;
    public enum Direction { RIGHT = 1, LEFT = -1};
    private Direction jumpDirection;
    public Direction JumpDirection
    {
        get { return jumpDirection;}
        set { jumpDirection = value; }
    }
    public int timerWallJump;
    private int timer;
    private bool isWalljumping;
    public bool IsWalljumping
    {
        get { return isWalljumping; }
        set { isWalljumping = value; }
    }
    private int dir;

    private void Start()
    {
        vertPhysics = GetComponent<VerticalCollide>();
        timer = timerWallJump;
        isWalljumping = false;
    }
    // Update is called once per frame
    void Update()
    {
        dir = (jumpDirection == Direction.RIGHT ? 1 : -1);
        if (Input.GetButtonDown(button))
        {
            isWalljumping = true;
        }
        if (isWalljumping)
        {
            if (timer >= 0)
            {
                timer--;
                vertPhysics.HorizontalSpeed += dir * impulseForceH * Time.deltaTime;
                vertPhysics.VerticalSpeed = impulseForceV * Time.deltaTime;
            }
            else
            {
                isWalljumping = false;
                timer = timerWallJump;
                GetComponent<Jump>().enabled = true;
                enabled = false;
            }
        }
    }
}
