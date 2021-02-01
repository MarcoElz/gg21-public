using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LauncherView : MonoBehaviour
{
    [SerializeField] Button connectionButton = default;
    public void SetNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;

        bool isNicknameValid = !string.IsNullOrEmpty(nickname);

        connectionButton.interactable = isNicknameValid;
    }
}
