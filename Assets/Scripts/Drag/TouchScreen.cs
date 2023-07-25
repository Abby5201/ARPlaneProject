using UnityEngine;

public class TouchScreen : MonoBehaviour
{
    private Touch oldTouch1; //上次触摸点1(手指1)  
    private Touch oldTouch2; //上次触摸点2(手指2)  

    Touch newTouch1;
    Touch newTouch2;

    private float rotateSpeed = 0.3f;


    // Update is called once per frame 
    void Update()
    {
        //没有触摸  
        if (Input.touchCount <= 0)
        {
            return;
        }

        

        //单点拖动
        //else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //{
        //    //
        //    var deltaposition = Input.GetTouch(0).deltaPosition;


        //    transform.Translate(new Vector3(deltaposition.x * 0.01f, deltaposition.y * 0.01f, 0), Space.World);
        //    Debug.Log("pos:" + transform.position);
        //}
        //TODO:是否点击到ui上添加判断
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var deltaPos = Input.GetTouch(0).deltaPosition;
            
            transform.Rotate(-Vector3.up * deltaPos.x * rotateSpeed, Space.World);
        }
        //双指旋转  && 缩放
        else if (2 <= Input.touchCount)
        {
            //Debug.Log("单点触摸， 水平上下旋转  ");
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition; //位置增量

            // if (Mathf.Abs(deltaPos.x) >= 3 || Mathf.Abs(deltaPos.y) >= 3)
            // {
            //     transform.Rotate(-transform.up * deltaPos.x * 0.1f, Space.World); //绕y轴旋转
            //     // transform.Rotate(transform.right * deltaPos.y*0.1f, Space.World);//绕x轴
            // }

            //缩放
            newTouch1 = Input.GetTouch(0); //
            newTouch2 = Input.GetTouch(1);
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }

            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position); //计算两点距离
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position); //计算两点距离
            float offset = newDistance - oldDistance;


            if (Mathf.Abs(offset) >= 3)
            {
                // Debug.Log(offset);
                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / 100f;
                Vector3 localScale = transform.localScale;
                Vector3 scale = new Vector3(localScale.x + scaleFactor,
                    localScale.y + scaleFactor,
                    localScale.z + scaleFactor);

                //最小缩放到 0.3 倍 最大3倍 
                if (scale.x > 0.3f && scale.x < 10f)
                {
                    transform.localScale = scale; //赋新值
                }

                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }
    }
}