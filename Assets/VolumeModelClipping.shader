// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Volume/Test" {
    Properties {
        _Volume ("Volume Texture (generated)", 3D) = "" {}
        _Gradient ("Gradient Texture (generated)", 3D) = "" {}
        _DarkeningThresholdFront ("Front Darkening Amount", Range(0, 1)) = 0
        _DarkeningThresholdBack ("Back Darkening Amount", Range(0, 1)) = 1
        _ClipMax ("Model Render End (user-adjusted)", Float) =  (1, 1, 1)
        _ClipMin ("Model Render Start (user-adjusted)", Float) =  (0, 0, 0) 
        _renderingMode ("Rendering Mode", Float) = 1
        _NumSteps ("Number of Steps for volume rendering", Float) = 512
    }
SubShader {

    Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
    LOD 100
    Cull Front
    ZTest LEqual
    ZWrite Off    
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        
        //Properties for the model
        sampler3D _Volume;
        sampler3D _Gradient;
        float _Radius;
        float3 _ClipMax;
        float3 _ClipMin;
        float _DarkeningThresholdFront;
        float _DarkeningThresholdBack;
        float _renderingMode; //1 = Maximum Density, 2 = ISOSurface

        //properties for the volume rendering
        float _NumSteps;
        float _outlineThickness; //1 = Maximum Density, 2 = ISOSurface
        sampler2D _CameraDepthTexture;

        #include "UnityCG.cginc"
        
            struct vs_input {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float4 normal : NORMAL;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;    // Not used. See vert method for more info.
                float3 vertexLocal : TEXCOORD1; //vertex position in object space
                float2 uv : TEXCOORD0;
                //float3 normal : NORMAL;
            };
            
            
            v2f vert (vs_input v)
            {
                v2f o;
                o.uv = v.uv;
                //o.normal = v.normal;
                o.vertexLocal = v.vertex;
                o.pos = UnityObjectToClipPos(v.vertex); //Not sure why removing this breaks the shader - it is not used.
                return o;
            }
            
            float3 getDensity(float3 pos) {
                return tex3Dlod(_Volume, float4(pos.x, pos.y, pos.z, 0.0f)).rgb;
            }   

            float3 getGradient(float3 pos) {
                return tex3Dlod(_Gradient, float4(pos.x, pos.y, pos.z, 0.0f)).rgb;
            }


            //Maximum Density Approach
            float4 maximumDensity(float3 rayStartPos, float3 rayDir, float stepSize) {
                float maxDensity = 0.0f;
                for (uint iStep = 0; iStep < _NumSteps; iStep++) {
                    const float t = iStep * stepSize; //Distance covered by the ray for the current iter
                    const float3 currPos = rayStartPos + rayDir * t; //location in object space

                    //Check if the currPos is inside the area that should be rendered, otherwise continue.
                    //Simply disregard this position's value and move on to the next.
                    if (currPos.x < _ClipMin.x || currPos.x >= _ClipMax.x || currPos.y < _ClipMin.y || currPos.y >= _ClipMax.y || currPos.z < _ClipMin.z || currPos.z >= _ClipMax.z )
                        continue;
                    
                    const float density = getDensity(currPos);
                    maxDensity = max(density, maxDensity);
                }
                
                // //Might be possible to use clip() instead.
                if (maxDensity <= _DarkeningThresholdFront || maxDensity >= _DarkeningThresholdBack)
                    maxDensity = 0.0f;  

                float4 col = float4(1.0f, 1.0f, 1.0f, maxDensity);
                return col;
            }   

            float4 ISOSurfaceRendering(float3 rayStartPos, float3 rayDir, float stepSize) {
                    float density = 0.0f;
                    float3 currPos;
        
                    //Start from the end and then go towards the vertex
                    // - why start from the end? because we render the back face not the front face
                    rayStartPos += rayDir;
                    rayDir = -rayDir;

                    //rayStartPos += 2.0f * rayDir / _NumSteps;
                    for (uint iStep = 0; iStep < _NumSteps; iStep++) {
                        const float t = iStep * stepSize; //Distance covered by the ray for the current iter
                        currPos = rayStartPos + rayDir * t; //location in object space

                        //Check if the currPos is inside the area that should be rendered, otherwise continue.
                        //Simply disregard this position's value and move on to the next.
                        if (currPos.x < _ClipMin.x || currPos.x >= _ClipMax.x || currPos.y < _ClipMin.y || currPos.y >= _ClipMax.y || currPos.z < _ClipMin.z || currPos.z >= _ClipMax.z )
                            continue;
                        
                        density = getDensity(currPos);
                        if (density >= _DarkeningThresholdFront + 0.001 && density <= _DarkeningThresholdBack - 0.001) //small offset to prevent rendering the cube's sides
                            break;
                    }
                    
                    //Might be possible to use clip() instead.
                    if (density <= _DarkeningThresholdFront || density >= _DarkeningThresholdBack)
                        return float4(0.0f, 0.0f, 0.0f, 0.0f);

                    // ---- Lighting to make it not seem flat ---
                    // Gradient
                    // Compare neighbour values and find the gradient to determine the normal of the pixel
                    const float3 normal = normalize(getGradient(currPos));
                    const float3 lightDir = normalize(-rayDir); 
                    float lightReflection = abs(dot(normal, lightDir));

                    //Change the colors since black = transparent on the HoloLens
                    //Also more soft color to allow for extended usage
                    const float4 shadowColor = (1 - lightReflection) * float4(0.46, 0.27, 0.13, 1) * 0.5;

                    //Color + extra contrast for the lighter areas
                    const float4 modelColor = float4(0.46, 0.27, 0.13, 1) + float4(lightReflection, lightReflection, lightReflection, 0) * 0.4;

    
                    return lightReflection * modelColor + shadowColor;
            }
        
            float4 frag (v2f i) : COLOR
            {
                // stepSize is (non-normalized) rayDir vector  / _NumSteps. 
                // Helps preventing duplicate and inverted rendering of the volume.
                // --- Also determines how close to the camera we want to render the volume / where the clipping should start at the camera.
                const float stepSize = 1.0f / _NumSteps; 

                //vertex pos starts at -0.5 instead of 0 in object space.
                float3 rayStartPos = i.vertexLocal + float3(0.5f, 0.5f, 0.5f);
                // Must not be normalized, otherwise it renders duplicate and inverted when inside the volume
                // -- Because when inside the rayDir magnitute is less than 1
                // -- and when it becomes normalized the rayDir overshoots the camera and thus creates duplicate rendering.
                // -- and because there are 4 back surfaces when inside the volume, there will be 2 of them behind the camera and thus inverted.
                float3 rayDir = ObjSpaceViewDir(float4(i.vertexLocal, 0.0f)); 


                if (_renderingMode == 1) 
                    return maximumDensity(rayStartPos, rayDir, stepSize);
                else //if not 1 must be 2 - also used as fallback.
                    return ISOSurfaceRendering(rayStartPos, rayDir, stepSize);

            }
        
        ENDCG
        
        }
    }

}
 