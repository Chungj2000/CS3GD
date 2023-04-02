using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private ParameterValues parameterValues;

    private Dictionary<string, string> descriptions;
    private Dictionary<string, float> parameters;

    private int interactDistance = 1;
    private Canvas itemOverlayUI;

    private const string itemOverlayUI_tag = "ItemOverlayUI";
    private const string interactionMessage = "PICK UP [F]";

    private void Awake() {
        parameterValues.Awake();
        parameters = parameterValues.GetParameters();
        descriptions = parameterValues.GetDescriptions();
        itemOverlayUI = GameObject.FindWithTag(itemOverlayUI_tag).GetComponent<Canvas>();
    }

    private void Update() {

        //Determine whether the player is within an interaction distance.
        ItemInteraction();

        //Animate the Prefab model.
        AnimatePrefab();

    }

    private void AnimatePrefab() {
        transform.eulerAngles += new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime;
    }

    private void ItemInteraction() {

        if(PlayerWithinInteractDistance()) {
            
            //Update all UI elements when Player within range of interation.
            NotificationHandler.INSTANCE.SetInteractNotification(interactionMessage);

            ShowItemOverlay();
            NotificationHandler.INSTANCE.SetItemNotification(
                descriptions["itemName"],
                descriptions["itemDesc"]
            );

            //Hide UI visuals when currently paused.
            if(PauseMenuHandler.INSTANCE.IsPauseActive()) {
                ConcealNotifications();
            }

            if(InputManager.INSTANCE.IsInteracting()) {

                Debug.Log("Player has interacted with an item.");
                Player.INSTANCE.InteractWithItem(this);

                //When item is about to be Destroyed from being picked up.
                ConcealNotifications();

                //Remove the item when picked up.
                DestroyItem();

            }

        } else if (Player.INSTANCE.IsDead()) {

            //When Player is Dead.
            ConcealNotifications();

        } else {

            //If Player is out of range.
            ConcealNotifications();

        }

    }

    private bool PlayerWithinInteractDistance() {

        //Player is dead, therefore doesn't exist.
        if(Player.INSTANCE.IsDead()) {
            return false;
        }

        if(Vector3.Distance(Player.INSTANCE.transform.position, transform.position) <= interactDistance) {
            //Debug.Log("Player is within interaction distance.");
            return true;
        } else {
            //Debug.Log("Player is not within interaction distance.");
            return false;
        }

    }

    public Dictionary<string, float> GetMultipliedParameters(float multiplier) {
        foreach(string parameter in parameters.Keys) {
            parameters[parameter] = parameters[parameter] * multiplier;
        }

        return parameters;
    }

    private void ShowItemOverlay() {
        itemOverlayUI.enabled = true;
    }

    private void HideItemOverlay() {
        itemOverlayUI.enabled = false;
    }

    private void ConcealNotifications() {

        //Remove interaction & item messages.
        NotificationHandler.INSTANCE.ClearInteractNotification();

        HideItemOverlay();
        NotificationHandler.INSTANCE.ClearItemNotification();

    }

    private void DestroyItem() {
        Debug.Log("Item destroyed.");
        Destroy(gameObject);
    }

    public Dictionary<string, float> GetParameters() => parameterValues.GetParameters();
    public Dictionary<string, string> GetDescriptions() => parameterValues.GetDescriptions();



    [System.Serializable]
    private class ParameterValues {

        [Header("Item Descriptors")]
        [SerializeField] private string itemName;
        [SerializeField] private string itemDesc;

        [Header("Item Parameter Modifiers")]
        [SerializeField] private float paramMAX_HP = 0;
        [SerializeField] private float paramHP = 0;
        [SerializeField] private float paramATK = 0;
        [SerializeField] private float paramATK_SPD = 0;
        [SerializeField] private float paramATK_RANGE = 0;
        [SerializeField] private float paramMOVE_SPD = 0;
        [SerializeField] private float paramDEF = 0;

        [Header("Parameter Multipliers")]
        [SerializeField] private float multipltierMAX_HP = 1;
        [SerializeField] private float multipltierHP = 1;
        [SerializeField] private float multipltierATK = 1;
        [SerializeField] private float multipltierATK_SPD = 1;
        [SerializeField] private float multipltierATK_RANGE = 1;
        [SerializeField] private float multipltierMOVE_SPD = 1;
        [SerializeField] private float multipltierDEF = 1;

        private Dictionary<string, string> descriptions = new Dictionary<string, string>();
        private Dictionary<string, float> parameters = new Dictionary<string, float>();

        public void Awake() {

            //Initialise descriptions.
            descriptions.Add("itemName", itemName);
            descriptions.Add("itemDesc", itemDesc);

            //Initialise parameters.
            parameters.Add("paramMAX_HP", paramMAX_HP);
            parameters.Add("paramHP", paramHP);
            parameters.Add("paramATK", paramATK);
            parameters.Add("paramATK_SPD", paramATK_SPD);
            parameters.Add("paramATK_RANGE", paramATK_RANGE);
            parameters.Add("paramMOVE_SPD", paramMOVE_SPD);
            parameters.Add("paramDEF", paramDEF);

            parameters.Add("multipltierMAX_HP", multipltierMAX_HP);
            parameters.Add("multipltierHP", multipltierHP);
            parameters.Add("multipltierATK", multipltierATK);
            parameters.Add("multipltierATK_SPD", multipltierATK_SPD);
            parameters.Add("multipltierATK_RANGE", multipltierATK_RANGE);
            parameters.Add("multipltierMOVE_SPD", multipltierMOVE_SPD);
            parameters.Add("multipltierDEF", multipltierDEF);

            Debug.Log("Item initiated.");

        }

        public int Size() {
            return parameters.Count;
        }

        public Dictionary<string, string> GetDescriptions() {
            return descriptions;
        }

        public Dictionary<string, float> GetParameters() {
            return parameters;
        }

    }

}
