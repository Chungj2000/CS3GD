using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationHandler : MonoBehaviour {
    
    public static NotificationHandler INSTANCE {get; private set;}

    [Header("GameOverlayUI Fields")]
    [SerializeField] private TextMeshProUGUI waveNotificationUI;
    [SerializeField] private TextMeshProUGUI interactNotificationUI;
    [SerializeField] private TextMeshProUGUI itemInteractNotificationUI;

    [Header("ItemOverlayUI Fields")]
    [SerializeField] private TextMeshProUGUI itemNameNotificationUI;
    [SerializeField] private TextMeshProUGUI itemDescNotificationUI;

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

    public void SetItemInteractNotification(string notificationMessage) {
        itemInteractNotificationUI.text = notificationMessage;
    }

    public void ClearItemInteractNotification() {
        itemInteractNotificationUI.text = "";
    }

    public void SetItemNotification(string itemName, string itemDesc) {
        itemNameNotificationUI.text = itemName;
        itemDescNotificationUI.text = itemDesc;
    }

    public void ClearItemNotification() {
        itemNameNotificationUI.text = "";
        itemDescNotificationUI.text = "";
    }

}
