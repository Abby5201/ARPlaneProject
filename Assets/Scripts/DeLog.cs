using System.Collections;
using System.Collections.Generic;
using LeeFramework.Console;
using UnityEngine;

public class DeLog : MonoBehaviour
{
    public static void Log(object obj)
    {
        Debug.Log(obj);
        
    }

    public static void LogError(object obj)
    {
        Debug.LogError(obj);
    }
    
}
