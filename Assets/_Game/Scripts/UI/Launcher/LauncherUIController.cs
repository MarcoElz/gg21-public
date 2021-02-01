using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherUIController : MonoBehaviour
{
    [SerializeField] LaunchManager launchManager = default;

    private void OnEnable()
    {
        //Connection OK
        launchManager.onConnectServerStarted += ShowLoader;
        launchManager.onJoinRoomSucced += JoinRoomSucced;

        //Error
        launchManager.onConnectedServerFailed += Error;
        launchManager.onJoinRoomFailed += Error;
    }


    private void ShowLoader() => Loader.Instance.Show();
    private void HideLoader() => Loader.Instance.Hide();

    private void Error()
    {
        Debug.LogError("Connection Error");
    }

    private void JoinRoomSucced()
    {
        SceneController.Instance.LoadTeamSelection();
    }

}
