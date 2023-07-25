using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TakePhotoManager : MonoBehaviour
{
    private Button takePhotoBtn;

    private GameObject uiObj;

    private GameObject showPhotoObj;

    private RawImage showPhotoRawImg;

    private Texture2D texture2D;

    private ARPlaneManager _arPlaneManager;
    private List<ARPlane> _arPlanes;



    // Start is called before the first frame update
    void Start()
    {
        _arPlaneManager = FindObjectOfType<ARPlaneManager>();
        _arPlanes = new List<ARPlane>();
        _arPlaneManager.planesChanged += OnPlaneChanged;
        
        var canvas = GameObject.Find("Canvas").transform;
        takePhotoBtn = canvas.Find("TakePhotoBtn").GetComponent<Button>();
        uiObj = canvas.Find("UI").gameObject;
        showPhotoObj = canvas.Find("ShowPhotoImg").gameObject;
        showPhotoRawImg = showPhotoObj.transform.Find("RawImage").GetComponent<RawImage>();
        showPhotoObj.transform.Find("RawImage/SavePicBtn").GetComponent<Button>().onClick.AddListener(SaveImg);
        takePhotoBtn.onClick.AddListener(TakePhotoBtnClick);
    }

    /// <summary>
    /// 拍照按钮的响应事件
    /// </summary>
    void TakePhotoBtnClick()
    {
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()
    {
        uiObj.SetActive(false);
        takePhotoBtn.gameObject.SetActive(false);
        // _arPlaneManager.planePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_TexTintColor",new Color(1,1,1,0));
        ShowOrHideARPlane(false);
        Debug.Log("隐藏ARPlane:");
       
        
        //TODO:隐藏平面shader
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        yield return new WaitForEndOfFrame();
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();
        yield return tex;
        uiObj.SetActive(true);
        takePhotoBtn.gameObject.SetActive(true);
        ShowOrHideARPlane(true);
        // _arPlaneManager.planePrefab.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_TexTintColor",new Color(1,1,1,1));
        texture2D = tex;
        ShowPhoto(tex);
    }

    void ShowOrHideARPlane(bool isShow)
    {
        foreach (var arPlane in _arPlanes)
        {
            if (arPlane == null || arPlane.gameObject == null)
                _arPlanes.Remove(arPlane);
            else
                arPlane.gameObject.SetActive(isShow);
        }
    }

    void OnPlaneChanged(ARPlanesChangedEventArgs args)
    {
        for (int i = 0; i < args.added.Count; i++)
        {
            _arPlanes.Add(args.added[i]);
        }
    }

    /// <summary>
    /// 显示拍照的图片
    /// </summary>
    void ShowPhoto(Texture2D tex)
    {
        showPhotoObj.SetActive(true);
        showPhotoRawImg.texture = tex;
    }

    /// <summary>
    /// 保存图片按钮的响应事件
    /// </summary>
    private void SaveImg()
    {
        if (texture2D == null)
            return;
        string path = GetPath() + GetCurTime()+".png";
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        string[] paths = new string[1];
        paths[0] = path;
        if (Application.platform == RuntimePlatform.Android)
        {
            ScanFile(paths);
        }
        showPhotoObj.SetActive(false);
    }
    
    private string GetPath()
    {
        string path;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = Application.dataPath + "/ScreenImg/";
        }
        else
        {
            path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android")) +
                   "/DCIM/Camera/";
        }

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return path;
    }

   

    /// <summary>
    /// 刷新图片，显示在相册中
    /// </summary>
    /// <param name="path"></param>
    void ScanFile(string[] path)
    {
        using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
            using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
            {
                Conn.CallStatic("scanFile", playerActivity, path, null, null);
            }
        }
    }

    private void OnDisable()
    {
        _arPlaneManager.planesChanged -= OnPlaneChanged;
    }

    /// <summary>
    /// 获取当前年月日时分秒，如201803081916
    /// </summary>
    /// <returns></returns>
    string GetCurTime()
    {
        return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
               + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
    }
    
}
