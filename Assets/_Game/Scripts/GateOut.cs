using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOut : MonoBehaviour
{
    [SerializeField] GameSettings settings = default;

    int count;

    bool alreadyWin;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character"))
        {
            GoodPlayer goodPlayer = other.GetComponent<GoodPlayer>();
            if(goodPlayer != null)
            {
                if (goodPlayer.photonView.IsMine) FindObjectOfType<DoorMessage>().Show();

                count++;

                if(count == settings.teamA.Count && FindObjectOfType<GameNetworkController>().IsKeyTaken)
                {
                    EscapeWin();
                    alreadyWin = true;
                }
            }
        }
    }

    private void EscapeWin()
    {
        if (alreadyWin) return;

        if(PhotonNetwork.IsMasterClient)
        {
            object content = null;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(NetworkEventCodes.GameEndsWinEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            GoodPlayer goodPlayer = other.GetComponent<GoodPlayer>();
            if (goodPlayer != null)
            {
                if(goodPlayer.photonView.IsMine) FindObjectOfType<DoorMessage>().Hide();
                count--;
            }
        }
    }

}
