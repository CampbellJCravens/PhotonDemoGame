﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CustomMatchmakingLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    GameObject lobbyConnectButton;

    [SerializeField]
    GameObject loadingButton;

    [SerializeField]
    GameObject lobbyPanel;
    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    InputField playerNameInput;
    string roomName;
    int roomSize;
    List<RoomInfo> roomListings;
    [SerializeField]
    Transform roomsContainer;
    [SerializeField]
    GameObject roomListingPrefab;

    public override void OnConnectedToMaster() {
        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyConnectButton.SetActive(true);
        loadingButton.SetActive(false);
        
        roomListings = new List<RoomInfo>();

        if(PlayerPrefs.HasKey("NickName")) {
            if (PlayerPrefs.GetString("NickName") == "") {
                PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
            } else {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
            }
        } else {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
        }
        playerNameInput.text = PhotonNetwork.NickName;
    }

    public void PlayerNameUpdate(string nameInput) {
        PhotonNetwork.NickName = nameInput;
        PlayerPrefs.SetString("NickName", nameInput);
    }

    public void JoinLobbyOnClick() {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        int tempIndex;
        foreach(RoomInfo room in roomList) {
            if (roomListings != null) {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            } else {
                tempIndex = -1;
            }
            if (tempIndex != -1) {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomsContainer.GetChild(tempIndex).gameObject);
            }
            if (room.PlayerCount > 0) {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name) {
        return delegate (RoomInfo room) {
            return room.Name == name;
        };
    }

    void ListRoom(RoomInfo room) {
        if (room.IsOpen && room.IsVisible) {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    public void OnRoomNameChanged(string nameIn) {
        roomName = nameIn;
    }

    public void OnRoomSizeChanged(string sizeIn) {
        roomSize = int.Parse(sizeIn);
    } 
 
    public void CreateRoom() {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        print ("ROOM FAILED TO BE CREATED, LIKELY ROOM WITH SAME NAME ");
    }

    public void MatchmakingCancel() {
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
