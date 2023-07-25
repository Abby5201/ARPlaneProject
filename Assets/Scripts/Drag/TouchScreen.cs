using UnityEngine;

public class TouchScreen : MonoBehaviour
{
    private Touch oldTouch1; //�ϴδ�����1(��ָ1)  
    private Touch oldTouch2; //�ϴδ�����2(��ָ2)  

    Touch newTouch1;
    Touch newTouch2;

    private float rotateSpeed = 0.3f;


    // Update is called once per frame 
    void Update()
    {
        //û�д���  
        if (Input.touchCount <= 0)
        {
            return;
        }

        

        //�����϶�
        //else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //{
        //    //
        //    var deltaposition = Input.GetTouch(0).deltaPosition;


        //    transform.Translate(new Vector3(deltaposition.x * 0.01f, deltaposition.y * 0.01f, 0), Space.World);
        //    Debug.Log("pos:" + transform.position);
        //}
        //TODO:�Ƿ�����ui������ж�
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var deltaPos = Input.GetTouch(0).deltaPosition;
            
            transform.Rotate(-Vector3.up * deltaPos.x * rotateSpeed, Space.World);
        }
        //˫ָ��ת  && ����
        else if (2 <= Input.touchCount)
        {
            //Debug.Log("���㴥���� ˮƽ������ת  ");
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition; //λ������

            // if (Mathf.Abs(deltaPos.x) >= 3 || Mathf.Abs(deltaPos.y) >= 3)
            // {
            //     transform.Rotate(-transform.up * deltaPos.x * 0.1f, Space.World); //��y����ת
            //     // transform.Rotate(transform.right * deltaPos.y*0.1f, Space.World);//��x��
            // }

            //����
            newTouch1 = Input.GetTouch(0); //
            newTouch2 = Input.GetTouch(1);
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }

            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position); //�����������
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position); //�����������
            float offset = newDistance - oldDistance;


            if (Mathf.Abs(offset) >= 3)
            {
                // Debug.Log(offset);
                //�Ŵ����ӣ� һ�����ذ� 0.01������(100�ɵ���)  
                float scaleFactor = offset / 100f;
                Vector3 localScale = transform.localScale;
                Vector3 scale = new Vector3(localScale.x + scaleFactor,
                    localScale.y + scaleFactor,
                    localScale.z + scaleFactor);

                //��С���ŵ� 0.3 �� ���3�� 
                if (scale.x > 0.3f && scale.x < 10f)
                {
                    transform.localScale = scale; //����ֵ
                }

                //��ס���µĴ����㣬�´�ʹ��  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }
    }
}