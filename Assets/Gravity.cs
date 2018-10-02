using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    private Collider2D playerCollider;

    public float verticalSpeed = 0;

    private Ray2D verticalRay;


    // Use this for initialization
    void Start () {
        playerCollider = GetComponent<Collider2D>();
        verticalRay = new Ray2D();
	}
	
	// Update is called once per frame
	void Update () {

        verticalRay.direction = new Vector2(-1, 0);
        verticalRay.origin = playerCollider.bounds.min + new Vector3(-1, 0, 0);

        RaycastHit2D hit = Physics2D.Raycast(verticalRay.origin, verticalRay.direction, verticalSpeed +1f);
        
        if (hit.transform)
        {
            Debug.Log(hit.collider);
        }
        else
        {
            Debug.Log("Raycast hit nothing");
        }

	}
}
