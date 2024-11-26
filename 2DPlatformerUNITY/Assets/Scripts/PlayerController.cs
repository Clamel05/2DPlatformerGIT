using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    Rigidbody2D rigidPlayer;

    public float maxSpeed = 10f;
    public float accelerationTime = 0.25f;
    public float deccelerationTime = 0.15f;

    public float accelerationRate;
    public float deccelerationRate;

    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask jumpLayerMask;
    //[SerializeField] private ForceMode2D forceMode;

    private bool isGrounded = true;

    public float maxHeight = 5f;


    public enum FacingDirection
    {
        left, right

    }


    //public FacingDirection GetFacingDirection;



    private FacingDirection currentDirection = FacingDirection.right;

    private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        //if (rigidPlayer == null)
        //rigidPlayer = GetComponent<Rigidbody2D>();

        body.gravityScale = 0;

        accelerationRate = maxSpeed / accelerationTime;
        deccelerationRate = maxSpeed / deccelerationTime;

    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKey(KeyCode.A))
        {
            GetFacingDirection = GetFacingDirection.Left;

        }*/

        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        playerInput.x = Input.GetAxisRaw("Horizontal");  //Prof way of doing this
        MovementUpdate(playerInput);

        body.velocity = velocity;

    }

    private void MovementUpdate(Vector2 playerInput)
    {
        //Prof way
        if (playerInput.x < 0)
            currentDirection = FacingDirection.left;
        else if (playerInput.x > 0)
            currentDirection = FacingDirection.right;

        if(playerInput.x != 0)
        {
            velocity.x += accelerationRate * playerInput.x * Time.deltaTime;
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }
        else
        {
            if(velocity.x > 0)
            {
                velocity.x -= deccelerationRate * Time.deltaTime;
                velocity.x = Mathf.Max(velocity.x, 0);
            }
            else if (velocity.x < 0)
            {
                velocity.x += deccelerationRate * Time.deltaTime;
                velocity.x = Mathf.Min(velocity.x, 0);
            }
        }



        //My first attempt at task1
        /*if (Input.GetKey(KeyCode.A))
        {
            playerInput = new Vector2(-1, 0);
            rigidPlayer.AddForce(playerInput * maxSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerInput = new Vector2(1, 0);
            rigidPlayer.AddForce(playerInput * maxSpeed);
        }

        //Jumping - Task 1 Journal 7
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerInput = new Vector2(0, 1);
            rigidPlayer.AddForce(playerInput * maxHeight);
            Debug.Log("Jump");
        }
        */
        
    }

    public bool IsWalking()
    {
        return velocity.x != 0;



        /*
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
        {
            Debug.Log("WalkingTrue");
            return true;
        } 
        else
        {
            Debug.Log("WalkingFalse");
            return false;
        }
        */
    }
    public bool IsGrounded()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, jumpLayerMask);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);

        if (isGrounded = hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.green);
            Debug.Log("True");
            return true;
        }
        else
        {
            Debug.Log("False");
            return false;
        }
            
    }

    public FacingDirection GetFacingDirection()
    {
        return currentDirection;
    }
}
