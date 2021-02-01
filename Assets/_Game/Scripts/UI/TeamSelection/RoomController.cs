using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] GameSettings settings = default;

    public event Action<List<Player>, List<Player>> onTeamsChanged;

    private void Awake()
    {
        settings.teamA = new List<Player>();
        settings.teamB = new List<Player>();
    }

    public override void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    public override void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    private void Start()
    {
        Loader.Instance.Hide();
        var players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
            AddPlayerToTeam(players[i]);


        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("There are " + PhotonNetwork.CurrentRoom.PlayerCount + " connected players."
               + "(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")");

        AddPlayerToTeam(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the room" + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("There are " + PhotonNetwork.CurrentRoom.PlayerCount + " connected players."
               + "(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")");

        RemovePlayerFromTeams(otherPlayer);
    }

    private void AddPlayerToTeam(Player player)
    {
        bool isTeamAFewer = settings.teamA.Count <= settings.teamB.Count;

        if (isTeamAFewer)
            settings.teamA.Add(player);
        else
            settings.teamB.Add(player);

        OnTeamsChanged();
    }

    private void RemovePlayerFromTeams(Player player)
    {
        settings.teamA.Remove(player);
        settings.teamB.Remove(player);
        OnTeamsChanged();
    }

    private void OnTeamsChanged()
    {
        onTeamsChanged.Invoke(settings.teamA, settings.teamB);
    }

    public void ChangeTeam(Player player)
    {
        bool isATeam = settings.teamA.Contains(player);

        if(isATeam)
        {
            settings.teamA.Remove(player);
            settings.teamB.Add(player);
        }
        else
        {
            settings.teamB.Remove(player);
            settings.teamA.Add(player);
        }

        OnTeamsChanged();
    }

    private void ShowPlayerList()
    {
        var players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("In room: " + players[i].NickName);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if(eventCode == NetworkEventCodes.ChangePlayerTeamEventCode)
        {
            object data = photonEvent.CustomData;
            Player player = (Player)data;
            ChangeTeam(player);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        SceneController.Instance.LoadInGame();
    }
}
