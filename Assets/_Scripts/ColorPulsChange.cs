using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ColorPulsManager : MonoBehaviour
{
    // config
    [Header("Config")]
    public bool colorPulsActive;
    public bool changeCameraBG;
    public bool changeUIOverlay;
    public List<Color> colorList = new List<Color>();
    public float pulsSpeed = 1.0f;
    // reference
    [Header("References")]
    [SerializeField]
    private Camera cameraTOColor;
    [SerializeField]
    private Image imageOverlay;
    // tweening
    private Sequence colorPulsSequence;
    void Start()
    {
        if (!colorPulsActive)
            return;
        colorPulsSequence = DOTween.Sequence();
        if (changeCameraBG)
        {
            imageOverlay.enabled = false;
            cameraTOColor.backgroundColor = colorList[colorList.Count-1];
            foreach (Color colorValue in colorList)
            {
                Tween changeTOColor_Tween = cameraTOColor.DOColor(colorValue, pulsSpeed);
                colorPulsSequence.Append(changeTOColor_Tween);
            }
            colorPulsSequence.SetLoops(-1, LoopType.Yoyo);
        }
        else if (changeUIOverlay)
        {
            imageOverlay.enabled = true;
            imageOverlay.color = colorList[colorList.Count - 1];
            foreach (Color colorValue in colorList)
            {
                Tween changeTOColor_Tween = imageOverlay.DOColor(colorValue, pulsSpeed);
                colorPulsSequence.Append(changeTOColor_Tween);
            }
            colorPulsSequence.SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            Debug.LogWarning("Color Puls Manager active but no change mode is selected!");
        }
    }
}