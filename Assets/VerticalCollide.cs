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

        rayA.origin = new Vector2(-playerCollider.bounds.extents.x  + playerCollider.transform.position.x, -playerCollider.bounds.extents.y + playerCollider.transform.position.y);
        rayB.origin = new Vector2(playerCollider.bounds.extents.x + playerCollider.transform.position.x, -playerCollider.bounds.extents.y + playerCollider.transform.position.y);
        rayC.origin = new Vector2(playerCollider.bounds.extents.x + playerCollider.transform.position.x, playerCollider.bounds.extents.y + playerCollider.transform.position.y);
        rayD.origin = new Vector2(-playerCollider.bounds.extents.x + playerCollider.transform.position.x, playerCollider.bounds.extents.y + playerCollider.transform.position.y);

        //ContactFilter2D filter = new ContactFilter2D();
        

        RaycastHit2D castResultsA = Physics2D.Raycast(rayA.origin, rayA.direction, new Vector2(verticalSpeed, horizontalSpeed).magnitude, LayerMask.GetMask("Solid"));
        RaycastHit2D castResultsB = Physics2D.Raycast(rayB.origin, rayB.direction, new Vector2(verticalSpeed, horizontalSpeed).magnitude, LayerMask.GetMask("Solid"));
        RaycastHit2D castResultsC = Physics2D.Raycast(rayC.origin, rayC.direction, new Vector2(verticalSpeed, horizontalSpeed).magnitude, LayerMask.GetMask("Solid"));
        RaycastHit2D castResultsD = Physics2D.Raycast(rayD.origin, rayD.direction, new Vector2(verticalSpeed, horizontalSpeed).magnitude, LayerMask.GetMask("Solid"));
        


        playerTrasform.position += new Vector3(horizontalSpeed, verticalSpeed);

        //  collide left
        if (horizontalSpeed < 0 && castResultsA.collider )
        {
            if(castResultsA.distance != 0 || castResultsA.collider.bounds.min.y < playerCollider.bounds.max.y) // verifier que on n'est pas juste posés sur le plafond
                playerTrasform.position = new Vector2(Mathf.Max(castResultsA.point.x, playerTrasform.position.x), playerTrasform.position.y);
            speedManager(Direction.LEFT);
        }
        if (horizontalSpeed < 0 && castResultsD.collider )
        {
            if (castResultsD.distance != 0 || castResultsD.collider.bounds.max.y > playerCollider.bounds.min.y) // verifier que on n'est pas juste posés sur le sol
                playerTrasform.position = new Vector2(Mathf.Max(castResultsD.point.x, playerTrasform.position.x), playerTrasform.position.y);
            speedManager(Direction.LEFT);
        }

        // collide right
        if (horizontalSpeed > 0 && castResultsB.collider)
        {
            if (castResultsB.distance != 0 || castResultsB.collider.bounds.min.y < playerCollider.bounds.max.y) // verifier que on n'est pas juste posés sur le plafond
                playerTrasform.position = new Vector2(Mathf.Min(castResultsB.point.x, playerTrasform.position.x), playerTrasform.position.y);
            speedManager(Direction.RIGHT);
        }
        if (horizontalSpeed < 0 && castResultsC.collider)
        {
            if (castResultsC.distance != 0 || castResultsC.collider.bounds.max.y > playerCollider.bounds.min.y) // verifier que on n'est pas juste posés sur le sol
                playerTrasform.position = new Vector2(Mathf.Min(castResultsC.point.x, playerTrasform.position.x), playerTrasform.position.y);
            speedManager(Direction.RIGHT);
        }


        // collide up
        if (verticalSpeed > 0 && castResultsB.collider)
        {
            if (castResultsB.distance != 0 || castResultsB.collider.bounds.min.x < playerCollider.bounds.max.x) // verifier que on n'est pas juste posés sur le coté
                playerTrasform.position = new Vector2( playerTrasform.position.x, Mathf.Min(castResultsB.point.y, playerTrasform.position.y));
            speedManager(Direction.UP);
        }
        if (verticalSpeed > 0 && castResultsA.collider)
        {
            if (castResultsA.distance != 0 || castResultsA.collider.bounds.max.x > playerCollider.bounds.min.x) // verifier que on n'est pas juste posés sur le coté
                playerTrasform.position = new Vector2(playerTrasform.position.x, Mathf.Min(castResultsA.point.y, playerTrasform.position.y));
            speedManager(Direction.UP);
        }

        // collide down
        if (verticalSpeed < 0 && castResultsC.collider)
        {
            if (castResultsC.distance != 0 || castResultsC.collider.bounds.min.x < playerCollider.bounds.max.x) // verifier que on n'est pas juste posés sur le coté
                playerTrasform.position = new Vector2(playerTrasform.position.x, Mathf.Max(castResultsC.point.y, playerTrasform.position.y));
            speedManager(Direction.DOWN);
        }
        if (verticalSpeed < 0 && castResultsD.collider)
        {

            Debug.Log("down");
            if (castResultsD.distance != 0 || castResultsD.collider.bounds.max.x < playerCollider.bounds.min.x) // verifier que on n'est pas juste posés sur le coté
                playerTrasform.position = new Vector2(playerTrasform.position.x, Mathf.Max(castResultsD.point.y, playerTrasform.position.y));
            speedManager(Direction.DOWN);
        }

        
        else
        {
            playerTrasform.position += new Vector3(horizontalSpeed, verticalSpeed);
        }
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
                horizontalSpeed = 0;
                break;
            case Direction.RIGHT:
                Debug.Log("right");
                horizontalSpeed = 0;
                break;
            case Direction.DOWN:
                verticalSpeed = 0;
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        
        

    }

    private void LateUpdate()
    {
        CollisionManager();
    }
}
