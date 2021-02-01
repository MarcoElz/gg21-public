using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIController : MonoBehaviour
{
    [SerializeField] RoomController roomController = default;

    [SerializeField] Transform teamAContainer = default;
    [SerializeField] Transform teamBContainer = default;

    [SerializeField] GameObject playerRowPrefab = default;

    [SerializeField] Button startButton = default;
    [SerializeField] GameObject onlyNonMasterPlayer = default;

    private void OnEnable() => roomController.onTeamsChanged += RefreshUI;
    private void OnDisable() => roomController.onTeamsChanged -= RefreshUI;

    private void Awake()
    {
        bool isMaster = PhotonNetwork.IsMasterClient;
        startButton.gameObject.SetActive(isMaster);
        onlyNonMasterPlayer.SetActive(!isMaster);
    }

    public void ClickChangeTeam()
    {
        object content = PhotonNetwork.LocalPlayer;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkEventCodes.ChangePlayerTeamEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    private void RefreshUI(List<Player> teamA, List<Player> teamB)
    {
        CleanTeamContainers();
        FillContainer(teamA, teamAContainer);
        FillContainer(teamB, teamBContainer);
        VerifyTeamsToStart(teamA, teamB);
    }


    private void FillContainer(List<Player> players, Transform container)
    {
        for(int i = 0; i < players.Count; i++)
        {
            AddPlayerRowToContainer(players[i], container);
        }
    }

    private void AddPlayerRowToContainer(Player player, Transform container)
    {
        GameObject rowObject =  Instantiate(playerRowPrefab, container);
        PlayerRow row = rowObject.GetComponent<PlayerRow>();
        row.SetNickname(player.NickName);
    }

    private void VerifyTeamsToStart(List<Player> teamA, List<Player> teamB)
    {
        bool atLeastTwoInATeam = teamA.Count >= 2;
        bool atLeastOneInBTeam = teamB.Count > 0;

        startButton.interactable = atLeastTwoInATeam && atLeastOneInBTeam;
    }

    private void CleanTeamContainers()
    {
        DestroyAllChildren(teamAContainer);
        DestroyAllChildren(teamBContainer);
    }

    private void DestroyAllChildren(Transform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
