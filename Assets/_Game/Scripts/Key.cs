using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviourPun
{

    private void Start()
    {
        Team team = FindObjectOfType<GameNetworkController>().FindPlayerTeam(PhotonNetwork.LocalPlayer);

        if(!team.Equals(Team.Good))
        {
            this.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void RPC_TakeKey()
    {
        FindObjectOfType<GameNetworkController>().TakeKey();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            GoodPlayer goodPlayer = other.GetComponent<GoodPlayer>();
            if (goodPlayer != null && goodPlayer.photonView.IsMine)
            {
                photonView.RPC("RPC_TakeKey", RpcTarget.All);
            }
        }
    }

}
