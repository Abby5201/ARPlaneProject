using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public GameObject LiangLongAudio;


    private GameObject currentAudio;

    private void Start()
    {
        if (instance == null)
            instance = this;
        EventManager.instance.AddListener(Event.PlayAudioEvent,OnPrePlayAudio);
    }

    public void StopAudio()
    {
        // switch (name)
        // {
        //     case "LiangLong":
        //         LiangLongAudio.SetActive(false);
        //         break;
        // }
        
        // currentAudio.SetActive(true);
        var audio =currentAudio.GetComponentsInChildren<AudioSource>();
        foreach (var a in audio)
        {
            a.Stop();
        }
    }


     void OnPrePlayAudio(object obj)
     {
         Debug.Log("OnPrePlayAudio:"+obj.ToString());
         string name = obj.ToString();
         if (name.Equals("LiangLong"))
         {
             currentAudio = LiangLongAudio;
             if (currentAudio != null)
             {
                 PlayAudio();
             }
         }
         else
         {
             if(currentAudio==null)
                 return;
             // LiangLongAudio.SetActive(false);
             var audio =currentAudio.GetComponentsInChildren<AudioSource>();
             foreach (var a in audio)
             {
                 a.Stop();
             }
         }

        
         
     }

     void PlayAudio()
     {
        // currentAudio.SetActive(true);
         var audio =currentAudio.GetComponentsInChildren<AudioSource>();
         foreach (var a in audio)
         {
             a.Play();
         }
     }
     
     
     
     


     private void OnDestroy()
     {
         EventManager.instance.RemoveListener(Event.PlayAudioEvent,OnPrePlayAudio);
     }
}
