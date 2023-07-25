using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetItem : MonoBehaviour
{
    public Transform axis; //������ģ��
    public Transform TargetObj;  //Ҫ�ƶ�������
    public Transform axisCamera; //ֻ��Ⱦ������������

    private const float MOVE_SPEED = 0.003F;

    private int currentAxis = 0;//1:x 2:y 3:z Ҫ�ƶ�����
    private bool choosedAxis = false;
    private Vector3 lastPos; //��һ֡���λ��

    void Start()
    {

    }

    public void Init(Transform currentObj,Transform axisObj)
    {
        TargetObj = currentObj;
        axis = axisObj;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0))||( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray ray = axisCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Axis"))) //ֻ���Axis��һ��
            {
                choosedAxis = true;
                lastPos = Input.mousePosition;
                if (hit.collider.name == "x") { currentAxis = 1; }
                if (hit.collider.name == "y") { currentAxis = 2; }
                if (hit.collider.name == "z") { currentAxis = 3; }
            }
        }
        if ((Input.GetMouseButton(0) && choosedAxis)||( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved&&choosedAxis))
        {
            UpdateCubePosition();
        }
        if ((Input.GetMouseButtonUp(0))||(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            choosedAxis = false;
            currentAxis = 0;
        }
    }

    private void UpdateCubePosition()
    {
        Camera camera = axisCamera.GetComponent<Camera>();
        Vector3 origin = camera.WorldToScreenPoint(axis.position);  //����������������ԭ���Ӧ��Ļ����
        Vector3 mouse = Input.mousePosition - lastPos;   //�����֮֡����ƶ��켣����Ļ�ϵ�����

        Vector3 axisEnd_x = camera.WorldToScreenPoint(axis.Find("x/x").position); //������������յ��Ӧ��Ļ����
        Vector3 axisEnd_y = camera.WorldToScreenPoint(axis.Find("y/y").position);
        Vector3 axisEnd_z = camera.WorldToScreenPoint(axis.Find("z/z").position);

        Vector3 vector_x = axisEnd_x - origin;  //x���Ӧ��Ļ����
        Vector3 vector_y = axisEnd_y - origin;
        Vector3 vector_z = axisEnd_z - origin;

        Vector3 cubePos = TargetObj.position;
        float d = Vector3.Distance(Input.mousePosition, lastPos) * MOVE_SPEED; //����ƶ�����
        if (currentAxis == 1)
        {
            //����ƶ��켣��X��нǵ�����ֵ
            float cos = Mathf.Cos(Mathf.PI / 180 * Vector3.Angle(mouse, vector_x));
            if (cos < 0) { d = -d; }
            cubePos.x += d;
            TargetObj.localPosition = cubePos;
            axis.position = cubePos;
        }
        if (currentAxis == 2)
        {

            float cos = Mathf.Cos(Mathf.PI / 180 * Vector3.Angle(mouse, vector_y));
            if (cos < 0) { d = -d; }
            cubePos.y += d;
            TargetObj.position = cubePos;
            axis.position = cubePos;
        }
        if (currentAxis == 3)
        {

            float cos = Mathf.Cos(Mathf.PI / 180 * Vector3.Angle(mouse, vector_z));
            if (cos < 0) { d = -d; }
            cubePos.z += d;
            TargetObj.position = cubePos;
            axis.position = cubePos;
        }


        lastPos = Input.mousePosition;
    }

}
