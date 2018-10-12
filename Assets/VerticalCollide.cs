using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VerticalCollide : MonoBehaviour {

    private Collider2D playerCollider;

    private Transform playerTrasform;

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
        playerTrasform = GetComponent<Transform>();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(rayA.origin, rayA.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayA.origin);
        Gizmos.DrawLine(rayB.origin, rayB.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayB.origin);
        Gizmos.DrawLine(rayC.origin, rayC.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayC.origin);
        Gizmos.DrawLine(rayD.origin, rayD.direction * (new Vector2(verticalSpeed, horizontalSpeed).magnitude) + rayD.origin);



    }


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
        

        Vector2 futurePosition = playerTrasform.position + new Vector3(horizontalSpeed, verticalSpeed);

        //  collide left

        if (horizontalSpeed <= 0)
        {
            if ( castResultsA.collider )
            {
                 if (castResultsA.collider.bounds.max.x - 0.01f <= playerCollider.bounds.min.x)
                {
                    futurePosition = new Vector2(Mathf.Max(castResultsA.point.x + playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);
                }
                speedManager(Direction.LEFT);
            }
            if ( castResultsD.collider )
            {
                if (castResultsD.collider.bounds.max.x - 0.01f <= playerCollider.bounds.min.x)
                {
                    futurePosition = new Vector2(Mathf.Max(castResultsD.point.x + playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);
                }
                speedManager(Direction.LEFT);
            }
        }
        

        // collide right
      
        if (horizontalSpeed > 0)
        {
            if (castResultsB.collider)
            {
                if (castResultsB.collider.bounds.min.x +0.01f > playerCollider.bounds.max.x) // verifier que on n'est pas juste posés sur le plafond
                    futurePosition = new Vector2(Mathf.Min(castResultsB.point.x - playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);
                
               
                speedManager(Direction.RIGHT);
            }
            if (castResultsC.collider)
            {
                if (castResultsC.collider.bounds.min.x + 0.01f  > playerCollider.bounds.max.x) // verifier que on n'est pas juste posés sur le sol
                    futurePosition = new Vector2(Mathf.Min(castResultsC.point.x - playerCollider.bounds.extents.x, futurePosition.x), futurePosition.y);
                speedManager(Direction.RIGHT);
            }
        }


        // collide up
        if (verticalSpeed >= 0)
        {
            if ( castResultsB.collider)
            {
                 if (castResultsB.collider.bounds.min.y + 0.01f >= playerCollider.bounds.max.y)
                {
                    futurePosition = new Vector2(futurePosition.x, Mathf.Min(castResultsB.point.y - playerCollider.bounds.extents.y, futurePosition.y));
                }
                speedManager(Direction.UP);
            }
            if ( castResultsA.collider)
            {
                 if (castResultsA.collider.bounds.min.y + 0.01f >= playerCollider.bounds.max.y)
                {
                    futurePosition = new Vector2(futurePosition.x, Mathf.Min(castResultsA.point.y - playerCollider.bounds.extents.y, futurePosition.y));
                }
                speedManager(Direction.UP);
            }
        }

        // collide down
        if (verticalSpeed <= 0) //else
        {
            if (castResultsC.collider)
            {
                 if(castResultsC.collider.bounds.max.y - 0.01f <= playerCollider.bounds.min.y)
                {
                    futurePosition = new Vector2(futurePosition.x, Mathf.Max(castResultsC.point.y + playerCollider.bounds.extents.y, futurePosition.y));
                }
                speedManager(Direction.DOWN);
            }
            if ( castResultsD.collider)
            {
                 if (castResultsC.collider.bounds.max.y - 0.01f <= playerCollider.bounds.min.y)
                {

                    futurePosition = new Vector2(futurePosition.x, Mathf.Max(castResultsC.point.y + playerCollider.bounds.extents.y, futurePosition.y));
                }
                speedManager(Direction.DOWN);
            }
        }


        playerTrasform.position = futurePosition;

    }

    private void speedManager(Direction collideDirection)
    {
        switch(collideDirection)
        {
            case Direction.UP:
                Debug.Log("up");
                verticalSpeed = 0;
                break;
            case Direction.LEFT:
                Debug.Log("left");
                break;
            case Direction.RIGHT:
                Debug.Log("right");
                break;
            case Direction.DOWN:
                Debug.Log("down");
                verticalSpeed = 0;
                break;
        }
    }
    

    private void LateUpdate()
    {
        CollisionManager();
    }
}
