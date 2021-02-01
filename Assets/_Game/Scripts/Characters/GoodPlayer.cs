using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodPlayer : MonoBehaviourPun
{
    [SerializeField] float stunDuration = 60f;

    PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    [PunRPC]
    public void RPC_Stun()
    {
        controller.Stun(stunDuration);
    }

    [PunRPC]
    public void RPC_Save()
    {
        controller.SaveStun();
    }


}
