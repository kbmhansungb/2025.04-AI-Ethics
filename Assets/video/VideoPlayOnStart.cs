using UnityEngine;
using UnityEngine.Video;

public class VideoPlayOnStart : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.Play();
    }
}
