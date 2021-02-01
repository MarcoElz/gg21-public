using Doozy.Engine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNetworkController : MonoBehaviour, IOnEventCallback
{
    [SerializeField] GameObject playerGoodPrefab = default;
    [SerializeField] GameObject playerEvilPrefab = default;
    [SerializeField] GameObject keyPrefab = default;
    [SerializeField] Transform[] goodSpawnPoints = default;
    [SerializeField] Transform[] evilSpawnPoints = default;
    [SerializeField] Transform[] keySpawnPoints = default;
    [SerializeField] GameSettings settings = default;
    [SerializeField] UIView startView = default;
    [SerializeField] UIView loseGhostView = default;
    [SerializeField] UIView winEscapeView = default;
    [SerializeField] Transform doorParent = default;

    List<Transform> remainingGoodSpawnPoints;
    List<Transform> remainingEvilSpawnPoints;

    Dictionary<int, GameObject> playersDictionary;

    public GameObject MyLocalPlayer { get; private set; }

    public bool IsKeyTaken { get; private set; }

    private void Awake()
    {
        playersDictionary = new Dictionary<int, GameObject>();

        remainingGoodSpawnPoints = new List<Transform>(goodSpawnPoints);
        remainingEvilSpawnPoints = new List<Transform>(evilSpawnPoints);
    }

    private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    private void Start()
    {
        if (!PhotonNetwork.IsConnected) return;

        Team team = FindPlayerTeam(PhotonNetwork.LocalPlayer);

        switch(team)
        {
            case Team.Good:
                MyLocalPlayer = PhotonNetwork.Instantiate(playerGoodPrefab.name, Vector3.one, Quaternion.identity);
                //MyLocalPlayer.GetComponent<PlayerController>().SetTeam(Team.Good);
                MyLocalPlayer.GetComponent<PhotonView>().RPC("RPC_SetTeam", RpcTarget.All, Team.Good);
                break;

            case Team.Evil:
                MyLocalPlayer = PhotonNetwork.Instantiate(playerEvilPrefab.name, Vector3.one, Quaternion.identity);
                //MyLocalPlayer.GetComponent<PlayerController>().SetTeam(Team.Evil);
                MyLocalPlayer.GetComponent<PhotonView>().RPC("RPC_SetTeam", RpcTarget.All, Team.Evil);
                break;

            default:
                Debug.LogError("NO TEAM FOR PLAYER: " + PhotonNetwork.LocalPlayer.NickName);
                break;
        }

        
        if(MyLocalPlayer != null)
        {
            if (PhotonNetwork.InRoom)
            {
                object content = MyLocalPlayer.GetComponent<PhotonView>().ViewID;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
                PhotonNetwork.RaiseEvent(NetworkEventCodes.SpawnPlayerEventCode, content, raiseEventOptions, SendOptions.SendReliable);
            }
        }

        if(team.Equals(Team.Evil) && MyLocalPlayer.GetComponent<PhotonView>().IsMine)
        {
            RemoveDoorColliders();
        }


        if(PhotonNetwork.IsMasterClient)
        {
            int randomIndex = Random.Range(0, keySpawnPoints.Length);
            Vector3 position = keySpawnPoints[randomIndex].position;
            PhotonNetwork.Instantiate(keyPrefab.name, position, Quaternion.identity);

            StartCoroutine(GameStartCallEventCoroutine());
        }
    }

    private void RemoveDoorColliders()
    {
        Collider[] collider = doorParent.GetComponentsInChildren<Collider>();
        for(int i = 0; i < collider.Length; i++)
        {
            collider[i].enabled = false;
        }
    }

    public void TakeKey()
    {
        IsKeyTaken = true;
    }

    public void SetPlayerInDictionary(int id, GameObject gameObject)
    {
        playersDictionary.Add(id, gameObject);
    }

    private Vector3 GetRandomPosition(Team team)
    {
        Vector3 position = Vector3.zero;

        if (team.Equals(Team.Good))
        {
            int randomIndex = Random.Range(0, remainingGoodSpawnPoints.Count);
            position = remainingGoodSpawnPoints[randomIndex].position;
            remainingGoodSpawnPoints.RemoveAt(randomIndex);
        }
        else if (team.Equals(Team.Evil))
        {
            int randomIndex = Random.Range(0, remainingEvilSpawnPoints.Count);
            position = remainingEvilSpawnPoints[randomIndex].position;
            remainingEvilSpawnPoints.RemoveAt(randomIndex);
        }

        return position;       
    }


    public Team FindPlayerTeam(Player player)
    {
        bool isATeam = settings.teamA.Contains(player);
        bool isBTeam = settings.teamB.Contains(player);

        if (isATeam) return Team.Good;
        if (isBTeam) return Team.Evil;

        return Team.Null;
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == NetworkEventCodes.SpawnPlayerEventCode)
        {
            object data = photonEvent.CustomData;
            int photonViewId = (int)data;
            StartCoroutine(SetPositionCoroutine(photonViewId));
        }
        else if (eventCode == NetworkEventCodes.GameStartsEventCode)
        {
            object data = photonEvent.CustomData;
            int time = (int)data;

            GameStarted(time);
        }
        else if (eventCode == NetworkEventCodes.GameEndsLoseEventCode)
        {
            loseGhostView.Show();
        }
        else if (eventCode == NetworkEventCodes.GameEndsWinEventCode)
        {   
             winEscapeView.Show();
        }
    }

    private void GameStarted(int time)
    {
        startView.Hide();
        FindObjectOfType<GameTimer>().StartTime(time);
    }

    public void ReturnToMenu()
    {
        SceneController.Instance.LoadTeamSelection();
    }

    private IEnumerator SetPositionCoroutine(int photonViewId)
    {
        yield return new WaitForSeconds(0.5f);

        while (!playersDictionary.ContainsKey(photonViewId))
            yield return null;

        GameObject playerObject = playersDictionary[photonViewId];

        PlayerPhotonSyncView syncPlayer = playerObject.GetComponent<PlayerPhotonSyncView>();
        PlayerController playerController = playerObject.GetComponent<PlayerController>();
        Vector3 position = GetRandomPosition(playerController.Team);
        syncPlayer.photonView.RPC("RPC_SetPosition", RpcTarget.All, position);

    }

    private IEnumerator GameStartCallEventCoroutine()
    {
        yield return new WaitForSeconds(1f);
        
        object content = PhotonNetwork.ServerTimestamp;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkEventCodes.GameStartsEventCode, content, raiseEventOptions, SendOptions.SendReliable);

    }
}


public enum Team { Null, Good, Evil }
