using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController pController;
    public Animator pAnimator;
    public BoxCollider hitZone;

    public float moveSpeed;
    public float gravity;

    public float hitZoneTime;
    public float timer;

    Vector3 forward, right;

    private Vector3 velocity;
    private Quaternion lastRotation;

    void Start()
    {
        pAnimator = GetComponentInChildren<Animator>();
        pController = GetComponent<CharacterController>();
        hitZone = GetComponentInChildren<BoxCollider>();

        hitZone.enabled = false;

        forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        transform.position = FindObjectOfType<StartingPosition>().GetPosition();
    }

    void Update()
    {
        Move();

        if (Input.GetButtonDown("Fire1"))
            Attack();

        if (hitZone.enabled && timer < hitZoneTime)
            timer += Time.deltaTime;
        else if (timer >= hitZoneTime)
        {
            timer = 0f;
            hitZone.enabled = false;
        }

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

        velocity.y += gravity * Time.deltaTime;

        if(heading != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(heading);

        if (horizontal !=0 || vertical != 0)
        {
            lastRotation = transform.rotation;
            pAnimator.SetBool("isMoving", true);
        } else
        {
            pAnimator.SetBool("isMoving", false);
        }

        pController.Move(heading + (velocity * Time.deltaTime));

        transform.rotation = lastRotation;

    }
    
    void Attack()
    {
        pAnimator.SetTrigger("Strike");

        hitZone.enabled = true;

    }
}
