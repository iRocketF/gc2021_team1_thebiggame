using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController pController;
    public PlayerHealth pHealth;
    public Animator pAnimator;
    public BoxCollider hitZone;
    public Camera pCamera;

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
    private bool isDead;

    // dash stuff
    public float dashCooldown; // how long until the player can dash again
    private float dashTimer; // counts time from last dash
    public float dashSpeed; // how fast the dash moves
    public float maxDashLength; // absolute max length of the dash
    private float dashRatio; // used for lerp
    private Vector3 ogPosition; // where the dash starts
    private Vector3 dashDirection; // direction of the dash
    private Vector3 dashPosition; // where the dash finishes
    public bool isDashing;
    public bool hasDashed;

    void Awake()
    {
        transform.position = FindObjectOfType<StartingPosition>().GetPosition();
    }

    void Start()
    {
        pHealth = GetComponent<PlayerHealth>();
        pAnimator = GetComponentInChildren<Animator>();
        pController = GetComponent<CharacterController>();
        hitZone = GetComponentInChildren<BoxCollider>();
        pCamera = FindObjectOfType<Camera>();

        hitZone.enabled = false;

        forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void Update()
    {
        if (pHealth.currentHealth <= 0f)
            isDead = true;

        if(!isImmobile && !isDead)
            Move();

        if (Input.GetButtonDown("Fire1") && !isDashing && !isDead)
            Attack();

        if (Input.GetButtonDown("Fire2") && !hasDashed && !isDead)
            Dash();
        else if (isDashing)
            DashLerp();

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

        Ray ray = pCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var target = hitInfo.point;
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
            lastRotation = transform.rotation;
        }

        hitZone.enabled = true;

    }

    void Dash()
    {
        pAnimator.SetTrigger("Dash");

        ogPosition = transform.position;
        dashDirection = transform.forward;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        float dashLength = maxDashLength;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, dashDirection, out hit, dashLength))
            dashLength = hit.distance;
        else
            dashLength = maxDashLength;

        dashPosition = transform.position + (dashDirection * dashLength);

        if (dashPosition != ogPosition)
            isDashing = true;
    }

    void DashLerp()
    {
        dashRatio = dashRatio + (Time.deltaTime * dashSpeed);

        transform.position = Vector3.Lerp(ogPosition, dashPosition, dashRatio);

        if (dashRatio >= 1f)
        {
            isDashing = false;
            hasDashed = true;
            dashRatio = 0f;
        }
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

        if (hasDashed)
        {
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashCooldown)
            {
                hasDashed = false;
                dashTimer = 0f;
            }
        }
    }
}
