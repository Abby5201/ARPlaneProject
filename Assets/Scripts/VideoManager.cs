using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    private bool playOnAwake = false;

    private Transform maskImage;
    private Button pauseBtn;
    private Button playBtn;
    private Button fullScreenBtn;

    private RenderTexture currentRenderTexture;

    private VideoPlayer _videoPlayerFullScreen;
    private Button _videoFullCloseBtn;

    
    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        maskImage = transform.Find("Video Player/MaskImage");
        pauseBtn = transform.Find("Video Player/MaskImage/PauseBtn").GetComponent<Button>();
        playBtn = transform.Find("Video Player/MaskImage/PlayBtn").GetComponent<Button>();
        fullScreenBtn = transform.Find("Video Player/MaskImage/FullScreenBtn").GetComponent<Button>();
        _videoPlayerFullScreen = GameObject.Find("Canvas").transform.Find("VideoObj/Video Player")
            .GetComponent<VideoPlayer>();
        _videoFullCloseBtn = _videoPlayerFullScreen.transform.parent.Find("CloseBtn").GetComponent<Button>();
        
        _videoPlayer.GetComponent<Button>().onClick.AddListener(ShowMaskImage);
        pauseBtn.onClick.AddListener(PauseBtnClick);
        playBtn.onClick.AddListener(PlayBtnClick);
        fullScreenBtn.onClick.AddListener(FullScreenBtnClick);
        
        
        playBtn.gameObject.SetActive(false);
        fullScreenBtn.gameObject.SetActive(false);

        int width = (int)_videoPlayer.clip.width;
        int height =(int)_videoPlayer.clip.height;
        Debug.Log("width:"+width);
        Debug.Log("height:"+height);
        RenderTexture rt = new RenderTexture(1920, 1080,0);
        _videoPlayer.GetComponent<RawImage>().texture = rt;
        _videoPlayer.targetTexture = rt;
        currentRenderTexture = rt;

        _videoPlayer.time = 2;
        _videoPlayer.Pause();
    }

    /// <summary>
    /// 暂停按钮点击
    /// </summary>
    void PauseBtnClick()
    {
        if (_videoPlayer.clip != null)
        {
            _videoPlayer.Play();
            maskImage.gameObject.SetActive(false);
            pauseBtn.gameObject.SetActive(false);
            playBtn.gameObject.SetActive(true);
            fullScreenBtn.gameObject.SetActive(true);
        }
            
    }

    /// <summary>
    /// 播放按钮点击
    /// </summary>
    void PlayBtnClick()
    {
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
        fullScreenBtn.gameObject.SetActive(true);
        _videoPlayer.Pause();
    }

    /// <summary>
    /// 全屏按钮点击
    /// </summary>
    void FullScreenBtnClick()
    {
        _videoPlayerFullScreen.transform.parent.gameObject.SetActive(true);
        _videoPlayerFullScreen.targetTexture = currentRenderTexture;
        _videoPlayerFullScreen.GetComponent<RawImage>().texture = currentRenderTexture;
    }

    /// <summary>
    /// 点击正在播放的视频，显示暂停按钮
    /// </summary>
    void ShowMaskImage()
    {
        maskImage.gameObject.SetActive(true);
    }

   
}
