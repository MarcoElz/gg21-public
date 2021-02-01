using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSystem : MonoBehaviour, IOnEventCallback
{
    private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == NetworkEventCodes.TreeDrawEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int treeIndex = (int)data[0];
            int drawIndex = (int)data[1];

            transform.GetChild(treeIndex).GetComponentInChildren<TreeTrigger>().SetDraw(drawIndex);
        }
    }


    public void SaveTreeImage(int treeIndex, int drawIndex)
    {
        object[] content = new object[] { treeIndex, drawIndex };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkEventCodes.TreeDrawEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
