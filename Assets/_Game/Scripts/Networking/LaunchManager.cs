using Doozy.Engine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    [SerializeField] UIView splashView = default;
    [SerializeField] float splashTime = 3f;

    #region Events
    public event Action onConnectServerStarted;
    public event Action onConnectServerSucced;
    public event Action onConnectedServerFailed;

    public event Action onJoinRoomStarted;
    public event Action onJoinRoomSucced;
    public event Action onJoinRoomFailed;
    #endregion

    #region Unity
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        splashView.Hide(splashTime);
    }
    #endregion

    #region Public Methods
    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            onConnectServerStarted?.Invoke();
        }
    }

    public void JoinRandomRoom()
    {
        onJoinRoomStarted?.Invoke();
        Debug.Log("Entrando a random room...");
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    #region Photon Override
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " Connected to Photon Servers");
        onConnectServerSucced?.Invoke();

        JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        onConnectedServerFailed?.Invoke();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();
    }



    public override void OnJoinedRoom()
    {
        onJoinRoomSucced?.Invoke();
        PhotonNetwork.LoadLevel(1);
        //Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        onJoinRoomFailed?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("There are " + PhotonNetwork.CurrentRoom.PlayerCount + " connected players."
               + "(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the room" + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("There are " + PhotonNetwork.CurrentRoom.PlayerCount + " connected players."
               + "(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")");
    }

    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        Debug.Log("Creando un nuevo room...");
        string randomRoomName = "Room " + Random.Range(0, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 10;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion

}
