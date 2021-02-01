using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] float duration = 60f;

    public event Action<float> onUpdateTimer;

    private float currentTime;
    private bool isActive;

    public void StartTime(int time)
    {
        isActive = true;
        currentTime = duration;
        onUpdateTimer.Invoke(1f);
    }

    private void Update()
    {
        if (!isActive) return;

        currentTime -= Time.deltaTime;
        onUpdateTimer.Invoke(currentTime / duration);

        if(currentTime < 0f)
        {
            isActive = false;
            GameTimeOut();
        }

    }

    private void GameTimeOut()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        object content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkEventCodes.GameEndsLoseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
