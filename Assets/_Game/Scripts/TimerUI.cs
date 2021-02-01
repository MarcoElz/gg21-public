using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] GameTimer gameTimer = default;
    [SerializeField] Image imageFill = default;


    private void OnEnable() => gameTimer.onUpdateTimer += UpdateProgress;
    private void OnDisable() => gameTimer.onUpdateTimer -= UpdateProgress;

    private void UpdateProgress(float progress)
    {
        imageFill.fillAmount = progress;
    }

}
