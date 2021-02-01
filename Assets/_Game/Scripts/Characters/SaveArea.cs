using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            GoodPlayer player = other.GetComponent<GoodPlayer>();

            if (player != null)
            {
                player.photonView.RPC("RPC_Save", RpcTarget.All);
            }
        }
    }
}
