using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMessage : MonoBehaviour
{

    private UIView view;

    private void Awake()
    {
        view = GetComponent<UIView>();
    }

    public void Show()
    {
        view.Show();
    }

    public void Hide() => view.Hide();

}
