using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Gamescript : MonoBehaviour
{

    // Singleton reference
    public static Gamescript instance;

    public static PhotonView photonView;
    public static List<string> activeFishTypes = new List<string>();

    public static int initialFishCount = 0;

    void Awake () {
        instance = this;
        // QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient && DelayStartLobbyController.photonActive) { return; }
        createNewFish("Fish", randomSpawn());
        initialFishCount ++;
        StartCoroutine(randomSpawnNewFish());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator randomSpawnNewFish() {
        yield return new WaitForSeconds(3);
        if (initialFishCount < 5) {
            createNewFish("Fish", randomSpawn());
            initialFishCount ++;
            StartCoroutine(randomSpawnNewFish());
        }
    }

    public static Vector3 randomSpawn() {
        int randDirection = Random.Range(0, 4);
        float randX = 0;
        float randZ = 0;
        float minX = -11;
        float maxX = 11;
        float minZ = -8;
        float maxZ = 8;
        if (randDirection == 0) {
            randZ = maxZ;
            randX = Random.Range(minX, maxX);
        } else if (randDirection == 1) {
            randX = minX;
            randZ = Random.Range(minZ, maxZ);
        } else if (randDirection == 2) {
            randZ = minZ;
            randX = Random.Range(minX, maxX);
        } else if (randDirection == 3) {
            randX = maxX;
            randZ = Random.Range(minZ, maxZ);
        }
        return new Vector3(randX, 2.781647f, randZ);
    }

    public static void createNewFish(string fishType, Vector3 spawnPos) {
        if (DelayStartLobbyController.photonActive) {
            GameObject fish = PhotonNetwork.Instantiate(Path.Combine("", fishType), spawnPos, Quaternion.identity);
        } else {
            GameObject fish = Instantiate<GameObject>(Resources.Load<GameObject>(fishType), spawnPos, Quaternion.identity);
        }
        
    }


}
