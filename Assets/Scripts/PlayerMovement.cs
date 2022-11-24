using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12.0f;

    public float gravity = -9.81f;
    public Vector3 velocity;

    public float jumpHeight = 3.0f;

    private Vector3 startingPosition;
    private bool needToRespawn = false;

    private void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Incorrect movement:
        //Vector3 move = new Vector3(x, 0.0f, z);

        Vector3 move = (transform.right * x) + (transform.forward * z);
        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocity.y = 0.0f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Space bar is pressed");
            velocity.y = Mathf.Sqrt(-2.0f * jumpHeight * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(transform.position.y < -10.0f)
        {
            needToRespawn = true;
        }

        if(needToRespawn)
        {
            ResetPosition();
            needToRespawn = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        needToRespawn = true;
    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
    }
}
