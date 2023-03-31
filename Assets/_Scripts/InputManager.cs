using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    //Declare singleton.
    public static InputManager INSTANCE {get; private set;}

    private PlayerInputActions playerInputActions;
    private Vector3 playerOnScreenPosition;
    private Vector3 cursorPositionFromPlayer;

    private void Awake() {

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("InputManager instance created.");
        } else {
            Debug.LogError("More than one InputManager instance created.");
        }
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.DefaultPlayer.Enable();

    }

    private void Update() {
        //Caculate the X and Y vectors from mouse and player object.
        playerOnScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        cursorPositionFromPlayer = Input.mousePosition - playerOnScreenPosition;
    }

    //Change to Singleton later.
    public Vector2 GetMovementVector() {

        //Vectors are already normalized from playerInputActions package generated code.
        Vector2 playerMovementVector = playerInputActions.DefaultPlayer.Move.ReadValue<Vector2>();
        
        //Debug.Log(playerMovementVector);

        return playerMovementVector;

    }

    public GameObject GetPlayer() {
        return gameObject;
    }

    public Vector3 GetCursorPositionFromPlayer() {
        //Debug.Log("Cursor At: "  + cursorPositionFromPlayer);
        return cursorPositionFromPlayer;
    }

    public PlayerInputActions GetPlayerInputActions() {
        return playerInputActions;
    }

    public bool IsAttacking() {
        if (playerInputActions.DefaultPlayer.Attack.triggered) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsInteracting() {
        if (playerInputActions.DefaultPlayer.Interact.triggered) {
            return true;
        } else {
            return false;
        }
    }

}
