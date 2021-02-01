using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StunArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            GoodPlayer player = other.GetComponent<GoodPlayer>();

            if (player != null)
            {
                player.photonView.RPC("RPC_Stun", RpcTarget.All);
            }
        }
    }

}
