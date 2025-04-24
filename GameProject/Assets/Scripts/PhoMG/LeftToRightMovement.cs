using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftToRightMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody2DBeingMoved;
    public float timer, timeBetweenSwitches;
    public bool moveRight;
    public float speed;

    private void Start()
    {
        timer = timeBetweenSwitches;

    }

    private void Update()
    {
        timer -= Time.deltaTime;

    }

    void FixedUpdate()
    {
        if (moveRight == true)
        {
            if (timer > 0)
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);

            }
            else
            {

                moveRight = false;
                timer = timeBetweenSwitches; 

            }
        }//End of if moveRight = true

        if (moveRight == false)
        {

            if (timer > 0)
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);

            }
            else
            {

                moveRight = true;
                timer = timeBetweenSwitches;
            }
        }
        
    }
}
