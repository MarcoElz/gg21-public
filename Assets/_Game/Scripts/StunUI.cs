using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunUI : MonoBehaviour
{
    [SerializeField] PlayerController player = default;
    [SerializeField] Image fillImage = default;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void OnEnable()
    {
        player.onStunned += OnStunned;
        player.onStunSaved += OnFinishStunned;
        player.onStunProgress += OnUpdateStunned;
    }


    private void OnStunned()
    {
        canvas.enabled = true;
    }

    private void OnFinishStunned()
    {
        canvas.enabled = false;
    }

    private void OnUpdateStunned(float fill)
    {
        fillImage.fillAmount = fill;
    }


}
