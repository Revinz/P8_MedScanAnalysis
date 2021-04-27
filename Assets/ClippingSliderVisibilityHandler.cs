using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * 
 * Handles the visibility of the model's sliders around it, which controls the clipping for each axis.
 * 
 */

public class ClippingSliderVisibilityHandler : MonoBehaviour
{

    public GameObject[] XSliders;
    public GameObject[] YSliders;
    public GameObject[] ZSliders;

    void FixedUpdate()
    {
        UpdateAxisSlidersVisibility(XSliders);
        UpdateAxisSlidersVisibility(YSliders);
        UpdateAxisSlidersVisibility(ZSliders);
    }

    void UpdateAxisSlidersVisibility(GameObject[] sliders)
    {
        //Find closest slider
        GameObject closestSlider = sliders[0];
        float shortestDist = 999999f;
        Debug.Log("Cam pos: " + Camera.main.transform.position);
        foreach (GameObject slider in sliders)
        {
            float dist = Vector3.Distance(Camera.main.transform.position, slider.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                closestSlider = slider;
            }
            Debug.Log(slider + " dist: " + dist);
        }
        Debug.Log("closest: " + closestSlider);
        //Hide other slides
        foreach (GameObject slider in sliders)
        {
            if (slider != closestSlider)
                slider.SetActive(false);
        }
        closestSlider.SetActive(true);

    }
}
