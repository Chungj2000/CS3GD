using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private ParameterValues parameterValues;

    private Dictionary<string, string> descriptions;
    private Dictionary<string, float> parameters;

    private const string interactionMessage = "PICK UP [F]";

    private void Awake() {
        parameterValues.Awake();

        parameters = parameterValues.GetParameters();
        descriptions = parameterValues.GetDescriptions();
    }

    private void Update() {
        //Animate the Prefab model.
        AnimatePrefab();
    }

    private void AnimatePrefab() {
        transform.eulerAngles += new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime;
    }

    //Player within range of item therefore add it to the possible items the Player can interact with.
    private void OnTriggerEnter(Collider collider) {

        //Debug.Log("Something has collided with the item.");

        if(collider.tag == "Player") {
            //Debug.Log("Player has been detected");
            PlayerItemHandler.INSTANCE.AddNewItemInRange(this);
        }

    }

    //Player moved outside range of item therefore remove it from the possible items the Player can interact with.
    private void OnTriggerExit(Collider collider) {

        //Debug.Log("Something has exited the item collider.");

        if(collider.tag == "Player") {
            //Debug.Log("Player has been detected");
            PlayerItemHandler.INSTANCE.RemoveItemInRange(this);
        }

    }

    //Update all UI elements when Player within range of interation.
    public void DisplayInteractionNotifications() {

        
        NotificationHandler.INSTANCE.SetItemInteractNotification(interactionMessage);

        NotificationHandler.INSTANCE.SetItemNotification(
            descriptions["itemName"],
            descriptions["itemDesc"]
        );

        PlayerParameterUI.INSTANCE.UpdateItemParametersUI(parameters);

    }

    //Destroy the item when interacted with.
    public void DestroyItem() {
        Debug.Log("Item destroyed.");
        Destroy(gameObject);
    }

    public Dictionary<string, float> GetParameters() => parameterValues.GetParameters();
    public Dictionary<string, string> GetDescriptions() => parameterValues.GetDescriptions();
    public float GetRecoveryMultiplier() => parameterValues.GetRecoveryMultiplier();



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
        [SerializeField] private float multiplierMAX_HP = 1;
        [SerializeField] private float multiplierHP = 1;
        [SerializeField] private float multiplierATK = 1;
        [SerializeField] private float multiplierATK_SPD = 1;
        [SerializeField] private float multiplierATK_RANGE = 1;
        [SerializeField] private float multiplierMOVE_SPD = 1;
        [SerializeField] private float multiplierDEF = 1;

        [Header("Unique Parameters")]

        //Unique field for HP recovery scaled by MAX_HP.
        [SerializeField] private float recoveryMultiplier = 0;

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

            parameters.Add("multiplierMAX_HP", multiplierMAX_HP);
            parameters.Add("multiplierHP", multiplierHP);
            parameters.Add("multiplierATK", multiplierATK);
            parameters.Add("multiplierATK_SPD", multiplierATK_SPD);
            parameters.Add("multiplierATK_RANGE", multiplierATK_RANGE);
            parameters.Add("multiplierMOVE_SPD", multiplierMOVE_SPD);
            parameters.Add("multiplierDEF", multiplierDEF);

            parameters.Add("recoveryMultiplier", recoveryMultiplier);

            //Debug.Log("Item initiated.");

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

        public float GetRecoveryMultiplier() {
            return recoveryMultiplier;
        }

    }

}
