Shader "Custom/URP_Hair"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map (Albedo)", 2D) = "white" {}
        [MainColor] _BaseColor("Color Tint", Color) = (1,1,1,1)
        _MaskMap("Alpha Mask (Greyscale)", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        
        [HideInInspector] _Cull("__cull", Float) = 0.0 // 0 is Both, 2 is Back
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "TransparentCutout" 
            "Queue" = "AlphaTest"
            "RenderPipeline" = "UniversalPipeline" 
        }

        // We use 'Cull Off' because eyelashes are usually single-sided planes 
        // and we need to see them from both sides.
        Cull Off
        ZWrite On

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Required to support URP features
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_MaskMap);
            SAMPLER(sampler_MaskMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half _Cutoff;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 1. Get the color and the mask
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half mask = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, IN.uv).r;

                // 2. Combine textures with the tint
                half4 finalColor = texColor * _BaseColor;

                // 3. ALPHA CLIPPING
                // This is the "Mask" logic. If the mask value is below _Cutoff,
                // the pixel is deleted.
                clip(mask - _Cutoff);

                return finalColor;
            }
            ENDHLSL
        }
    }
}