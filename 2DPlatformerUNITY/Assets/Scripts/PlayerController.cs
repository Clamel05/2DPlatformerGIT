using UnityEngine;

public enum PlayerDirection
{
    left, right
}

public enum PlayerState
{
    idle, walking, jumping, dead
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    private PlayerDirection currentDirection = PlayerDirection.right;
    public PlayerState currentState = PlayerState.idle;
    public PlayerState previousState = PlayerState.idle;

    

    [Header("Horizontal")]
    public float maxSpeed = 5f;
    public float accelerationTime = 0.25f;
    public float decelerationTime = 0.15f;


    [Header("Vertical")]
    public float apexHeight = 3f;
    public float apexTime = 0.5f;


    [Header("Ground Checking")]
    public float groundCheckOffset = 0.5f;
    public Vector2 groundCheckSize = new(0.4f, 0.1f);
    public LayerMask groundCheckMask;

    [Header("Dash CheckR")]
    public float dashCheckOffsetR = 0.5f;
    public Vector2 dashCheckSizeR = new(0.5f, 1.0f);
    public LayerMask dashCheckMaskR;


    [Header("Dash CheckL")]
    public float dashCheckOffsetL = 0.5f;
    public Vector2 dashCheckSizeL = new(0.5f, 1.0f);
    public LayerMask dashCheckMaskL;



    public float dashSpeedLeft = -10f;
    public float dashSpeedRight = 10f;

    private float accelerationRate;
    private float decelerationRate;

    private float gravity;
    private float initialJumpSpeed;

    private bool isGrounded = false;
    public bool isDead = false;
    private bool isDashing = false;

    private Vector2 velocity;


    public void Start()
    {
        body.gravityScale = 0;

        accelerationRate = maxSpeed / accelerationTime;
        decelerationRate = maxSpeed / decelerationTime;

        gravity = -2 * apexHeight / (apexTime * apexTime);
        initialJumpSpeed = 2 * apexHeight / apexTime;

        body.gravityScale = 1;
    }


    public void Update()
    {
        previousState = currentState;

        CheckForContact();

        Vector2 playerInput = new Vector2();
        playerInput.x = Input.GetAxisRaw("Horizontal");

        if (isDead)
        {
            currentState = PlayerState.dead;
        }

        switch(currentState)
        {
            case PlayerState.dead:
                // do nothing - we ded.
                break;
            case PlayerState.idle:
                if (!isGrounded) currentState = PlayerState.jumping;
                else if (velocity.x != 0) currentState = PlayerState.walking;
                break;
            case PlayerState.walking:
                if (!isGrounded) currentState = PlayerState.jumping;
                else if (velocity.x == 0) currentState = PlayerState.idle;
                break;
            case PlayerState.jumping:
                if (isGrounded)
                {
                    if (velocity.x != 0) currentState = PlayerState.walking;
                    else currentState = PlayerState.idle;
                }
                break;
        }

        MovementUpdate(playerInput);
        JumpUpdate();
        DashUpdate();

        if (!isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = 0;

        body.velocity = velocity;
    }


    private void MovementUpdate(Vector2 playerInput)
    {
        if (playerInput.x < 0)
            currentDirection = PlayerDirection.left;
        else if (playerInput.x > 0)
            currentDirection = PlayerDirection.right;

        if (playerInput.x != 0)
        {
            velocity.x += accelerationRate * playerInput.x * Time.deltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }
        else
        {
            if (velocity.x > 0)
            {
                velocity.x -= decelerationRate * Time.deltaTime;
                velocity.x = Mathf.Max(velocity.x, 0);
            }
            else if (velocity.x < 0)
            {
                velocity.x += decelerationRate * Time.deltaTime;
                velocity.x = Mathf.Min(velocity.x, 0);
            }
        }
    }


    private void JumpUpdate()
    {
        if (isGrounded && Input.GetButton("Jump"))
        {
            velocity.y = initialJumpSpeed;
            isGrounded = false;
        }
    }

    private void DashUpdate()
    {
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift)) //NOT WORKING: Gravity, always move to the right - speed whan facing left/right works though.
        {

            if (currentDirection == PlayerDirection.left)
            {
                body.constraints = RigidbodyConstraints2D.FreezePositionY;
                velocity.x = dashSpeedLeft;

            }

            if (currentDirection == PlayerDirection.right)
            {
                velocity.x = dashSpeedRight;
            }
            
        }
        else if (isDashing)
        {
            isDashing = false;
            body.constraints = RigidbodyConstraints2D.None;
        }
        

    }


    private void CheckForContact()
    {
        isGrounded = Physics2D.OverlapBox(transform.position + Vector3.down * groundCheckOffset, groundCheckSize, 0, groundCheckMask);

        isDashing = Physics2D.OverlapBox(transform.position + Vector3.right * dashCheckOffsetR, dashCheckSizeR, 0, dashCheckMaskR);
        isDashing = Physics2D.OverlapBox(transform.position + Vector3.left * dashCheckOffsetL, dashCheckSizeL, 0, dashCheckMaskL);
    }


    public void OnDrawGizmos()
    {
        //Ground
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckOffset, groundCheckSize);
        //Dash
        Gizmos.DrawWireCube(transform.position + Vector3.left * dashCheckOffsetL, dashCheckSizeL);
        Gizmos.DrawWireCube(transform.position + Vector3.right * dashCheckOffsetR, dashCheckSizeR);

    }


    public bool IsWalking()
    {
        return velocity.x != 0;
    }


    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsDashing()
    {
        return isDashing;
    }


    public PlayerDirection GetFacingDirection()
    {
        return currentDirection;
    }

}
