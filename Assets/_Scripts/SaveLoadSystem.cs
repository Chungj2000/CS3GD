using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour {
    
    [Serializable]
    public struct SaveState {

        public int waveCount;
        public int scoreCount;
        public float playTimeCount;

        public Vector3 playerPosition;
        public Quaternion playerRotation;

        public float playerMAX_HP;
        public float playerHP;
        public float playerATK;
        public float playerATK_SPD;
        public float playerATK_RANGE;
        public float playerMOVE_SPD;
        public float playerDEF;

        public SaveState(int waveCount, int scoreCount, float playTimeCount, Player player) {

            //Save system data.
            this.waveCount = waveCount;
            this.scoreCount = scoreCount;
            this.playTimeCount = playTimeCount;

            //Save the player object transformation.
            playerPosition = player.gameObject.transform.position;
            playerRotation = player.gameObject.transform.rotation;

            //Save player parameters.
            playerMAX_HP = player.GetParamMAX_HP();
            playerHP = player.GetParamHP();
            playerATK = player.GetParamATK();
            playerATK_SPD = player.GetParamATK_SPD();
            playerATK_RANGE = player.GetParamATK_RANGE();
            playerMOVE_SPD = player.GetParamMOVE_SPD();
            playerDEF = player.GetParamDEF();

        }

    }

    public static SaveLoadSystem INSTANCE {get; private set;}

    [SerializeField] private string xmlName = "Assets/Saves/SaveData.xml";
    private bool isLoadedGame = false;

    private void Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("SaveLoadSystem instance created.");
        } else {
            //Debug.LogError("More than one SaveLoadSystem instance created.");
            Destroy(this);
            return;
        }
    }

    private void Start() {
        isLoadedGame = PlayerPrefs.GetInt("loadGame") != 0;
        Debug.Log("Is Loading Game: " + isLoadedGame);

        //Check if the newly created scene is for loading save data, if true load the save data.
        if(isLoadedGame) {
            LoadGame();
            isLoadedGame = false;
            //Can't set bools for PlayerPref hence SetInt.
            PlayerPrefs.SetInt("loadGame", isLoadedGame ? 1 : 0);
            Debug.Log("Is Loading Game: " + isLoadedGame);
        }
    }

    public void SaveGame() {
        SaveGameState(xmlName);
        Debug.Log("Saving data with handle: " + xmlName);
    }

    private void LoadGame() {
        LoadGameState(xmlName);
        Debug.Log("Loading data with handle: " + xmlName);
    }

    //Create an XML to persist the save data of the game.
    private void SaveGameState(string saveFileName) {

        XmlDocument xml = new XmlDocument();

        //Create the save state.
        SaveState saveData = new SaveState(WaveSystem.INSTANCE.GetWaveCount(),
                                  ScoreTracker.INSTANCE.GetScore(),
                                  PlayTimeTracker.INSTANCE.GetPlayTime(),
                                  Player.INSTANCE);

        XmlSerializer serializer = new XmlSerializer(typeof(SaveState));

        using (MemoryStream stream = new MemoryStream()) {
            serializer.Serialize(stream, saveData);
            stream.Position = 0;
            xml.Load(stream);
            xml.Save(saveFileName);
        }

        Debug.Log("Save data created.");
    }

    //Load an XML of a given name to load persisted save data.
    private void LoadGameState(string saveFileName) {

        XmlDocument xml = new XmlDocument();

        //Get the save state XML document.
        xml.Load(saveFileName);
        string xmlString = xml.OuterXml;

        SaveState saveData;

        //Get the save data using readers from the XML document.
        using (StringReader stringReader = new StringReader(xmlString)) {

            XmlSerializer serializer = new XmlSerializer(typeof(SaveState));

            using (XmlReader xmlReader = new XmlTextReader(stringReader)) {
                saveData = (SaveState) serializer.Deserialize(xmlReader);
            }

        }

        //Set the game data to the respective components.
        WaveSystem.INSTANCE.SetWaveCount(saveData.waveCount);
        ScoreTracker.INSTANCE.SetScore(saveData.scoreCount);
        PlayTimeTracker.INSTANCE.SetPlayTime(saveData.playTimeCount);

        Player.INSTANCE.SetTranform(saveData.playerPosition, saveData.playerRotation);
        Player.INSTANCE.SetParamMAX_HP(saveData.playerMAX_HP);
        Player.INSTANCE.SetParamHP(saveData.playerHP);
        Player.INSTANCE.SetParamATK(saveData.playerATK);
        Player.INSTANCE.SetParamATK_SPD(saveData.playerATK_SPD);
        Player.INSTANCE.SetParamATK_RANGE(saveData.playerATK_RANGE);
        Player.INSTANCE.SetParamMOVE_SPD(saveData.playerMOVE_SPD);
        Player.INSTANCE.SetParamDEF(saveData.playerDEF);

        Player.INSTANCE.UpdatePlayerParameterUI();

        Debug.Log("Save data loaded.");
    }

}
