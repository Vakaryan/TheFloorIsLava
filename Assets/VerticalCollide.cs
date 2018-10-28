using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VerticalCollide : MonoBehaviour {

    private Collider2D playerCollider;

    private Transform playerTransform;

    public float verticalSpeed  = 0;
    public float VerticalSpeed {
        get { return verticalSpeed; }
        set { verticalSpeed = value; } }

    public float horizontalSpeed = 0;
    public float HorizontalSpeed
    {
        get { return horizontalSpeed; }
        set { horizontalSpeed = value; }
    }
    

    private Ray2D rayA;
    private Ray2D rayB;
    private Ray2D rayC;
    private Ray2D rayD;

    public float precision;



    enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }



    // Use this for initialization
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerTransform = GetComponent<Transform>();
    }

/*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(rayA.origin, rayA.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayA.origin);
        Gizmos.DrawLine(rayB.origin, rayB.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayB.origin);
        Gizmos.DrawLine(rayC.origin, rayC.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayC.origin);
        Gizmos.DrawLine(rayD.origin, rayD.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayD.origin);



    }*/


    private void CollisionManager()
    {

        // Rework des collisions :

        // Todo : 4 raycast

        // Collision :
        //      - la plus proche -> déplacement au contact de la BondingBox !  => devrait fonctionner !
        // Sinon : va !

        // Problèmes : [] ---  avec déplacement horizontal -> pas de collision
        // Est-ce un problème ? (cf jeux de plateforme classiques.)

        // Attention : ray du bas / du haut ...


        /*
         *     A __________ B
         *     |            | 
         *     |            |
         *     D____________C
         *     
         */

        rayA.direction = new Vector2(horizontalSpeed, verticalSpeed);
        rayB.direction = new Vector2(horizontalSpeed, verticalSpeed);
        rayC.direction = new Vector2(horizontalSpeed, verticalSpeed);
        rayD.direction = new Vector2(horizontalSpeed, verticalSpeed);

        rayA.origin = new Vector2(-playerCollider.bounds.extents.x  + playerCollider.transform.position.x, playerCollider.bounds.extents.y + playerCollider.transform.position.y);
        rayB.origin = new Vector2(playerCollider.bounds.extents.x + playerCollider.transform.position.x, playerCollider.bounds.extents.y + playerCollider.transform.position.y);
        rayC.origin = new Vector2(playerCollider.bounds.extents.x + playerCollider.transform.position.x, -playerCollider.bounds.extents.y + playerCollider.transform.position.y );
        rayD.origin = new Vector2(-playerCollider.bounds.extents.x + playerCollider.transform.position.x, -playerCollider.bounds.extents.y + playerCollider.transform.position.y );
        
        RaycastHit2D castResultsA = Physics2D.Raycast(rayA.origin, rayA.direction, new Vector2(horizontalSpeed, verticalSpeed).magnitude, LayerMask.GetMask("Solid"));
        RaycastHit2D castResultsB = Physics2D.Raycast(rayB.origin, rayB.direction, new Vector2(horizontalSpeed, verticalSpeed).magnitude, LayerMask.GetMask("Solid"));
        RaycastHit2D castResultsC = Physics2D.Raycast(rayC.origin, rayC.direction, new Vector2(horizontalSpeed, verticalSpeed).magnitude, LayerMask.GetMask("Solid"));
        RaycastHit2D castResultsD = Physics2D.Raycast(rayD.origin, rayD.direction, new Vector2(horizontalSpeed, verticalSpeed).magnitude, LayerMask.GetMask("Solid"));

        Debug.DrawRay(rayA.origin, rayA.direction, Color.green);
        Debug.DrawRay(rayB.origin, rayB.direction, Color.green);
        Debug.DrawRay(rayC.origin, rayC.direction, Color.green);
        Debug.DrawRay(rayD.origin, rayD.direction, Color.green);


        Vector3 futurePosition = playerTransform.position + new Vector3(horizontalSpeed, verticalSpeed);

       


        // collide up
        if (verticalSpeed > 0)
        {
            if ( castResultsB.collider)
            {
                 if (castResultsB.collider.bounds.min.y + precision >= playerCollider.bounds.max.y && castResultsB.collider.gameObject.tag != "MovingPlatform")
                {
                    futurePosition = new Vector2(futurePosition.x, Mathf.Min(castResultsB.point.y - playerCollider.bounds.extents.y, futurePosition.y));
                speedManager(Direction.UP);
                }
            }
            if ( castResultsA.collider)
            {
                 if (castResultsA.collider.bounds.min.y + precision  >= playerCollider.bounds.max.y && castResultsA.collider.gameObject.tag != "MovingPlatform")
                {
                    futurePosition = new Vector2(futurePosition.x, Mathf.Min(castResultsA.point.y - playerCollider.bounds.extents.y, futurePosition.y));
                speedManager(Direction.UP);
                }
            }
        }

        // collide down
        if (verticalSpeed < 0) 
        {
            bool playerAlreadyOnBoard = false;
            if (castResultsC.collider)
            {
                if (castResultsC.collider.bounds.max.y - precision <= playerCollider.bounds.min.y)
                {
                    futurePosition = new Vector2(futurePosition.x, Mathf.Max(castResultsC.point.y + playerCollider.bounds.extents.y, futurePosition.y));
                    speedManager(Direction.DOWN);
                    if (castResultsC.collider.gameObject.tag == "MovingPlatform" && castResultsC.collider.gameObject.GetComponent<MovingPlatform>().curMove == MovingPlatform.MovementType.vertical && castResultsC.collider.gameObject.GetComponent<MovingPlatform>().startingDirection>0 && !playerAlreadyOnBoard)
                    {
                        Debug.Log("Player aboard");
                        float newSpeed = castResultsC.collider.gameObject.GetComponent<MovingPlatform>().speed * Time.deltaTime * castResultsC.collider.gameObject.GetComponent<MovingPlatform>().startingDirection;
                        verticalSpeed = newSpeed;
                        futurePosition += new Vector3(horizontalSpeed, verticalSpeed, 0);
                        playerAlreadyOnBoard = true;
                    }
                    if (castResultsC.collider.gameObject.tag == "MovingPlatform" && castResultsC.collider.gameObject.GetComponent<MovingPlatform>().curMove == MovingPlatform.MovementType.horizontal && !playerAlreadyOnBoard)
                    {
                        Debug.Log("Player aboard");
                        float newSpeed = castResultsC.collider.gameObject.GetComponent<MovingPlatform>().speed * Time.deltaTime * castResultsC.collider.gameObject.GetComponent<MovingPlatform>().startingDirection;
                        horizontalSpeed = newSpeed;
                        futurePosition += new Vector3(horizontalSpeed, verticalSpeed, 0);
                        playerAlreadyOnBoard = true;
                    }
                }
            }
            if (castResultsD.collider)
            {
                if (castResultsD.collider.bounds.max.y - precision <= playerCollider.bounds.min.y)
                {

                    futurePosition = new Vector2(futurePosition.x, Mathf.Max(castResultsD.point.y + playerCollider.bounds.extents.y, futurePosition.y));
                    speedManager(Direction.DOWN);
                    if (castResultsD.collider.gameObject.tag == "MovingPlatform" && castResultsD.collider.gameObject.GetComponent<MovingPlatform>().curMove == MovingPlatform.MovementType.vertical && castResultsD.collider.gameObject.GetComponent<MovingPlatform>().startingDirection > 0 && !playerAlreadyOnBoard)
                    {
                        Debug.Log("Player aboard");
                        float newSpeed = castResultsD.collider.gameObject.GetComponent<MovingPlatform>().speed * Time.deltaTime * castResultsD.collider.gameObject.GetComponent<MovingPlatform>().startingDirection;
                        verticalSpeed = newSpeed;
                        futurePosition += new Vector3(horizontalSpeed, verticalSpeed, 0);
                        playerAlreadyOnBoard = true;
                    }
                    if (castResultsD.collider.gameObject.tag == "MovingPlatform" && castResultsD.collider.gameObject.GetComponent<MovingPlatform>().curMove == MovingPlatform.MovementType.horizontal && !playerAlreadyOnBoard)
                    {
                        Debug.Log("Player aboard");
                        float newSpeed = castResultsD.collider.gameObject.GetComponent<MovingPlatform>().speed * Time.deltaTime * castResultsD.collider.gameObject.GetComponent<MovingPlatform>().startingDirection;
                        horizontalSpeed = newSpeed;
                        futurePosition += new Vector3(horizontalSpeed, verticalSpeed, 0);
                        playerAlreadyOnBoard = true;
                    }
                }
            }
        }



        //  collide left

        if (horizontalSpeed < 0)
        {
            if (castResultsA.collider)
            {
                if (castResultsA.collider.bounds.max.x - precision <= playerCollider.bounds.min.x && castResultsA.collider.gameObject.tag != "MovingPlatform")
                {
                    futurePosition = new Vector2(Mathf.Max(castResultsA.collider.bounds.max.x + playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);
                    speedManager(Direction.LEFT);
                }
            }
            if (castResultsD.collider)
            {
                if (castResultsD.collider.bounds.max.x - precision <= playerCollider.bounds.min.x && castResultsD.collider.gameObject.tag != "MovingPlatform")
                {
                    futurePosition = new Vector2(Mathf.Max(castResultsD.collider.bounds.max.x + playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);
                }
            }
        }


        // collide right

        if (horizontalSpeed > 0)
        {
            if (castResultsB.collider)
            {
                if (castResultsB.collider.bounds.min.x + precision >= playerCollider.bounds.max.x && castResultsB.collider.gameObject.tag != "MovingPlatform") // verifier que on n'est pas juste posés sur le plafond
                    futurePosition = new Vector2(Mathf.Min(castResultsB.point.x - playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);


                speedManager(Direction.RIGHT);
            }
            if (castResultsC.collider)
            {
                if (castResultsC.collider.bounds.min.x + precision >= playerCollider.bounds.max.x && castResultsC.collider.gameObject.tag != "MovingPlatform") // verifier que on n'est pas juste posés sur le sol
                    futurePosition = new Vector2(Mathf.Min(castResultsC.point.x - playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);
            }
        }

        playerTransform.position = futurePosition;

    }

    private void speedManager(Direction collideDirection)
    {
        switch(collideDirection)
        {
            case Direction.UP:
                Debug.Log("up");
                verticalSpeed = 0;
                GetComponent<Jump>().enabled = true;
                GetComponent<WallJump>().enabled = false;
                break;
            case Direction.LEFT:
                Debug.Log("left");
                GetComponent<Jump>().enabled = false;
                GetComponent<WallJump>().enabled = true;
                GetComponent<WallJump>().JumpDirection = WallJump.Direction.RIGHT;
                GetComponent<WallJump>().IsWalljumping = false;
                break;
            case Direction.RIGHT:
                Debug.Log("right");
                GetComponent<Jump>().enabled = false;
                GetComponent<WallJump>().enabled = true;
                GetComponent<WallJump>().JumpDirection = WallJump.Direction.LEFT;
                GetComponent<WallJump>().IsWalljumping = false;
                break;
            case Direction.DOWN:
                Debug.Log("down");
                verticalSpeed = 0;
                GetComponent<Jump>().enabled = true;
                GetComponent<WallJump>().enabled = false;
                break;

        }
    }
    

    private void LateUpdate()
    {
        CollisionManager();
    }
}
