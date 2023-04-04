using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour {

    public static PlayerItemHandler INSTANCE {get; private set;}
    
    //Stores references of items within range of the Player.
    private List<ItemHandler> items = new List<ItemHandler>();

    private Canvas itemOverlayUI;
    private const string itemOverlayUI_tag = "ItemOverlayUI";

    private void Awake() {

        itemOverlayUI = GameObject.FindWithTag(itemOverlayUI_tag).GetComponent<Canvas>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayerItemHandler instance created.");
        } else {
            Debug.LogError("More than one PlayerItemHandler instance created.");
            Destroy(gameObject);
            return;
        }
    }

    private void Update() {
        PlayerItemInteraction();
    }

    private void PlayerItemInteraction() {

        if(items.Count > 0) {

            //Debug.Log(items.Count + " items present.");

            ItemHandler closestItem = GetClosestItemToPlayer();

            //Player is dead, therefore hide item notifications and do not allow interactions.
            if(Player.INSTANCE.IsDead()) {
                ConcealNotifications();
                return;
            }

            //Display item notifications when items are within Player interaction range (collision).
            ShowItemOverlay();
            closestItem.DisplayInteractionNotifications();

            //Hide UI visuals when currently paused.
            if(PauseMenuHandler.INSTANCE.IsPauseActive()) {
                ConcealNotifications();
            }

            //When interacting with valid items.
            if(InputManager.INSTANCE.IsInteracting()) {

                //Debug.Log("Player has interacted with an item: " + closestItem);
                Player.INSTANCE.InteractWithItem(closestItem);

                //When item is about to be Destroyed from being picked up.
                ConcealNotifications();

                //Remove the reference and item when picked up.
                RemoveItemInRange(closestItem);
                closestItem.DestroyItem();

            }

        } else {

            //Debug.Log("No items present.");

            //Hide notifications when there are no nearby items.
            ConcealNotifications();

        }
        
    }

    public void AddNewItemInRange(ItemHandler item) {
        
        items.Add(item);
        //Debug.Log("Item added.");

    }

    public void RemoveItemInRange(ItemHandler item) {
        
        items.Remove(item);
        //Debug.Log("Item removed.");

    }

    private ItemHandler GetClosestItemToPlayer() {

        ItemHandler closestItem = null;
        float closetDistance = Mathf.Infinity;

        foreach(ItemHandler item in items) {

            float distanceFromCurrentItem = Vector3.Distance(transform.position, item.transform.position);
            //Debug.Log("Current item distance from player: " + distanceFromCurrentItem);

            if(closetDistance > distanceFromCurrentItem) {

                closetDistance = distanceFromCurrentItem;
                //Debug.Log("Current closest distance: " + closetDistance);
                closestItem = item;

            }

        }

        //Debug.Log("Closest item: " + closestItem);

        return closestItem;

    }

    //Display UI.
    private void ShowItemOverlay() {
        itemOverlayUI.enabled = true;
    }

    //Hide UI.
    private void HideItemOverlay() {
        itemOverlayUI.enabled = false;
    }

    private void ConcealNotifications() {
        //Remove interaction & item messages.
        NotificationHandler.INSTANCE.ClearInteractNotification();

        HideItemOverlay();
        NotificationHandler.INSTANCE.ClearItemNotification();

        PlayerParameterUI.INSTANCE.ClearChangeAllValues();
    }

}
