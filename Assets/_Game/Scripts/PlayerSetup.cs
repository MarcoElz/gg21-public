using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] GameObject[] disableWhenNotOwner = default;

    PlayerController controller;
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if(!photonView.IsMine)
        {
            controller.enabled = false;
            GetComponent<PlayerMovement>().enabled = false;

            for(int i = 0; i < disableWhenNotOwner.Length; i++)
            {
                disableWhenNotOwner[i].SetActive(false);
            }
        }
        else //It's Mine.
        {
            FindObjectOfType<CinemachineVirtualCamera>().Follow = this.transform;
            Transform fov = FindObjectOfType<FieldOfView>().transform;
            Vector3 localPos = fov.localPosition;
            fov.SetParent(this.transform);
            fov.localPosition = localPos;

            controller.SetFov(fov);
        }

        FindObjectOfType<GameNetworkController>().SetPlayerInDictionary(photonView.ViewID, this.gameObject);

    }

}
