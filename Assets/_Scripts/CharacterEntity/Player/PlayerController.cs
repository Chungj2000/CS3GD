using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float MOVEMENT_SPEED;
    [SerializeField] private float ROTATION_SPEED = 10f;
    private bool isWalking;

    private void Start() {
        MOVEMENT_SPEED = Player.INSTANCE.GetParamMOVE_SPD();
    }

    // Update is called once per frame
    private void Update() {

        Vector2 playerMovementVector = InputManager.INSTANCE.GetMovementVector();
        //Debug.Log(playerMovementVector);
        Vector3 playerMoveDirection = new Vector3(playerMovementVector.x, 0f, playerMovementVector.y);

        //Transform player character independant of framerate with movement modifier.
        transform.position += playerMoveDirection * Time.deltaTime * MOVEMENT_SPEED;
        //Lerp function used for smooth rotation.
        transform.forward = Vector3.Slerp(transform.forward, playerMoveDirection, Time.deltaTime * ROTATION_SPEED);
        
        RotatePlayerTowardsCursor();

        //Identify whether player is moving.
        isWalking = playerMoveDirection != Vector3.zero;
        //Debug.Log("IsWalking: " + isWalking);
        
    }

    //Make the player face the cursor position.
    private void RotatePlayerTowardsCursor() {

        //Constant value used to align cursor and player rotation. DO NOT CHANGE.
        const float normaliseAngle = 90f;

        //Return the rotation angle using X and Y vectors between cursor and player.

        float rotationAngle = (Mathf.Atan2(InputManager.INSTANCE.GetCursorPositionFromPlayer().y, InputManager.INSTANCE.GetCursorPositionFromPlayer().x) * Mathf.Rad2Deg) - normaliseAngle;
        //Debug.Log("Rotation Angle: " + rotationAngle);

        //Rotate the object using the angle on a Y axis.
        transform.localRotation = Quaternion.AngleAxis(rotationAngle, Vector3.down);
    }

    public bool IsWalking() {
        //Debug.Log("IsWalking: " + isWalking);
        return isWalking;
    }

}
