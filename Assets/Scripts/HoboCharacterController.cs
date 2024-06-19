using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoboCharacterController : MonoBehaviour
{
    [SerializeField]
    public Animator animator;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField] 
    public GameObject characterController;

    [SerializeField]
    private bool ableToJump = false;
    [SerializeField]
    private bool isWalking = false;
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool isOnGround = false;
    [SerializeField]
    private bool isFalling = false;
    [SerializeField]
    private bool isInteracting = false;
    [SerializeField]
    private bool isAttacking = false;

    // player
    private float jumpForce = 7f;
    private float speed;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private AnimatorStateInfo stateInfo;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 0.5f;
    private float gravityValue = -9.81f;

    private void Awake()
    {
        //gameObject.transform.position = new Vector3(0, 5, 0);
        FindComponents();
    }

    public void FindComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        if (rigidbody == null)
        {
            Debug.LogWarning("rigidbody NOT FOUND!");
            return;
        }
        UpdateAnimationAction();
    }

    private void Update()
    {
        if (transform.position.y < -5)
        {
            Respawn();
        }

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = stateInfo.IsName("Attack");
        bool isInteracting = stateInfo.IsName("Interact");
        //Left Click: Interact
        if (Input.GetMouseButtonDown(0) && !isInteracting && !isAttacking)
        {
            animator.SetTrigger("isInteracting");
        }
        //Right Click: Attack
        else if (Input.GetMouseButtonDown(1) && !isInteracting && !isAttacking)
        {
            animator.SetTrigger("isAttacking");
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if (direction != Vector3.zero && !isInteracting && !isAttacking)
        {
            isWalking = true;
            animator.SetBool("isWalking", true);
            // Move player
            controller.Move(moveDir * Time.deltaTime * playerSpeed);
            // Rotate player to face direction
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
            isWalking = false;
            animator.SetBool("isWalking", false);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer && ableToJump)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            isJumping = true;
            animator.SetBool("isJumping", true);
            isOnGround = false;
            animator.SetBool("isOnGround", false);
            isFalling = false;
            animator.SetBool("isFalling", false);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (!controller.isGrounded)
        {
            isOnGround = false;
            UpdateAnimationAction();
        }

        if (transform.position.y <= -10)
        {
            Respawn();
        }
    }
    void FixedUpdate()
    {

    }

    private void UpdateAnimationAction()
    {
        //if (!isOnGround && !isFalling && !isJumping)
        //{
        //    isFalling = true;
        //    animator.SetBool("isFalling", true);
        //}

        if (isOnGround && !isFalling)
        {
            //Landing
            //Handled by OnControllerColliderHit
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Landing
        if (!isOnGround && hit.transform.CompareTag("Ground"))
        {
            isOnGround = true;
            animator.SetBool("isOnGround", true);
            isFalling = false;
            animator.SetBool("isFalling", false);
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    public bool GetAbleToJump()
    {
        return ableToJump;
    }

    public void SetAbleToJump(bool enableDisableJump)
    {
        ableToJump = enableDisableJump;
    }

    public void RestarLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Respawn()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            gameObject.transform.position = new Vector3(-1.644f, 5.0f, -107.121f);
        }
        else
        {
            gameObject.transform.position = new Vector3(0, 5, 0);

        }
    }
}