using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * 
 * Synchronizes the values of the given sliders
 * 
 * */
public class SlidersSyncer : MonoBehaviour
{
    public DualSlider[] sliders;
    private bool isSyncing = false;
    /*
     * Syncs the other slider to the given slider
     */
    public void Sync(DualSlider dualSlider)
    {
        //We have to check if we are already syncing
        //to prevent infinite looping the syncing process
        if (isSyncing)
            return;
        isSyncing = true;
        foreach (DualSlider slider in sliders)
        {
            if (slider != dualSlider)
                slider.sync(dualSlider);
        }
        isSyncing = false;
    }
}
