﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{

    public static bool photonActive = false;

    [SerializeField]
    private GameObject delayStartButton;

    [SerializeField]
    private GameObject delayCancelButton;

    [SerializeField]
    int roomSize;

    public override void OnConnectedToMaster() {
        PhotonNetwork.AutomaticallySyncScene = true;
        delayStartButton.SetActive(true);
        photonActive = true;
    }

    public void DelayStart() {
        delayStartButton.SetActive(false);
        delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Delay Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        createRoom();
    }

    void createRoom() {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Failed to create room... trying again");
        createRoom();
    }

    public void delayCancel() {
        delayCancelButton.SetActive(false);
        delayStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
 }
