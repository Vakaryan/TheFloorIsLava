using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCollide : MonoBehaviour {

    private Collider2D playerCollider;

    private Transform playerTrasform;

    public float verticalSpeed ;
    public float VerticalSpeed {
        get { return verticalSpeed; }
        set { verticalSpeed = value; } }


    private Ray2D verticalRay;



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
        
        if(verticalSpeed <= 0)
        {
            verticalRay.origin = playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y) + new Vector3(0, -0.01f, 0);
            verticalRay.direction = new Vector2(0, -1);
        }
        else
        {
            verticalRay.origin = playerCollider.bounds.center + new Vector3(0, playerCollider.bounds.extents.y) + new Vector3(0, +0.01f, 0);
            verticalRay.direction = new Vector2(0,1);
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

            Debug.Log(verticalSpeed);

        }
    }
}
