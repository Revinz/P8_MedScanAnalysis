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

    public SlidersSyncer syncer;

    public bool updating = false;

    /**
     * Syncs this dual slider to the given dual slider values 
     */
    public void sync(DualSlider slider)
    {
        XMax.SliderValue = slider.XMax.SliderValue;
        XMin.SliderValue = slider.XMin.SliderValue;

        YMax.SliderValue = slider.YMax.SliderValue;
        YMin.SliderValue = slider.YMin.SliderValue;

        ZMax.SliderValue = slider.ZMax.SliderValue;
        ZMin.SliderValue = slider.ZMin.SliderValue;
    }

    public void updateAxisMaxSlider(PinchSlider maxSlider, PinchSlider minSlider)
    {
        //We are checking if are already updating
        //otherwise it will go into an infinite loop
        //When overshooting the other slider
        if (updating)
            return;
        updating = true;

        if (maxSlider.SliderValue <= minSlider.SliderValue)
        {
            maxSlider.SliderValue = minSlider.SliderValue;
        }
        syncer.Sync(this);

        updating = false;
    }

    public void updateAxisMinSlider(PinchSlider maxSlider, PinchSlider minSlider)
    {
        if (updating)
            return;
        updating = true;

        if (maxSlider.SliderValue <= minSlider.SliderValue)
        {
            minSlider.SliderValue = maxSlider.SliderValue;
        }
        syncer.Sync(this);

        updating = false;
    }

    public void YMaxUp()
    {
        updateAxisMaxSlider(YMax, YMin);
        syncer.Sync(this);
    }


    public void ZMaxUp()
    {
        updateAxisMaxSlider(ZMax, ZMin);
        syncer.Sync(this);
    }


    public void XMaxUp()
    {
        updateAxisMaxSlider(XMax, XMin);
        syncer.Sync(this);
    }



    public void XMinUp()
    {
        updateAxisMinSlider(XMax, XMin);
        syncer.Sync(this);
    }

    public void YMinUp()
    {
        updateAxisMinSlider(YMax, YMin);
        syncer.Sync(this);
    }


    public void ZMinUp()
    {
        updateAxisMinSlider(ZMax, ZMin);
        syncer.Sync(this);
    }



    public void FrontUp()
    {
        syncer.Sync(this);
    }


    public void BackUp()
    {
        syncer.Sync(this);
    }




}
