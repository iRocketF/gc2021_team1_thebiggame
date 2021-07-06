using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController pController;

    public float moveSpeed;
    public float gravity;

    Vector3 forward, right;

    private Vector3 velocity;

    void Start()
    {
        pController = GetComponent<CharacterController>();

        forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (pController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float horizontal = Input.GetAxis("Iso_Horizontal");
        float vertical = Input.GetAxis("Iso_Vertical");

        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * horizontal;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * vertical;

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement) * moveSpeed * Time.deltaTime;

        // transform.forward = heading;
        // transform.position += rightMovement;
        // transform.position += upMovement;

        velocity.y += gravity * Time.deltaTime;

        pController.Move(heading + (velocity * Time.deltaTime));


    }
}
