using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidPlayer;
    public float speed = 1f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask jumpLayerMask;
    //[SerializeField] private ForceMode2D forceMode;

    private bool isGrounded = false;

    public float maxHeight = 5f;


    //public FacingDirection GetFacingDirection;

    public enum FacingDirection
    {
        left, right

    }

    // Start is called before the first frame update
    void Start()
    {
        if (rigidPlayer == null)
        rigidPlayer = GetComponent<Rigidbody2D>();
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
        MovementUpdate(playerInput);

    }

    private void MovementUpdate(Vector2 playerInput)
    {

        //My first attempt at task1
        if (Input.GetKey(KeyCode.A))
        {
            playerInput = new Vector2(-1, 0);
            rigidPlayer.AddForce(playerInput * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerInput = new Vector2(1, 0);
            rigidPlayer.AddForce(playerInput * speed);
        }

        //Jumping - Task 1 Journal 7
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerInput = new Vector2(0, 1);
            rigidPlayer.AddForce(playerInput * maxHeight * Time.deltaTime);
            Debug.Log("Jump");
        }

        
    }

    public bool IsWalking()
    {
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
        return false;
    }
    public bool IsGrounded()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, jumpLayerMask);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);

        if (isGrounded = hitInfo.collider != null)
        {
            print(hitInfo.collider.name);
            Debug.DrawLine(transform.position, hitInfo.point, Color.green);
            Debug.Log("False");
            return false;
        }
        else
        {
            Debug.Log("True");
            return true;
        }
            

    }

    public FacingDirection GetFacingDirection()
    {
        return FacingDirection.left;
    }
}
