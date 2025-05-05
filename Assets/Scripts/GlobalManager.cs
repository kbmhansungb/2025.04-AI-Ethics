using UnityEngine;

public enum EState
{
    None,
    Info,
    Video,
    Event,
}

public class GlobalManager : MonoBehaviour
{
    static GlobalManager instance;
    public static GlobalManager Instance
    {
        get { return instance; }
    }


    public UserInfoManager userInfoManager;
    public EventManager eventManager;
    public VideoManager videoManager;

    public void Awake()
    {
        instance = this;
    }

    public void SetState(EState newState)
    {
        userInfoManager.gameObject.SetActive(false);
        videoManager.gameObject.SetActive(false);
        eventManager.gameObject.SetActive(false);
        switch (newState)
        {
            case EState.Info:
                userInfoManager.gameObject.SetActive(true);
                userInfoManager.ResetUserInfo();
                break;
            case EState.Video:
                videoManager.gameObject.SetActive(true);
                break;
            case EState.Event:
                eventManager.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
