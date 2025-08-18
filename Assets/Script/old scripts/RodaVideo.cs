using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RodaVideo : MonoBehaviour
{
    public VideoClip video;

    public void PlayVideo()
    {
        VideoManager.instance.Play(video);
    }
}
