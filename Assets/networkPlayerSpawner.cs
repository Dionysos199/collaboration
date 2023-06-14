using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class networkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Whale_rigged_by_me", transform.position,transform.rotation);
    }
    public override void OnLeftRoom()
    {

        base.OnLeftRoom();
        Debug.Log("player left the room");
        PhotonNetwork.Destroy(spawnedPlayerPrefab);

    }
}
