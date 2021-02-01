using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRow : MonoBehaviour
{
    [SerializeField] TMP_Text nicknameLabel = default;

    public void SetNickname(string nickname)
    {
        nicknameLabel.text = nickname;
    }

}
