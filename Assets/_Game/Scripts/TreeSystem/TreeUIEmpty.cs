using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeUIEmpty : MonoBehaviour
{
    private UIView view;

    private int currentTreeIndex;

    private void Awake()
    {
        view = GetComponent<UIView>();
    }

    public void SelectDraw(int index)
    {
        FindObjectOfType<TreeSystem>().SaveTreeImage(currentTreeIndex, index);
    }

    public void Show(int treeIndex)
    {
        currentTreeIndex = treeIndex;
        view.Show();
    }

    public void Hide() => view.Hide();

}
