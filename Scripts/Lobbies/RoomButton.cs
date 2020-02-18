using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    [SerializeField]
    Text sizeText;

    string roomName;
    int roomSize;
    int playerCount;

    public void JoinRoomOnClick() {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void SetRoom(string nameInput, int sizeInput, int countInput) {
        roomName = nameInput;
        roomSize = sizeInput;
        playerCount = countInput;
        nameText.text = nameInput;
        sizeText.text = countInput + "/" + sizeInput;
    }



}
