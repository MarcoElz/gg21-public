using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeUITallado : MonoBehaviour
{
    [SerializeField] Sprite[] sprites = default;
    [SerializeField] Image image = default;
    private UIView view;

    private void Awake()
    {
        view = GetComponent<UIView>();
    }

    public void Show(int spriteIndex)
    {
        image.sprite = sprites[spriteIndex];
        view.Show();
    }

    public void Hide() => view.Hide();
}
