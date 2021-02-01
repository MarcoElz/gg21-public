using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectsIfNotMasterClient : MonoBehaviour
{
    [SerializeField] GameObject[] toHide = default;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) return;

        for(int i = 0; i < toHide.Length; i++)
        {
            toHide[i].SetActive(false);
        }
    }

}
