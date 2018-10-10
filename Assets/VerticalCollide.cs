using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCollide : MonoBehaviour {

    private Collider2D playerCollider;

    private Transform playerTrasform;

    private float verticalSpeed  = 0;
    public float VerticalSpeed {
        get { return verticalSpeed; }
        set { verticalSpeed = value; } }

    private float horizontalSpeed = 0;
    public float HorizontalSpeed
    {
        get { return horizontalSpeed; }
        set { horizontalSpeed = value; }
    }

    private Ray2D verticalRay;

    public float precision;



    // Use this for initialization
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerTrasform = GetComponent<Transform>();
        verticalRay = new Ray2D();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(verticalRay.origin, verticalRay.direction * (Mathf.Abs(verticalSpeed) * 100 +1) + verticalRay.origin);

        Gizmos.DrawCube(playerCollider.bounds.center, playerCollider.bounds.extents);


    }

    // Update is called once per frame
    void Update () {

        verticalRay.direction = new Vector2(horizontalSpeed, verticalSpeed);
        if (verticalSpeed <= 0)
        {
            verticalRay.origin = playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y) + new Vector3(0, -precision, 0);
        }
        else
        {
            verticalRay.origin = playerCollider.bounds.center + new Vector3(0, playerCollider.bounds.extents.y) + new Vector3(0, precision, 0);
        }

        RaycastHit2D hit = Physics2D.Raycast(verticalRay.origin, verticalRay.direction, Mathf.Abs( verticalSpeed));

        if (hit.transform)
        {
            Debug.Log(hit.collider);
            playerTrasform.position = new Vector3(playerTrasform.position.x, hit.point.y + playerCollider.bounds.extents.y);
            verticalSpeed = 0;
        }
        else
        {
            Debug.Log("Raycast hit nothing");
            playerTrasform.position += new Vector3(0, verticalSpeed);
            

        }

        // Rework des collisions :

        // Todo : 4 raycast

        // Collision :
        //      - la plus proche -> déplacement au contact de la BondingBox !  => devrait fonctionner !
        // Sinon : va !

        // Problèmes : [] ---  avec déplacement horizontal -> pas de collision
        // Est-ce un problème ? (cf jeux de plateforme classiques.)

        // Attention : ray du bas / du haut ...


    }
}
