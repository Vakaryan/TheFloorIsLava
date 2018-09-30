using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGroundIsLava : MonoBehaviour {
    public GameObject startingPoint;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.transform.position = startingPoint.transform.position;
        }
    }
}
