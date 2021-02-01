using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    [SerializeField] float startDelay = 0.25f;

    private void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(NextSceneCoroutine());
    }

    IEnumerator NextSceneCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        SceneController.Instance.LoadLauncher();
    }

}
