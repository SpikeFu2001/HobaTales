using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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

    private Vector2 _input;

    // player
    private float jumpForce = 7f;
    private float speed;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private Dictionary<string, int> actions = new Dictionary<string, int>();

    [HideInInspector]
    public int actionID = 0;

    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool isGrounded = false;
    [SerializeField]
    private bool isFalling = false;

    private bool animationHasLoop = false;
    private bool actionNoLoopedReturnToIdle = true;
    private AnimatorClipInfo[] animatorInfo;
    private AnimationClip currentAnimationClip;
    private string currentAnimation;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    [Header("Actions Names")]
    private string A_001_T_POSE = "001_pose_1_TPose";
    private string A_002_CROUCH = "002_pose_crouch";
    private string A_003_SPIN_1 = "003_pose_spin_1";
    private string A_004_SPIN_2 = "004_pose_spin_2";
    private string A_011_IDLE_STAND = "011_idle_1_stand";
    private string A_012_IDLE_CROUCH = "012_idle_2_crouch";
    private string A_021_WALK_FORWARD = "021_walk_forward";
    private string A_022_WALK_BACKWARD = "022_walk_backward";
    private string A_023_WALK_RIGHT = "023_walk_right";
    private string A_024_WALK_LEFT = "024_walk_left";
    private string A_031_RUN_FORWARD = "031_run_forward";
    private string A_041_JUMP_1 = "041_jump_1";
    private string A_051_JUMP_SPIN_FORWARD = "051_jump_spin_forward";
    private string A_052_JUMP_SPIN_BACKFLIP = "052_jump_spin_backflip";
    private string A_061_FALL_1 = "061_fall_1";
    private string A_071_LAND_1 = "071_land_1";
    private string A_081_PUNCH_1 = "081_punch_1";
    private string A_091_KICK_1 = "091_kick_1";
    private string A_101_ATTACK_1 = "101_attack_1";
    private string A_111_MAGIC_1 = "111_magic_1";
    private string A_121_BLOCK_1 = "121_block_1";
    private string A_131_HIT_1 = "131_hit_1";
    private string A_141_LOSE_1 = "141_lose_1";
    private string A_151_DIE_1 = "151_die_1";
    private string A_161_VICTORY_1 = "161_victory_1";
    private string A_162_VICTORY_2 = "162_victory_2";
    private string A_163_VICTORY_3 = "163_victory_3";
    private string A_164_VICTORY_4 = "164_victory_4";
    [Header("Actions ID")]
    private int A_001_T_POSE_ID = 1;
    private int A_002_CROUCH_ID = 2;
    private int A_003_SPIN_1_ID = 3;
    private int A_004_SPIN_2_ID = 4;
    private int A_011_IDLE_STAND_ID = 11;
    private int A_012_IDLE_CROUCH_ID = 12;
    private int A_021_WALK_FORWARD_ID = 21;
    private int A_022_WALK_BACKWARD_ID = 22;
    private int A_023_WALK_RIGHT_ID = 23;
    private int A_024_WALK_LEFT_ID = 24;
    private int A_031_RUN_FORWARD_ID = 31;
    private int A_041_JUMP_1_ID = 41;
    private int A_051_JUMP_SPIN_FORWARD_ID = 51;
    private int A_052_JUMP_SPIN_BACKFLIP_ID = 52;
    private int A_061_FALL_1_ID = 61;
    private int A_071_LAND_1_ID = 71;
    private int A_081_PUNCH_1_ID = 81;
    private int A_091_KICK_1_ID = 91;
    private int A_101_ATTACK_1_ID = 101;
    private int A_111_MAGIC_1_ID = 111;
    private int A_121_BLOCK_1_ID = 121;
    private int A_131_HIT_1_ID = 131;
    private int A_141_LOSE_1_ID = 141;
    private int A_151_DIE_1_ID = 151;
    private int A_161_VICTORY_1_ID = 161;
    private int A_162_VICTORY_2_ID = 162;
    private int A_163_VICTORY_3_ID = 163;
    private int A_164_VICTORY_4_ID = 164;

    private string backActionName = "011_idle_1_stand";
    private int backActionID = 11;

    private void Awake()
    {
        //gameObject.transform.position = new Vector3(0, 5, 0);
        FindComponents();
        actions[A_001_T_POSE] = A_001_T_POSE_ID;
        actions[A_002_CROUCH] = A_002_CROUCH_ID;
        actions[A_003_SPIN_1] = A_003_SPIN_1_ID;
        actions[A_004_SPIN_2] = A_004_SPIN_2_ID;
        actions[A_011_IDLE_STAND] = A_011_IDLE_STAND_ID;
        actions[A_012_IDLE_CROUCH] = A_012_IDLE_CROUCH_ID;
        actions[A_021_WALK_FORWARD] = A_021_WALK_FORWARD_ID;
        actions[A_022_WALK_BACKWARD] = A_022_WALK_BACKWARD_ID;
        actions[A_023_WALK_RIGHT] = A_023_WALK_RIGHT_ID;
        actions[A_024_WALK_LEFT] = A_024_WALK_LEFT_ID;
        actions[A_031_RUN_FORWARD] = A_031_RUN_FORWARD_ID;
        actions[A_041_JUMP_1] = A_041_JUMP_1_ID;
        actions[A_051_JUMP_SPIN_FORWARD] = A_051_JUMP_SPIN_FORWARD_ID;
        actions[A_052_JUMP_SPIN_BACKFLIP] = A_052_JUMP_SPIN_BACKFLIP_ID;
        actions[A_061_FALL_1] = A_061_FALL_1_ID;
        actions[A_071_LAND_1] = A_071_LAND_1_ID;
        actions[A_081_PUNCH_1] = A_081_PUNCH_1_ID;
        actions[A_091_KICK_1] = A_091_KICK_1_ID;
        actions[A_101_ATTACK_1] = A_101_ATTACK_1_ID;
        actions[A_111_MAGIC_1] = A_111_MAGIC_1_ID;
        actions[A_121_BLOCK_1] = A_121_BLOCK_1_ID;
        actions[A_131_HIT_1] = A_131_HIT_1_ID;
        actions[A_141_LOSE_1] = A_141_LOSE_1_ID;
        actions[A_151_DIE_1] = A_151_DIE_1_ID;
        actions[A_161_VICTORY_1] = A_161_VICTORY_1_ID;
        actions[A_162_VICTORY_2] = A_162_VICTORY_2_ID;
        actions[A_163_VICTORY_3] = A_163_VICTORY_3_ID;
        actions[A_164_VICTORY_4] = A_164_VICTORY_4_ID;

        backActionName = A_011_IDLE_STAND;
        backActionID = A_011_IDLE_STAND_ID;
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

        if (direction != Vector3.zero)
        {
            // Move player
            controller.Move(moveDir * Time.deltaTime * playerSpeed);
            // Rotate player to face direction
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (transform.position.y <= -100)
        {
            Respawn();

        }
    }

        void FixedUpdate()
    {
    }

    public void ActionNoLoopedReturnToIdle(bool value)
    {
        actionNoLoopedReturnToIdle = value;
    }

    public void SetActionInt(int _actionID = -1)
    {
        ActionNoLoopedReturnToIdle(true);
        StopCoroutine("ReturnToActionCoroutine");
        actionID = _actionID;
        animator.SetInteger("actionID", actionID);
        StopAllCoroutines();
        try
        {
            float waitTime = 1.0f;
            if (animator != null)
            {
                animatorInfo = this.animator.GetCurrentAnimatorClipInfo(0);
                for (int i = 0; i < animatorInfo.Length; i++)
                {
                }
                if (animatorInfo.Length > 0)
                {
                    currentAnimationClip = animatorInfo[0].clip;
                    if (currentAnimationClip != null)
                    {
                    }
                    currentAnimation = currentAnimationClip.name;
                    float clipDuration = currentAnimationClip.length;
                    float animatorSpeed = this.animator.speed;
                    waitTime = clipDuration / animatorSpeed;
                }
            }
            StartCoroutine("WaitToActionCoroutine", waitTime);
        }
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
        finally
        {
        }
        //UpdateAnimationAction();
    }

    public void SetActionName(string _actionName = "011_idle_stand")
    {
        StopCoroutine("ReturnToActionCoroutine");
        actionID = (int)actions[_actionName];
        animator.SetInteger("actionID", actionID);
        //UpdateAnimationAction();
    }

    public void SetAnimatorSpeed(float _speed = 1)
    {
        animator.speed = _speed;
    }

    private void UpdateAnimationAction()
    {
        //Falling
        if (!isGrounded && !isFalling)
        {
            isFalling = true;
            SetActionInt(A_061_FALL_1_ID);
        }

        if(isGrounded && !isFalling)
        {
            //Landing
            //Handled by OnControllerColliderHit
        }
    }

    private void ReturnToAction(string _actionName = "011_idle_1_stand", float _returnTime = 2.0f)
    {
        backActionName = _actionName;
        backActionID = (int)actions[_actionName];
        StopCoroutine("ReturnToActionCoroutine");
        StartCoroutine("ReturnToActionCoroutine", _returnTime);
    }

    private IEnumerator ReturnToActionCoroutine(float _returnTime = 3.0f)
    {
        yield return new WaitForSeconds(_returnTime);
        if (actionNoLoopedReturnToIdle == true)
        {
            if (backActionID != -1)
            {
                SetActionInt(backActionID);
            }
            else if (backActionName != "")
            {
                SetActionName(backActionName);
            }
        }
    }

    private IEnumerator WaitToActionCoroutine(float _returnTime = 2.0f)
    {
        yield return new WaitForSeconds(_returnTime);
        try
        {
            bool hasLoop = true;
            float clipDuration = 1;
            float animatorSpeed = 1;
            if (animator != null)
            {
                animatorInfo = this.animator.GetCurrentAnimatorClipInfo(0);
                for (int i = 0; i < animatorInfo.Length; i++)
                {
                }
                if (animatorInfo.Length > 0)
                {
                    currentAnimationClip = animatorInfo[0].clip;
                    currentAnimation = currentAnimationClip.name;
                    clipDuration = currentAnimationClip.length;
                    animatorSpeed = this.animator.speed;
                    hasLoop = currentAnimationClip.isLooping;
                }
            }
            if (hasLoop == true)
            {
            }
            else
            {
                ReturnToAction(backActionName, clipDuration * animatorSpeed);
            }
        }
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
        finally
        {
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Landing
        if (!isGrounded && hit.transform.CompareTag("Ground"))
        {
            isGrounded = true;
            SetActionName(A_071_LAND_1);
            isFalling = false;
            isJumping = false;
            ReturnToAction(A_011_IDLE_STAND, 1.0f);
        }
    }

    public void RestarLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Respawn()
    {
        gameObject.transform.position = new Vector3(0, 5, 0);
    }
}