using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Transform toggleParent;
    public ScrollRect ScrollRect;

    private Transform viewPort;
    
    // Start is called before the first frame update
    void Start()
    {
        viewPort = ScrollRect.transform.Find("ViewPort");
        
        for (int i = 0; i < toggleParent.GetComponentsInChildren<Toggle>().Length; i++)
        {
            int j = i;
            var toggle = toggleParent.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((inon) => { OnToggleChanged(inon,j);});
        }
        EventManager.instance.AddListener(Event.IngObjEvent,RestToggle);
        // DelBtn.onClick.AddListener(OnClickDelBtn);
        // foreach (var t in toggleParent.GetComponentsInChildren<Toggle>())
        // {
        //     t.onValueChanged.AddListener((bool ison) => { OnToggleChanged(ison,i);});
        //     i++;
        // }
        
    }

    void OnToggleChanged(bool isOn,int index)
    {
        if (isOn)
        {
            string name = "Engine";
            if (index !=0)
            {
                name = "KongLong";
            }

            ARPlaneController.instance.target .gameObject.SetActive(true);
            Global.CanInsObj = true;
            Global.name = name;

            ScrollRect.content = viewPort.GetChild(index).GetComponent<RectTransform>();
        }
    }

    void RestToggle(object obj)
    {
        foreach (var t in toggleParent.GetComponentsInChildren<Toggle>())
        {
            t.isOn = false;
        }
    }

    
}
