using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float range;
    public enum MovementType { horizontal, vertical };
    public MovementType curMove;
    private Vector2 startingPosition;
    public float startingDirection;
    public float speed;



    // Use this for initialization
    void Start()
    {
        startingPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        switch (curMove)
        {
            case MovementType.horizontal:
                transform.position += new Vector3(speed * startingDirection * Time.deltaTime, 0f, 0f);
                if (startingDirection == 1 && transform.position.x >= startingPosition.x + range)
                {
                    startingDirection *= -1;
                    startingPosition.x += range;
                }
                if (startingDirection == -1 && transform.position.x <= startingPosition.x - range)
                {
                    startingDirection *= -1;
                    startingPosition.x -= range;
                }
                break;

            case MovementType.vertical:
                transform.position += new Vector3(0f, speed * startingDirection * Time.deltaTime, 0f);
                if (startingDirection == 1 && transform.position.y >= startingPosition.y + range)
                {
                    startingDirection *= -1;
                    startingPosition.y += range;
                }
                if (startingDirection == -1 && transform.position.y <= startingPosition.y - range)
                {
                    startingDirection *= -1;
                    startingPosition.y -= range;
                }
                break;
        }
    }

}
