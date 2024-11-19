using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidPlayer;
    public float speed = 1f;

    public enum FacingDirection
    {
        left, right

    }

    // Start is called before the first frame update
    void Start()
    {
        rigidPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    public bool IsWalking()
    {

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


    }
    public bool IsGrounded()
    {
        return false;
    }

    public FacingDirection GetFacingDirection()
    {
        return FacingDirection.left;
    }
}
