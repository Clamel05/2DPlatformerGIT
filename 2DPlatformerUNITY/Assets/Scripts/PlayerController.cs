using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidPlayer;
    float speed = 0.01f;

    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidPlayer = GetComponent<Rigidbody>();
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
        if (Input.GetKey(KeyCode.A))
        {
            rigidPlayer.AddForce(speed * Time.deltaTime * transform.right);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rigidPlayer.AddForce(speed * Time.deltaTime * -transform.right);
        }


    }

    public bool IsWalking()
    {
        return false;
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
