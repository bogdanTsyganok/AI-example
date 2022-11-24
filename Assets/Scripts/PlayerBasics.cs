using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasics : MonoBehaviour
{
    Vector3 spawnPosition;
    // Update is called once per frame
    private Rigidbody rb;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = movement * speed;
        rb.AddForce(movement);
    }
}
