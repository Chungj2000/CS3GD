using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationHandler : MonoBehaviour {
    
    public static NotificationHandler INSTANCE {get; private set;}
    private TextMeshProUGUI notificationUI;

    private void Awake() {
        notificationUI = GetComponent<TextMeshProUGUI>();

        if(INSTANCE == null) {
            INSTANCE = this;
            Debug.Log("NotificationHandler instance created.");
        } else {
            Debug.LogError("More than one NotificationHandler instance created.");
            Destroy(gameObject);
            return;
        }
    }

    public IEnumerator SetNotification(string notificationMessage, int duration) {
        notificationUI.text = notificationMessage;
        yield return new WaitForSeconds(duration);
        notificationUI.text = "";
    }

}
