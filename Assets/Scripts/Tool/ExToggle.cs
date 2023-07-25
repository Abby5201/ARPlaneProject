using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExToggle : Toggle
{
    [SerializeField]
    public Color textNormalColor;
    public Color textSelectColor;
    public Color imageSelectColor;
    public bool useImageColor;

    private Text text;
    private Image graphicImg;
    private Image img;

    protected override void Start()
    {
        base.Start();
        text = GetComponentInChildren<Text>();
        img = image;
        onValueChanged.AddListener((ison) =>
        {
            if (ison)
            {
                if(text!=null)
                {
                    text.color = textSelectColor;
                }
                if(useImageColor)
                {
                    img.color = imageSelectColor;
                }
            }
            else
            {
                if (text != null)
                {
                    text.color = textNormalColor;
                }

                if (useImageColor)
                {
                    img.color = Color.white;
                }
            }
        });
    }
 
    
}
