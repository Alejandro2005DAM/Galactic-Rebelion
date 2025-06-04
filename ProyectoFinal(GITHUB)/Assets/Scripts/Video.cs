using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer.loopPointReached += terminar;
    }

    // Update is called once per frame
    void terminar(VideoPlayer vp)
    {
        SceneManager.LoadScene("GameScene");
    }
}
