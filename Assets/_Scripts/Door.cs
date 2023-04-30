using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour {

    [SerializeField] private int openTime = 15;
    private const string playerTag = "Player";
    private const string interactionMessage = "OPEN DOOR [F]";
    private Animator doorAnimator;
    private AudioSource audioSource;
    private NavMeshObstacle obstacle;
    private bool isOpen = false;

    void Awake() {
        doorAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        obstacle = GetComponent<NavMeshObstacle>();
    }
    
    //Allows for interaction detection.
    private void OnTriggerStay(Collider collider) {

        //Debug.Log("Something is within the collider.");

        if(collider.gameObject.CompareTag(playerTag)) {
            //Debug.Log("Player has been detected");

            //Tell player that they can interact with the door if it's closed.
            if(!isOpen) {
                NotificationHandler.INSTANCE.SetInteractNotification(interactionMessage);
            }
            
            //When player is within interaction range (collider) and is interacting with the door.
            if(InputManager.INSTANCE.IsInteracting() && !isOpen) {
                //Debug.Log("Player has opened the door.");

                //Player can't open an already open door.
                isOpen = true;

                OpenDoor();
            }

        }

    }

    private void OnTriggerExit(Collider collider) {
        Debug.Log("Something has left the collider.");

        //Clear interaction message if player is leaving and door is not open.
        if(collider.gameObject.CompareTag(playerTag)) {
            NotificationHandler.INSTANCE.ClearInteractNotification();
        }
    }

    private void OpenDoor() {
        doorAnimator.SetTrigger("open");
        SoundSystem.INSTANCE.PlaySFX(SoundSystem.INSTANCE.GetDoorSFX(), audioSource);

        //Stop the NavMeshObstacle from being an obstacle.
        obstacle.enabled = false;
        Debug.Log("Door is currently open.");
        StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor() {
        //Leave the door open for a given amount of time before closing.
        yield return new WaitForSeconds(openTime);

        doorAnimator.SetTrigger("close");
        SoundSystem.INSTANCE.PlaySFX(SoundSystem.INSTANCE.GetDoorSFX(), audioSource);
        obstacle.enabled = true;
        isOpen = false;
        Debug.Log("Door is now closed.");
    }

}
