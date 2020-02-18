using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{

    PhotonView myPhotonView;

    [SerializeField] int multiplayerSceneIndex;
    [SerializeField] int menuSceneIndex;

    int playerCount;
    int roomSize;

    [SerializeField] int minPlayersToStart;

    [SerializeField] Text playerCountDisplay;
    [SerializeField] Text timerToStartDisplay;

    bool readyToCountDown;
    bool readyToStart;
    bool startingGame;
    
    float timerToStartGame;
    float notFullGameTimer;
    float fullGameTimer;

    [SerializeField] float maxWaitTime;
    [SerializeField] float maxFullGameWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        playerCountUpdate();
        setPlayerColor();
        
    }

    void setPlayerColor() {
        print ("PlayerList length = " + PhotonNetwork.PlayerList.Length);
        print ("PlayeCount = " + playerCount);
        print ("isMasterClient = " + PhotonNetwork.IsMasterClient);
        if (playerCount == 2) {
            GamesceneSetup.playerColor = "Blue";
        } else if (playerCount == 3) {
            GamesceneSetup.playerColor = "Red";
        }
    }

    void playerCountUpdate() {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountDisplay.text = playerCount + " / " + roomSize;

        if (playerCount == roomSize) {
            readyToStart = true;
        } else if (playerCount >= minPlayersToStart) {
            readyToCountDown = true;
        } else {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        playerCountUpdate();
        if (PhotonNetwork.IsMasterClient) {
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
        }
        print ("playercolor = " + GamesceneSetup.playerColor);
    }

    [PunRPC]
    void RPC_SendTimer(float timeIn) {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer) {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        playerCountUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        waitingForMorePlayers();
    }

    void waitingForMorePlayers() {
        if (playerCount <= 1) {
            resetTimer();
        }
        if (readyToStart) {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        } else if (readyToCountDown) {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        } 
        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if (timerToStartGame <= 0f) {
            if (startingGame) { 
                return;
            } else {
                startGame();
            }
        }
    }

    void startGame() {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient) {
            return;
        } else {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }

    void resetTimer() {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    public void delayCancel() {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
