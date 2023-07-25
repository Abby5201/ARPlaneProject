using System.Linq;
using Newtonsoft.Json;
using Siccity.GLTFUtility;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject obj;
    public Transform Parent;
    private bool canDrag = true;
    private Transform axisTrans;

    public GameObject targetPanel;

    private string objNam;
    private string[] culturalRelic;
    
    // Start is called before the first frame update
    void Start()
    {
        targetPanel.SetActive(false);
        culturalRelic = new[] { "TongWoHu", "YuWa" };
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //创建防御塔
        //tower = Resources.Load<GameObject>("Cube");
        string name = gameObject.name;
        //Debug.Log("startDragName:"+name);
        if(GetName(name)=="")
            return;
        
        GameObject go = Resources.Load<GameObject>("Model/"+GetName(name));
        obj = Instantiate(go, Parent);
        obj.name = name;
        // string path = Application.dataPath + "/Resources/Model/" + GetName(name)+".gltf";
        // Debug.Log(path);
        // GameObject go = LoadGLBFile(path);
        // go.transform.SetParent(Parent);
        // go.name = name;
        // obj = go;
        SetObjPos();
        targetPanel.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetObjPos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(obj==null)
            return;
       // var mousePos = Input.mousePosition;
       // float z = Camera.main.transform.position.z;
       // Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 3));

       var targetPos = ARPlaneController.instance.target.position;

       if (IsCulturalRelic(obj.name))
       {
           obj.transform.position = new Vector3(targetPos.x,0,targetPos.z);
       }
       else
       {
           obj.transform.position = targetPos;
       }
       
        obj.transform.rotation = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0);
        
        
        targetPanel.SetActive(false);
        EventManager.instance.OnTriggerEvent(Event.PlayAudioEvent,objNam);
        GetCount();
    }

    /// <summary>
    /// 判断是否是文物，确认实例化的物体是否要和手机齐平
    /// </summary>
    /// <returns></returns>
    bool IsCulturalRelic(string name)
    {
        return culturalRelic.Contains(name);
    }

    /// <summary>
    /// 获取每个动物拖出的个数
    /// </summary>
    void GetCount()
    {
        switch (objNam)
        {
            case "LiangLong":
                ARPlaneController.instance.LiangLongCount++;
                break;
        }
    }

  
    void SetObjPos()
    {
        if(obj==null)
            return;
        var mousePos = Input.mousePosition;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 3));
        obj.transform.position = pos;
    }
    
    /// <summary>
    /// 从本地文件夹加载glb文件
    /// </summary>
    /// <param name="path"></param>
    public GameObject LoadGLBFile(string path)
    {
        GameObject go = Importer.LoadFromFile(path);

        return go;
        // go.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
    
    string GetName(string name)
    {
        string prefabName = "";
        switch (name)
        {
            case "Engine":
                prefabName = "Engine";
                break;
            case "LiangLong":
                prefabName = "LiangLong";
                break;
            case "TongDeng":
                prefabName = "TongDeng";
                break;
            case "YuWa":
                prefabName = "YuWa";
                break;
            case "TongWoHu":
                prefabName = "TongWoHu";
                break;
            case "SZTY":
                prefabName = "SZTY";
                break;
            case "SMSR":
                prefabName = "SMSR";
                break;
            case "Crew":
                prefabName = "Crew";
                break;
            case "图片":
                prefabName = "Square";
                break;
            case "视频":
                prefabName = "Video";
                break;
        }

        objNam = prefabName;
        return prefabName;
    }
 
}
