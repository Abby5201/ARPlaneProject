
using UnityEngine;
using UnityEngine.EventSystems;

public class Global : MonoBehaviour
{
   public static string name = "Engine";
   public static bool CanInsObj = false;

   
   /// <summary>
   /// 判断是否点击到UI上
   /// </summary>
   /// <returns></returns>
   public static bool IsTouchUI()
   {
      if (EventSystem.current == null) return false;
      if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
      {
         if (Input.touchCount < 1) return false;
         if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return true;
      }
      else
      {
         if (EventSystem.current.IsPointerOverGameObject()) return true;
      }

      return false;
   }
   
}
