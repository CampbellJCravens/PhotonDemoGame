using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovePlayer : MonoBehaviourPunCallbacks, IPunObservable
{

    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine) {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * 2 * Time.deltaTime;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(transform.position);
        } else {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
