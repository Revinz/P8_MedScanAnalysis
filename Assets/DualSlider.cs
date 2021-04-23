using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Microsoft.MixedReality.Toolkit.UI;
public class DualSlider : MonoBehaviour
{
    public PinchSlider XMax;
    public PinchSlider YMax;
    public PinchSlider ZMax;
    public PinchSlider XMin;
    public PinchSlider YMin;
    public PinchSlider ZMin;
    public PinchSlider Back;
    public PinchSlider Front;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void YMaxUp()
    {
        if (YMax.SliderValue <= YMin.SliderValue)
        {
            YMax.SliderValue = YMin.SliderValue;
        }
    }


    public void ZMaxUp()
    {
        if (ZMax.SliderValue <= ZMin.SliderValue)
        {
            ZMax.SliderValue = ZMin.SliderValue;
        }
    }


    public void XMaxUp()
    {
        if (XMax.SliderValue <= XMin.SliderValue)
        {
            XMax.SliderValue = XMin.SliderValue;
        }
    }



    public void XMinUp()
    {
        if (XMax.SliderValue <= XMin.SliderValue)
        {
            XMin.SliderValue = XMax.SliderValue;
        }
    }

    public void YMinUp()
    {
        if (YMax.SliderValue <= YMin.SliderValue)
        {
            YMin.SliderValue = YMax.SliderValue;
        }
    }


    public void ZMinUp()
    {
        if (ZMax.SliderValue <= ZMin.SliderValue)
        {
            ZMin.SliderValue = ZMax.SliderValue;
        }
    }



    public void FrontUp()
    {
        if (Front.SliderValue <= Back.SliderValue)
        {
            Front.SliderValue = Back.SliderValue;
        }
    }


    public void BackUp()
    {
        if (Front.SliderValue <= Back.SliderValue)
        {
            Back.SliderValue = Front.SliderValue;
        }
    }





}
