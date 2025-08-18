using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour {

    static public VideoManager instance;

    CanvasGroup canvasGroup;
    VideoPlayer videoPlayer;

    void Awake() {
        instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void Play(VideoClip clip) {
        videoPlayer.clip = clip;
        videoPlayer.Play();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(WaitForVideoEnd());
    }

    IEnumerator WaitForVideoEnd() {
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
