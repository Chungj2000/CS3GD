using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour {

    [SerializeField] private ParameterValues parameterValues;
    private Dictionary<string, float> parameters;
    private int interactDistance = 1;

    private void Awake() {
        parameterValues.Awake();
        parameters = parameterValues.GetParameters();
    }

    private void Update() {

        //Determine whether the player is within an interaction distance.
        ItemInteraction();

    }

    private void ItemInteraction() {

        if(PlayerWithinInteractDistance()) {
            
            NotificationHandler.INSTANCE.SetInteractNotification("INTERACT [F]");

            if(InputManager.INSTANCE.IsInteracting()) {
                Debug.Log("Player has interacted with an item.");
                Player.INSTANCE.InteractWithItem(this);
            }

        } else {

            NotificationHandler.INSTANCE.ClearInteractNotification();

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

    /** Multiply all parameters by x amount.
    public Dictionary<string, float> GetMultipliedParameters(float multiplier) {
        foreach(string parameter in parameters.Keys) {
            parameters[parameter] = parameters[parameter] * multiplier;
        }

        return parameters;
    } **/

    public Dictionary<string, float> GetParameters() => parameterValues.GetParameters();

    [System.Serializable]
    private class ParameterValues {

        [Header("Item Parameter Modifiers")]
        [SerializeField] private float paramMAX_HP = 0;
        [SerializeField] private float paramHP = 0;
        [SerializeField] private float paramATK = 0;
        [SerializeField] private float paramATK_SPD = 0;
        [SerializeField] private float paramATK_RANGE = 0;
        [SerializeField] private float paramMOVE_SPD = 0;
        [SerializeField] private float paramDEF = 0;
        //[Header("=Parameter Multipliers")]

        private Dictionary<string, float> parameters = new Dictionary<string, float>();

        public void Awake() {
            parameters.Add("paramMAX_HP", paramMAX_HP);
            parameters.Add("paramHP", paramHP);
            parameters.Add("paramATK", paramATK);
            parameters.Add("paramATK_SPD", paramATK_SPD);
            parameters.Add("paramATK_RANGE", paramATK_RANGE);
            parameters.Add("paramMOVE_SPD", paramMOVE_SPD);
            parameters.Add("paramDEF", paramDEF);
            Debug.Log("Item initiated.");
        }

        public int Size() {
            return parameters.Count;
        }

        public Dictionary<string, float> GetParameters() {
            return parameters;
        }

    }

}
