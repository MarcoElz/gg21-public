using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void LoadLauncher() => LoadScene(1);
    public void LoadTeamSelection() => LoadScene(2);
    public void LoadInGame() => LoadScene(3);

    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

    IEnumerator LoadSceneCoroutine(int index)
    {
        SetLoaderEvent(true);
        yield return new WaitForSeconds(0.5f);

        PhotonNetwork.LoadLevel(index);

        yield return new WaitForSeconds(0.5f);
        SetLoaderEvent(false);
    }

    private void SetLoaderEvent(bool show)
    {
        if(PhotonNetwork.InRoom)
        {
            object content = show;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(NetworkEventCodes.LoaderEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            if (show)
                Loader.Instance.Show();
            else
                Loader.Instance.Hide();
        }
        
    }

}
