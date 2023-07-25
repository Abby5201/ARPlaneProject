using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneController : MonoBehaviour
{
    public static ARPlaneController instance;
    private ARRaycastManager _arRaycastManager;
    
    private Transform selectedObj;//当前选中高亮的物体
    
    List<ARRaycastHit> arHit ;
    private List<ARRaycastHit> hits;

    public Transform target;

    public Transform Axis;//轴向

    private Transform currentObj;//当前选中要移动的物体

    public Button DelBtn;
    public GameObject UIMask;


    public int LiangLongCount = 0;


    public Material outLineMat;
    private Material oriMat;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _arRaycastManager = FindObjectOfType<ARRaycastManager>();
        arHit = new List<ARRaycastHit>();
        hits = new List<ARRaycastHit>();
        EventManager.instance.AddListener(Event.SelectedObjEvent, SelectedEvent); 
        EventManager.instance.AddListener(Event.DeSelectedEvent,DeSelectedEvent);
       // target.gameObject.SetActive(false);
        DelBtn.onClick.AddListener(OnClickDelBtn);
        DelBtn.gameObject.SetActive(false);
    }

    public void SelectedEvent(object go)
    {
        var t = go as GameObject;
        if (t.transform.parent.gameObject != currentObj)
        {
            if (currentObj != null)
            {

                TouchScreen touchSc;
                currentObj.transform.GetChild(0).TryGetComponent<TouchScreen>(out touchSc);
                if (touchSc != null)
                {
                    Debug.Log("Destroy:"+currentObj.name);
                    Destroy(touchSc);
                }
                
                DeSelectedObjOutLine(currentObj.GetChild(0));//重置模型的选中状态
            }
            currentObj = t.transform.parent;
           // Debug.Log("CurrentObj:"+currentObj.name);
            // Axis.gameObject.SetActive(true);
            // Axis.SetParent(currentObj);
            // Axis.localPosition = Vector3.zero;
            // Axis.rotation = Quaternion.identity;
            gameObject.GetComponent<MoveTargetItem>().TargetObj = currentObj;
            currentObj.transform.GetChild(0).gameObject.AddComponent<TouchScreen>();
            DelBtn.gameObject.SetActive(true);
            
            SetSelectedOutLine(t.transform);//添加模型选中效果
        }
    }
    
    /// <summary>
    /// 添加模型选中效果
    /// </summary>
    void SetSelectedOutLine(Transform o)
    {
        selectedObj = o;
        Debug.Log("SelectedObjName:"+selectedObj.name);
        if (selectedObj.name.Equals("LiangLongBox"))
        {
            oriMat = selectedObj.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMaterial;
            var mainTex = oriMat.mainTexture;
            selectedObj.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = outLineMat;
            outLineMat.mainTexture = mainTex;
            outLineMat.SetFloat("_OutlineFactor",0.03f);
        }else //if (selectedObj.name.Equals("EngineBox"))
        {
            oriMat = selectedObj.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;
            var mainTex1 = oriMat.mainTexture;
            // selectedObj.GetChild(0).GetComponent<MeshRenderer>().material = outLineMat;
            
            outLineMat.mainTexture = mainTex1;
            float width = 0.001f;
            if (selectedObj.name.Equals("EngineBox"))
            {
                width = 0.001f;
                
            }else if (selectedObj.name.Equals("YuWaBox"))
            {
                width = 0.02f;
                
            }else if (selectedObj.name.Equals("TongDengBox"))
            {
                width = 0.00001f;
                
            }else if (selectedObj.name.Equals("CrewBox"))
            {
                width = 0.01f;
            }
            else
            {
                width = 0.000002f;
            }
            
            outLineMat.SetFloat("_OutlineFactor",width);
            foreach (var meshRenderer in selectedObj.GetChild(0).GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = outLineMat;
            }
            
        }
    }
    
    

    /// <summary>
    /// 取消描边选中状态
    /// </summary>
    void DeSelectedObjOutLine(Transform o)
    {
        if(o==null)
            return;
        if (o.name.Equals("LiangLongBox"))
        {
            o.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = oriMat;
        }else //if (o.name.Equals("EngineBox"))
        {
            foreach (var meshRenderer in o.GetChild(0).GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = oriMat;
            }
            // o.GetChild(0).GetComponent<MeshRenderer>().material = oriMat;
        }
        if (currentObj != null)
        {
            TouchScreen touchSc;
            currentObj.transform.GetChild(0).TryGetComponent<TouchScreen>(out touchSc);
            if (touchSc != null)
            {
                Destroy(touchSc);
            }
            currentObj = null;
        }
        selectedObj = null;
        // Axis.gameObject.SetActive(false);
        DelBtn.gameObject.SetActive(false);
        
    }

    void DeSelectedEvent(object obj)
    {
        DeSelectedObjOutLine(selectedObj);
    }


    private float timer = 0;
    // Update is called once per frame
    void Update()
    {
        //显示物体拖拽的中心点（屏幕中心点） target
        if (_arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {
            if(UIMask.activeSelf)
                UIMask.SetActive(false);
            hits.Sort((x,y)=>x.distance.CompareTo(y.distance));//排序用于获取与最近平面的交叉点
            target.position = hits[0].pose.position;
            target.rotation = hits[0].pose.rotation;
        }
        
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject() )
            {
                CreatedRay();
            }
            
        }
        else
        {
            if (Input.touchCount==0)
                return;
            
            int fingerId = Input.GetTouch(0).fingerId;
            if (Input.touchCount == 1&&!EventSystem.current.IsPointerOverGameObject(fingerId))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began )
                {
                    timer = 0;
                    Debug.Log("Begin");
                }else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    timer += Time.deltaTime;
                }
                else if(Input.GetTouch(0).phase==TouchPhase.Ended)
                {
                    if (timer < 0.1f)
                    {
                        Debug.Log("End");
                        CreatedRay();
                    }
                }
            }
        }
       
    }

    void CreatedRay()
    {
        Debug.Log("CreateRay");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            if (hit.transform.tag != "Obj")
            {
                    
                EventManager.instance.OnTriggerEvent(Event.DeSelectedEvent, null);
                Debug.Log("没有点击到3d物体上");
            }
            else
            {
                
                Debug.Log("点击到物体身上了: "+hit.transform.name);
                EventManager.instance.OnTriggerEvent(Event.SelectedObjEvent,hit.transform.gameObject);
            }
        }
        else
        {
                
            EventManager.instance.OnTriggerEvent(Event.DeSelectedEvent, null);
            Debug.Log("没有点击到3d物体上");
        }
    }

    /// <summary>
    /// 删除模型
    /// </summary>
   void OnClickDelBtn()
    {
        //TODO:显示是否删除模型二次验证
        if (currentObj.name == "LiangLong")
        {
            LiangLongCount--;
            if (LiangLongCount == 0)
            {
                AudioManager.instance.StopAudio();
            }
        }
        
        Destroy(currentObj.gameObject);
        
        // var o = currentObj.transform.GetChild(0);
        // currentObj.transform.DetachChildren();
        // Destroy(o.gameObject);
        // Destroy(currentObj.gameObject);
        // DelBtn.gameObject.SetActive(false);
        // Axis.gameObject.SetActive(false);
        

    }
    
    bool TryGetTouch(out Vector2 touchPos)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
        Debug.Log("GetTouch");
            touchPos = Input.GetTouch(0).position;
            return true;
        }
#endif
        touchPos = Vector2.zero;

        return false;
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
        
        EventManager.instance.RemoveListener(Event.SelectedObjEvent, SelectedEvent);
    }
}
