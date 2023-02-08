using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance {get; private set;}
    public Image mask;
    float originalSize;
    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        //mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        mask.rectTransform.SetSizeWithCurrentAnchors(0, originalSize * value);
        Debug.Log(originalSize); 
    }
}
