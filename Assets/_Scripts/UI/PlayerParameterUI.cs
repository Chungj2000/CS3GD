using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerParameterUI : MonoBehaviour {

    public static PlayerParameterUI INSTANCE {get; private set;}
    
    [Header("Player Parameters Values")]
    [SerializeField] TextMeshProUGUI textParamMAX_HP;
    [SerializeField] TextMeshProUGUI textParamHP;
    [SerializeField] TextMeshProUGUI textParamATK;
    [SerializeField] TextMeshProUGUI textParamATK_SPD;
    [SerializeField] TextMeshProUGUI textParamATK_RANGE;
    [SerializeField] TextMeshProUGUI textParamMOVE_SPD;
    [SerializeField] TextMeshProUGUI textParamDEF;

    [Header("Player Parameters Change Values")]
    [SerializeField] TextMeshProUGUI textChangeParamMAX_HP;
    [SerializeField] TextMeshProUGUI textChangeParamHP;
    [SerializeField] TextMeshProUGUI textChangeParamATK;
    [SerializeField] TextMeshProUGUI textChangeParamATK_SPD;
    [SerializeField] TextMeshProUGUI textChangeParamATK_RANGE;
    [SerializeField] TextMeshProUGUI textChangeParamMOVE_SPD;
    [SerializeField] TextMeshProUGUI textChangeParamDEF;

    //Parameters values.
    private float playerMAX_HP;
    private float playerHP;
    private float playerATK;
    private float playerATK_SPD;
    private float playerATK_RANGE;
    private float playerMOVE_SPD;
    private float playerDEF;

    private Dictionary<string, float> itemParameters;

    //Colors.
    private Color changeValueIncrease = Color.green;
    private Color changeValueDecrease = Color.red; 

    private void Awake() {

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayerParameterUI instance created.");
        } else {
            Debug.LogError("More than one PlayerParameterUI instance created.");
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        //Update UI with Player initial parameter values.
        SetPlayerParameterUI();
    }

    public void SetPlayerParameterUI() {
        playerMAX_HP = Player.INSTANCE.GetParamMAX_HP();
        playerHP = Player.INSTANCE.GetParamHP();
        playerATK = Player.INSTANCE.GetParamATK();
        playerATK_SPD = Player.INSTANCE.GetParamATK_SPD();
        playerATK_RANGE = Player.INSTANCE.GetParamATK_RANGE();
        playerMOVE_SPD = Player.INSTANCE.GetParamMOVE_SPD();
        playerDEF = Player.INSTANCE.GetParamDEF();

        UpdatePlayerParameterUI();
    }

    private void UpdatePlayerParameterUI() {
        textParamMAX_HP.text = playerMAX_HP.ToString();
        textParamHP.text = playerHP.ToString();
        textParamATK.text = playerATK.ToString();
        textParamATK_SPD.text = playerATK_SPD.ToString();
        textParamATK_RANGE.text = playerATK_RANGE.ToString();
        textParamMOVE_SPD.text = playerMOVE_SPD.ToString();
        textParamDEF.text = playerDEF.ToString();
    }

    //Used only to update when health value is changed from taking, or increasing health.
    public void UpdatePlayerParameterUI_Health() {
        playerHP = Player.INSTANCE.GetParamHP();
        textParamHP.text = playerHP.ToString();
    }

    public void UpdateItemParametersUI(Dictionary<string, float> itemParameters) {

        SetItemParameters(itemParameters);

        //Determine which function to call based on item parameters.
        CompareItemParameters(playerMAX_HP, "paramMAX_HP", "multiplierMAX_HP", textChangeParamMAX_HP);
        CompareItemParameterHP(playerMAX_HP, playerHP, "paramHP", "multiplierHP", "recoveryMultiplier", textChangeParamHP);
        CompareItemParameters(playerATK, "paramATK", "multiplierATK", textChangeParamATK);
        CompareItemParameters(playerATK_SPD, "paramATK_SPD", "multiplierATK_SPD", textChangeParamATK_SPD);
        CompareItemParameters(playerATK_RANGE, "paramATK_RANGE", "multiplierATK_RANGE", textChangeParamATK_RANGE);
        CompareItemParameters(playerMOVE_SPD, "paramMOVE_SPD", "multiplierMOVE_SPD", textChangeParamMOVE_SPD);
        CompareItemParameters(playerDEF, "paramDEF", "multiplierDEF", textChangeParamDEF);

    }

    private void SetItemParameters(Dictionary<string, float> itemParameters) {
        this.itemParameters = itemParameters;
    }

    //Identify whether the item increases/decreases parameter by additive, multiplicative values or both.
    private void CompareItemParameters(float playerParam, string parameterModifierKey, string parameterMultiplierKey, TextMeshProUGUI changeValue) {

        if(playerParam != playerParam + itemParameters[parameterModifierKey] && 
            playerParam != playerParam * itemParameters[parameterMultiplierKey]) {

            //Item gives an additive and multiplicative change.
            UpdateItemParameterUI(playerParam, parameterModifierKey, parameterMultiplierKey, changeValue);

        } else if(playerParam != playerParam + itemParameters[parameterModifierKey]) {

            //Item gives an additive change.
            UpdateItemModifiedParameterUI(playerParam, parameterModifierKey, changeValue);

        } else if(playerParam != playerParam * itemParameters[parameterMultiplierKey]) {

            //Item gives a multiplicative change.
            UpdateItemMultipliedParameterUI(playerParam, parameterMultiplierKey, changeValue);

        } else {

            //No change to parameter, therefore remove visual.
            ClearChangeValue(changeValue);

        }

    }

    //Recovery should not be given a value if other HP modifiers are also given values else it will break the function.
    //Identify whether the item increases/decreases HP by additive, multiplicative, scaled by MAX_HP values or a combination.
    private void CompareItemParameterHP(float playerMAX_HP, float playerHP, string modifiedHP, string multipliedHP, string recoveryHP, TextMeshProUGUI changeValue) {

        if(playerHP != playerHP + itemParameters[modifiedHP] && 
            playerHP != playerHP * itemParameters[multipliedHP]) {

            //Item gives an additive and multiplicative change to HP.
            UpdateItemParameterUI(playerHP, modifiedHP, multipliedHP, changeValue);

        } else if(playerHP != playerHP + itemParameters[modifiedHP]) {

            //Item adds/removes a value of HP.
            UpdateItemModifiedParameterUI(playerHP, modifiedHP, changeValue);

        } else if(playerHP != playerHP * itemParameters[multipliedHP]) {

            //Item multiplies HP by a value.
            UpdateItemMultipliedParameterUI(playerHP, multipliedHP, changeValue);

        } else if(playerHP != playerHP + (playerMAX_HP * itemParameters[recoveryHP])) {

            //Item recovers a percentage of HP based on MAX_HP scaling.
            UpdateItemRecoveryParameterUI(playerMAX_HP, playerHP, recoveryHP, changeValue);

        } else {

            //No change to parameter, therefore remove visual.
            ClearChangeValue(changeValue);

        }

    }

    //Calculate the quantitative change of the specified parameter when modified and multiplied, and display the output.
    private void UpdateItemParameterUI(float playerParam, string parameterModifierKey, string parameterMultiplierKey, TextMeshProUGUI changeValue) {

        if(playerParam < ((playerParam + itemParameters[parameterModifierKey]) * itemParameters[parameterMultiplierKey])) {
            //If item increases Player parameter.
            changeValue.text = string.Format("[+" + (((playerParam + itemParameters[parameterModifierKey]) * itemParameters[parameterMultiplierKey]) - playerParam) +  "]");
            changeValue.color = changeValueIncrease;
        } else {
            //If item decreases Player parameter. Value should already have a '-'.
            changeValue.text = string.Format("[" + (((playerParam + itemParameters[parameterModifierKey]) * itemParameters[parameterMultiplierKey]) - playerParam) +  "]");
            changeValue.color = changeValueDecrease;
        } 

    }

    //Display the additive change for the specified parameter.
    private void UpdateItemModifiedParameterUI(float playerParam, string parameterKey, TextMeshProUGUI changeValue) {

        if(playerParam < playerParam + itemParameters[parameterKey]) {
            //If item increases Player parameter.
            changeValue.text = string.Format("[+" + itemParameters[parameterKey] +  "]");
            changeValue.color = changeValueIncrease;
        } else {
            //If item decreases Player parameter. Value should already have a '-'.
            changeValue.text = string.Format("[" + itemParameters[parameterKey] +  "]");
            changeValue.color = changeValueDecrease;
        } 

    }

    //Calculate the multiplicative value of change for the specified parameter, and display the output.
    private void UpdateItemMultipliedParameterUI(float playerParam, string parameterKey, TextMeshProUGUI changeValue) {

        if(playerParam < playerParam * itemParameters[parameterKey]) {
            //If item increases Player parameter.
            changeValue.text = string.Format("[+" + ((playerParam * itemParameters[parameterKey]) - playerParam) +  "]");
            changeValue.color = changeValueIncrease;
        } else {
            //If item decreases Player parameter. Value should already have a '-'.
            changeValue.text = string.Format("[" + ((playerParam * itemParameters[parameterKey]) - playerParam) +  "]");
            changeValue.color = changeValueDecrease;
        }

    }

    //Calculate the recovery amount of HP and display the output.
    private void UpdateItemRecoveryParameterUI(float playerMAX_HP, float playerHP, string recoveryHP, TextMeshProUGUI changeValue) {

        if(playerHP < playerHP + (playerMAX_HP * itemParameters[recoveryHP])) {
            //If item increases Player parameter.
            changeValue.text = string.Format("[+" + (playerMAX_HP * itemParameters[recoveryHP]) +  "]");
            changeValue.color = changeValueIncrease;
        } else {
            //If item decreases Player parameter. Value should already have a '-'.
            changeValue.text = string.Format("[" + (playerMAX_HP * itemParameters[recoveryHP]) +  "]");
            changeValue.color = changeValueDecrease;
        }

    }

    private void ClearChangeValue(TextMeshProUGUI changeValue) {
        changeValue.text = "";
    }

    public void ClearChangeAllValues() {
        textChangeParamMAX_HP.text = "";
        textChangeParamHP.text = "";
        textChangeParamATK.text = "";
        textChangeParamATK_SPD.text = "";
        textChangeParamATK_RANGE.text = "";
        textChangeParamMOVE_SPD.text = "";
        textChangeParamDEF.text = "";
    }

}
