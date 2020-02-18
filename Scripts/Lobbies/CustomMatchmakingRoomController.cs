using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CustomMatchmakingRoomController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    int multiPlayerSceneIndex;

    [SerializeField]
    GameObject lobbyPanel;
    [SerializeField]
    GameObject roomPanel;
    [SerializeField]
    GameObject startButton;

    [SerializeField]
    Transform playersContainer;

    [SerializeField]
    GameObject playerListingsPrefab;

    [SerializeField]
    Text roomNameDisplay;

    void ClearPlayerListings() {
        for (int i = playersContainer.childCount - 1; i >= 0; i--) {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    void ListPlayers() {
        foreach (Player player in PhotonNetwork.PlayerList) {
            GameObject tempListing = Instantiate(playerListingsPrefab, playersContainer);
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }

    public override void OnJoinedRoom() {
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient) {
            startButton.SetActive(true);
        } else {
            startButton.SetActive(false);
        }
        ClearPlayerListings();
        ListPlayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        ClearPlayerListings();
        ListPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        ClearPlayerListings();
        ListPlayers();
        if (PhotonNetwork.IsMasterClient) {
            startButton.SetActive(true);
        }
    }

    public void StartGame() {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiPlayerSceneIndex);
    }

    IEnumerator rejoinLobby() {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick() {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }
    
}
