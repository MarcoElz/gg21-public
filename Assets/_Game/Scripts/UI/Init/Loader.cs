using UnityEngine;
using Doozy.Engine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class Loader : MonoBehaviour, IOnEventCallback
{
    public static Loader Instance { get; private set; }

    private UIView view;

    private void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    private void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    private void Awake()
    {
        view = GetComponent<UIView>();

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Show() => view.Show();
    public void Hide() => view.Hide();

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == NetworkEventCodes.LoaderEventCode)
        {
            object data = photonEvent.CustomData;
            bool isShow = (bool)data;

            if (isShow)
                Show();
            else
                Hide();
        }
    }
}
