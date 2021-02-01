using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTrigger : MonoBehaviour
{
    public int DrawIndex { get; private set; }

    private void Awake()
    {
        DrawIndex = -1;
    }

    public void SetDraw(int drawIndex)
    {
        DrawIndex = drawIndex;
    }

    private void Show()
    {
        if(DrawIndex < 0)
        {
            FindObjectOfType<TreeUIEmpty>().Show(transform.parent.GetSiblingIndex());
        }
        else
        {
            FindObjectOfType<TreeUITallado>().Show(DrawIndex);
        }
    }

    private void Hide()
    {
        if (DrawIndex < 0)
        {
            FindObjectOfType<TreeUIEmpty>().Hide();
        }
        else
        {
            FindObjectOfType<TreeUITallado>().Hide();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            GoodPlayer goodPlayer = other.GetComponent<GoodPlayer>();          
            if (goodPlayer != null && goodPlayer.photonView.IsMine)
            {
                Show();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            GoodPlayer goodPlayer = other.GetComponent<GoodPlayer>();
            if (goodPlayer != null && goodPlayer.photonView.IsMine)
            {
                Hide();
            }
        }
    }

}
