using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityVolumeRendering;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

public class ModelClippingHandler : MonoBehaviour
{
    public bool flag=true;
    public Material[] _mat;
    [Header("Shader properties")]
    public ClippingShaderProps props;

    [Header("Volume Dataset Options")]
    public bool usePremadeTextures = false;
    public bool createNewTextures = true;
    public string NewTextureFileName = "Thoracic Spine Fracture";
    /*
    [Header("Slider Objects")]
    public MinMaxSlider sliderX;
    public MinMaxSlider sliderY;
    public MinMaxSlider sliderZ;
    */
    private List<ModelMesh> ModelMeshes = new List<ModelMesh>();

    private void createModelObject(VolumeDataset volumeDataset) {
        props = new ClippingShaderProps();
        GameObject model = GameObject.CreatePrimitive(PrimitiveType.Cube);
        model.transform.parent = this.transform;
        model.GetComponent<MeshRenderer>().materials = _mat;

        model.AddComponent<ModelMesh>();
        ModelMesh newModel = model.GetComponent<ModelMesh>();
        newModel.dataset = volumeDataset;
        ModelMeshes.Add(newModel);
        newModel.SetupMesh();
        model.transform.Rotate(new Vector3(-90, 180, 0));
        model.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        model.transform.localPosition = new Vector3(0f, 0.1f, 0f);
    }

    private void Start()
    {
        //string path = @"D:\Projects\Spine_HardTissue"; //Nikos
        string path = @"C:\Users\Revinz\Desktop\backup2\school\MED8\Dicom3DModel\VitreaDVD\DICOM\ST00001\SE00001 - Copy (2)"; //Patrick
        string newDCMPath = @"C:\Users\Revinz\Desktop\DICOM-fracture-thoracic\Series1";

        VolumeDataset dataset;
        if (!usePremadeTextures)
        {
            dataset = new DICOMLoader().LoadFolder(newDCMPath);
            if (createNewTextures)
            {
                NewTextureFileName = NewTextureFileName.Trim();
                AssetDatabase.CreateAsset(dataset.GetDataTexture(), "Assets/ShaderTextures/" + NewTextureFileName + "_volume.asset");
                AssetDatabase.CreateAsset(dataset.GetGradientTexture(), "Assets/ShaderTextures/" + NewTextureFileName + "_gradient.asset");
            }

        }
        else
        {
            dataset = GetComponent<VolumeDataset>();
;       }
        createModelObject(dataset);
        // UpdateXValues();
        // UpdateYValues();
        // UpdateZValues();
    }
    // Update is called once per frame
    void Update()
    {
        //updateObjectBoundsInfo();
        updateShaderProperties();
    }

    // Gets all the renderes and sets up the material property block
    // which allows for seperate instance values for the shader properties
    // void SetupShaders() {
    //     ModelMeshes = new List<ModelMesh>();

    //     foreach (Transform child in transform) {
    //        ModelMesh mesh = child.GetComponent<ModelMesh>();
    //        mesh.SetupMesh();
    //        ModelMeshes.Add(mesh);
    //     }  

    //     //Allows for the model being 1 big model instead of multiple meshes
    //     ModelMesh rootModelMesh = GetComponent<ModelMesh>();
    //     if (rootModelMesh != null) {
    //         rootModelMesh.SetupMesh();
    //         ModelMeshes.Add(rootModelMesh);
    //     }
    // }
    /*
    public void UpdateXValues()
    {
        props.ClipMin.x = sliderX.minValue/sliderX.maxLimit;
        props.ClipMax.x = sliderX.maxValue/sliderX.maxLimit;

    }
    public void UpdateYValues()
    {
        props.ClipMin.y = sliderY.minValue / sliderY.maxLimit;
        props.ClipMax.y = sliderY.maxValue / sliderY.maxLimit;
    }
    public void UpdateZValues()
    {
        props.ClipMin.z = sliderZ.minValue / sliderZ.maxLimit;
        props.ClipMax.z = sliderZ.maxValue / sliderZ.maxLimit;
    }
    */


    // // Finds the max and min bounderies for the model.
    // void updateObjectBoundsInfo() {
    //     Vector3 tmpMax = ModelMeshes[0].rend.bounds.max;
    //     Vector3 tmpMin = ModelMeshes[0].rend.bounds.min;;
    //     foreach (ModelMesh mesh in ModelMeshes)
    //     {   
    //         //Compare and get max bounds
    //         tmpMax.x = Mathf.Max(tmpMax.x, mesh.rend.bounds.max.x);
    //         tmpMax.y = Mathf.Max(tmpMax.y, mesh.rend.bounds.max.y);
    //         tmpMax.z = Mathf.Max(tmpMax.z, mesh.rend.bounds.max.z);

    //         //Compare and get min bounds
    //         tmpMin.x = Mathf.Min(tmpMin.x, mesh.rend.bounds.min.x);
    //         tmpMin.y = Mathf.Min(tmpMin.y, mesh.rend.bounds.min.y);
    //         tmpMin.z = Mathf.Min(tmpMin.z, mesh.rend.bounds.min.z);
    //     }

    //     boundsMax = tmpMax;
    //     boundsMin = tmpMin;
    // }

    //Updates all the clipping shaders in the gameobject tree with the model
    void updateShaderProperties() {
      //  if (flag == true)

       // {
            //Debug.Log(ModelMeshes.Count);
            foreach (ModelMesh mesh in ModelMeshes)
            {
                mesh.UpdateShaderProperties(props);
            }
      //  }
     //   flag = false;
    }

    public void ClipXMax(PinchSlider sliderXMax)
    {
        props.ClipMax.x = sliderXMax.SliderValue;
        

    }
    public void ClipXMin(PinchSlider sliderXMin)
    {
        props.ClipMin.x = sliderXMin.SliderValue;

    }
    public void ClipYMax(PinchSlider sliderYMax)
    {
        props.ClipMax.z = sliderYMax.SliderValue;

    }
    public void ClipYMin(PinchSlider sliderYMin)
    {
        props.ClipMin.z = sliderYMin.SliderValue;

    }
    public void ClipZMax (PinchSlider sliderZMax)
    {
        props.ClipMax.y = sliderZMax.SliderValue;

    }
    public void ClipZMin(PinchSlider sliderZMin)
    {
        props.ClipMin.y = sliderZMin.SliderValue;

    }
    public void ClipDarknesFront (PinchSlider sliderFront)
    {
        props.darkeningAmountFront = sliderFront.SliderValue;

    }
    public void ClipDarknessBack(PinchSlider sliderBack)
    {
        props.darkeningAmountBack = sliderBack.SliderValue;

    }

    public void Visualization(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                {
                    props.renderMode = ClippingShaderProps.RenderingMode.XRAY;
                    break;
                }
            case 1:
                {
                    props.renderMode = ClippingShaderProps.RenderingMode.SURFACE;
                    break;
                }
           
        }
    }

   

    public void changeQuality(TMP_Dropdown dropdown)
    {
        float temp;        
        if (float.TryParse(dropdown.options[dropdown.value].text, out temp))
        {
            props.Quality = temp;
        }

        
    }
}


