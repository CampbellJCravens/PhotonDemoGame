using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CoopConnectScript : MonoBehaviourPunCallbacks
{


    [SerializeField] GameObject loading;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject dropDownGameObject;
    [SerializeField] GameObject customLobbyMessage;
    [SerializeField] GameObject customLobbyMasterMessage;
    [SerializeField] GameObject userIDLobbyMasterMessage;
    [SerializeField] GameObject currentLobby;
    [SerializeField] GameObject roomListingPrefab;

    [SerializeField] Text lobbyKeyText;
    [SerializeField] GameObject cancelButton;

    enum LobbyOptions { customLobby, userId };
    LobbyOptions lobbyOptions = LobbyOptions.customLobby;

    [SerializeField] Dropdown dropdown;
    int dropDownValue = 0;

    string lobbyKey;

    // Start is called before the first frame update
    void Start()
    {
        loading.SetActive(true);
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "campbelljcravens@gmail.com";
        loading.SetActive(false);
        playButton.SetActive(true);
        dropDownGameObject.SetActive(true);
    }

    public override void OnJoinedLobby() {
        print ("JOINED LONBBY");
    }

    public void onDropDownChanged() {
        print ("value = " + dropdown.value);
        setDropDownValue();
    }

    void setDropDownValue() {
        if (dropdown.value == 0) {
            lobbyOptions = LobbyOptions.customLobby;
        } else if (dropdown.value == 1) {
            lobbyOptions = LobbyOptions.userId;
        }
    }

    public void onPlayButtonClicked() {
        if (lobbyOptions == LobbyOptions.customLobby) {
            customLobbyMessage.SetActive(true);
            userIDLobbyMasterMessage.SetActive(false);
            cancelButton.SetActive(true);
        } else if (lobbyOptions == LobbyOptions.userId) {
            customLobbyMessage.SetActive(false);
            userIDLobbyMasterMessage.SetActive(true);
            cancelButton.SetActive(true);
        }
    }

    public void createRoom() {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)2 };
        PhotonNetwork.CreateRoom(lobbyKey, roomOps);
        //PhotonNetwork.JoinLobby();
    }

    void generateLobbyKey() {
        int one = Random.Range(0, 10);
        int two = Random.Range(0, 10);
        int three = Random.Range(0, 10);
        int four = Random.Range(0, 10);
        int five = Random.Range(0, 10);
        int six = Random.Range(0, 10);

        lobbyKey = one.ToString() + two.ToString() + three.ToString() + four.ToString() + five.ToString() + six.ToString();
        lobbyKeyText.text = lobbyKey;
    }

    public void joinLobbyOnClick() {
        GameObject tempListing = Instantiate(roomListingPrefab, currentLobby.transform);
        PhotonNetwork.JoinRoom(lobbyKeyText.text);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        print ("ROOM FAILED TO BE CREATED, LIKELY ROOM WITH SAME NAME ");
    }

    public void matchmakingCancel() {
        customLobbyMasterMessage.SetActive(false);
        customLobbyMessage.SetActive(false);
        userIDLobbyMasterMessage.SetActive(false);
        cancelButton.SetActive(false);
        currentLobby.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }

    
    public void onGenerateKeyClicked() {
        customLobbyMessage.SetActive(false);
        customLobbyMasterMessage.SetActive(true);
        currentLobby.SetActive(true);
        generateLobbyKey();
        createRoom();
    }

    public void startGame() {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(1);
    }
    
}
