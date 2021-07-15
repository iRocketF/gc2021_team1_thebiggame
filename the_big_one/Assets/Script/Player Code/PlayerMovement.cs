using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController pController;
    public Animator pAnimator;
    public BoxCollider hitZone;
    public Camera camera;

    public float moveSpeed;
    public float gravity;
    public float rotSmooth;

    public float hitZoneTime;
    private float hitZoneTimer;

    public float immobilityTime;
    private float immobilityTimer;

    Vector3 forward, right;

    private Vector3 velocity;
    private Quaternion lastRotation;

    private bool isImmobile;

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
        if(!isImmobile)
            Move();

        if (Input.GetButtonDown("Fire1"))
            Attack();

        Timers();

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

        if (heading != Vector3.zero)
        {
            var newRotation = Quaternion.LookRotation(heading);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotSmooth);
        }
            


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
        isImmobile = true;
        immobilityTimer = 0f;

        pAnimator.SetTrigger("Strike");

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var target = hitInfo.point;
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
            lastRotation = transform.rotation;
        }

        hitZone.enabled = true;

    }

    void Timers()
    {
        if (hitZone.enabled && hitZoneTimer < hitZoneTime)
        {
            hitZoneTimer += Time.deltaTime;
        }

        else if (hitZoneTimer >= hitZoneTime)
        {
            hitZoneTimer = 0f;
            hitZone.enabled = false;
        }

        if(isImmobile)
        {
            immobilityTimer += Time.deltaTime;

            if (immobilityTimer >= immobilityTime)
            {
                isImmobile = false;
                immobilityTimer = 0f;
            }
        }
    }
}
