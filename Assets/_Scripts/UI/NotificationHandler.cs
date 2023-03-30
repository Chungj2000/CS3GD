using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationHandler : MonoBehaviour {
    
    public static NotificationHandler INSTANCE {get; private set;}
    [SerializeField] private TextMeshProUGUI waveNotificationUI;
    [SerializeField] private TextMeshProUGUI interactNotificationUI;

    private void Awake() {

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("NotificationHandler instance created.");
        } else {
            Debug.LogError("More than one NotificationHandler instance created.");
            Destroy(gameObject);
            return;
        }
        
    }

    public IEnumerator SetTimedWaveNotification(string notificationMessage, int duration) {
        waveNotificationUI.text = notificationMessage;
        yield return new WaitForSeconds(duration);
        waveNotificationUI.text = "";
    }

    public void SetInteractNotification(string notificationMessage) {
        interactNotificationUI.text = notificationMessage;
    }

    public void ClearInteractNotification() {
        interactNotificationUI.text = "";
    }

}
