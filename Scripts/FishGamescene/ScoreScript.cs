using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{

    public static int gameScore = 0;

    TextMesh getScoreText;

    // Start is called before the first frame update
    void Start()
    {
        gameScore = 0;
    }

    // public void submitScore() {
    //     postToDatabase();
    // }

    // void postToDatabase() {
    //     User user = new User();
    //     RestClient.Post(AuthenticationScript.databaseURL + User.localId + ".json", user);
    // }

    void updateScore() {
        
    }

   
}
