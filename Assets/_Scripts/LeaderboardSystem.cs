using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Linq;

public class ScoresResult {
    public int Scores;
    public string code;
    public string message;
}

//Descending order sort.
public class SortedKeysInDescending : IComparer<int> {
    public int Compare(int x, int y) { 
        return -x.CompareTo(y); 
    }

}

public class LeaderboardSystem : MonoBehaviour {
    
    public static LeaderboardSystem INSTANCE {get; private set;}

    private Leaderboard leaderboard;
    private LeaderboardUI leaderboardUI;
    private IEnumerator CR_Send;
    private IEnumerator CR_Receive;

    private const string scoresTableName = "Leaderboard";

    //Five score rows in the leaderboard.
    private string[] scoresID = new string[]{"13B13E62-1D4E-4EE5-9B43-315EBCEFEDAC", 
                                             "7E5ECF86-D43B-4D77-91BC-E1735EA77973", 
                                             "E286C5F7-1F6D-4D19-8419-CC0A1B45F799", 
                                             "A5CAD149-B045-4E8B-81AB-2EDCD90247E0", 
                                             "8A9D9D10-C500-440F-8187-902FD3F49909"};

    private const string applicationType = "REST";
    private const string contentType = "application/json";

    private SortedDictionary<int, string> scoreByID;
    private string backendURL;
    private bool leaderboardInitialised = false;
    private bool newScoreSaved = false;
    private int index = 0;
    private int newScoreIndex;

    private void Awake() {

        leaderboard = GetComponent<Leaderboard>();
        leaderboardUI = GetComponent<LeaderboardUI>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("LeaderboardSystem instance created.");
        } else {
            Debug.LogError("More than one LeaderboardSystem instance created.");
            Destroy(this);
            return;
        }

    }

    private void Start() {
        scoreByID = new SortedDictionary<int, string>(new SortedKeysInDescending());
    }

    public void GetHighScore() {
        CR_Receive = CR_GetHighScore();
        StartCoroutine(CR_Receive);
    }

    //For populating the leaderboard.
    public void GetHighScores() {
        CR_Receive = CR_GetHighScores();
        StartCoroutine(CR_Receive);
    }

    public void SetHighScore(int score) {
        CR_Send = CR_SetHighScore(score);
        StartCoroutine(CR_Send);
    }

    //Return general string of Backendless URL.
    private string ProcessSetURL() {
        return "https://eu-api.backendless.com/" + 
                Globals.APPLICATION_ID + "/" +
                Globals.REST_SECRET_KEY + 
                "/data/" + scoresTableName + "/";
    }

    private void SetRequestHeaders(UnityWebRequest webRequest) {
        //Mandatory reuqest headers for backend.
        webRequest.SetRequestHeader("application-id", Globals.APPLICATION_ID);
        webRequest.SetRequestHeader("secret-key", Globals.REST_SECRET_KEY);
        webRequest.SetRequestHeader("application-type", applicationType);
    }

    //Return error encountered in console.
    private void CheckForWebErrors(UnityWebRequest webRequest) {
        if (webRequest.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log("ConnectionError");
        } else if (webRequest.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log("ProtocolError");
        } else if (webRequest.result == UnityWebRequest.Result.DataProcessingError) {
            Debug.Log("DataProcessingError");
        } else if (webRequest.result == UnityWebRequest.Result.Success) {
            Debug.Log("Success");
            DownloadHandlerToJson(webRequest);
        } else {
            Debug.Log("Unknown state.");
        }
    }

    private void DownloadHandlerToJson(UnityWebRequest webRequest) {
        ScoresResult scoresData = JsonUtility.FromJson<ScoresResult>(webRequest.downloadHandler.text);

        //Debug.Log("Score is: " + scoresData.Scores);

        InitiateLeaderboard(scoresData.Scores);

        //For Testing:
        //ReturnSortedDictionary();

        if (!string.IsNullOrEmpty(scoresData.code)) {
            Debug.Log("Error:" + scoresData.code + " " + scoresData.message);
        }
    }

    //Populate the leaderboard with Backend Scores data.
    private void InitiateLeaderboard(int score) {
        if(!leaderboardInitialised) {

            leaderboard.AddScore(score);
            scoreByID.Add(score, scoresID[index]);

            if(index == scoresID.Length - 1) {
                leaderboardInitialised = true;
                Debug.Log("Leaderboard initialised.");

                //ReturnSortedDictionary();
                
                //Set the current highscore to beat.
                PlayerPrefs.SetInt("highScore", leaderboard.GetLeaderboardScores()[0]);
                //GameOverHandler.INSTANCE.SetHighScore(leaderboard.GetLeaderboardScores()[0]);

                //Update visuals.
                leaderboardUI.PopulateLeaderboard(leaderboard.GetLeaderboardScores());
            }

        }
    }

    private IEnumerator CR_GetHighScore() {
        //Debug.Log("Getting Score.");

        //Populate URL with variables.
        backendURL = ProcessSetURL();

        Debug.Log("URL: " + backendURL);

        //Create a web request using the URL for Backend.
        UnityWebRequest webRequest = UnityWebRequest.Get(backendURL); 

        //Debug.Log("Request made.");

        SetRequestHeaders(webRequest);

        //Debug.Log("Headers set.");

        yield return webRequest.SendWebRequest();

        CheckForWebErrors(webRequest);
    }

    //For populating the leaderboard.
    private IEnumerator CR_GetHighScores() {
        //Debug.Log("Getting Scores.");

        for(int x = 0; x < scoresID.Length; x++) {
            //Populate URL with variables.
            backendURL = ProcessSetURL() + scoresID[x];

            index = x;

            //Create a web request using the URL for Backend.
            UnityWebRequest webRequest = UnityWebRequest.Get(backendURL); 

            //Debug.Log("Request made.");

            SetRequestHeaders(webRequest);

            //Debug.Log("Headers set.");

            yield return webRequest.SendWebRequest();

            CheckForWebErrors(webRequest);
        }
    }

    //Replace leaderboard scores with the new score.
    private IEnumerator CR_SetHighScore(int score) {

        string data;
        int oldScore = 0;

        //Identify every score lower than the position of the new score.
        for(int x = newScoreIndex; x < leaderboard.GetLeaderboardScores().Count; x++) {

            if(!newScoreSaved) {
                //Set the new score in the leaderboard.
                data = JsonUtility.ToJson(new ScoresResult { Scores = score});
                newScoreSaved = true;
            } else {
                //Set the old scores below the new one in the backendless leaderboard.
                data = JsonUtility.ToJson(new ScoresResult { Scores = oldScore });
            }

            //Save the old score before it gets overwritten. So we can overwrite the score below.
            oldScore = leaderboard.GetLeaderboardScores()[x];

            //Populate URL with a new score using a reversed sorted dictionary
            backendURL = ProcessSetURL() + scoreByID.ElementAt(x).Value;

            //Pass URL and data
            UnityWebRequest webRequest = UnityWebRequest.Put(backendURL, data); 

            webRequest.SetRequestHeader("Content-Type", contentType);
            SetRequestHeaders(webRequest);

            yield return webRequest.SendWebRequest();

            CheckForWebErrors(webRequest);
        
        }

    }

    private void CheckIndex() {
        if(index > scoresID.Length) {
            ResetIndex();
        }
    }

    private void ResetIndex() {
        index = 0;
    }

    public string[] GetScoresID() {
        return scoresID;
    }

    public Leaderboard GetLeaderboard() {
        return leaderboard;
    }

    public LeaderboardUI GetLeaderboardUI() {
        return leaderboardUI;
    }

    public void SetNewScoreIndex(int index) {
        newScoreIndex = index;
    }

    public void ReturnSortedDictionary() {
        foreach(int key in scoreByID.Keys) {
            Debug.Log(key);
        }
    }

}