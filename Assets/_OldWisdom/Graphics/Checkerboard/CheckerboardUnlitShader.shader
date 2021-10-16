Shader "MyUnlitShaders/CheckerboardUnlitShader" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _OddColor("Odd Color", Color) = (1, 1, 1, 1)
        _EvenColor("Even Color", Color) = (0, 0, 0, 1)
        _ScaleFactor("Scale Factor", Float) = 0.25

        [HideInInspector]
        _StencilComp("Stencil Comparison", Float) = 8

        [HideInInspector]
        _Stencil("Stencil ID", Float) = 0

        [HideInInspector]
        _StencilOp("Stencil Operation", Float) = 0

        [HideInInspector]
        _StencilWriteMask("Stencil Write Mask", Float) = 255

        [HideInInspector]
        _StencilReadMask("Stencil Read Mask", Float) = 255

        [HideInInspector]
        _ColorMask("Color Mask", Float) = 15
    }

    Category {
        //* ShaderLab commands to set render state for all Passes in each SubShader in Category
        //*/

        SubShader {
            Tags {
                "RenderPipeline" = "UniversalRenderPipeline"
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
                "ForceNoShadowCasting" = "False"
                "DisableBatching" = "False"
                "IgnoreProjector" = "False"
                "PreviewType" = "Sphere"
                "CanUseSpriteAtlas" = "True"
                "UniversalMaterialType" = "Lit"
            }

            LOD 200

            //* ShaderLab commands to...
            //*/

            HLSLINCLUDE
            ENDHLSL

            Pass {
                Name "MyPass"
                Tags {
                    "LightMode" = "UniversalForwardOnly"
                }

                //* ShaderLab commands to...
                AlphaToMask Off
                Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
                BlendOp Add
                ColorMask RGBA
                Conservative False
                Cull Back
                Offset 0, 0

                Stencil {
                }

                ZClip True
                ZTest LEqual
                ZWrite On
                //*/

                HLSLPROGRAM

                //* Pragmas
                #pragma target 4.5
                #pragma exclude_renderers d3d11_9x
                #pragma vertex VertexMain
                #pragma fragment FragMain
                #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
                //*/
                
                //* Includes
                #include "UnityCG.cginc"
                #include "UnityLightingCommon.cginc"
                #include "AutoLight.cginc"
                //*/

                //* Defines
                //*/

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float4 _OddColor;
                float4 _EvenColor;
                float _ScaleFactor;

                #if SHADER_TARGET >= 45
                #endif

                struct VertexInputData {
                    float4 pos: POSITION;
                    float4 color: COLOR;
                    float4 texCoords0: TEXCOORD0;
                    float4 texCoords1: TEXCOORD1;
                    float4 texCoords2: TEXCOORD2;
                    float4 texCoords3: TEXCOORD3;
                    float3 normal: NORMAL;
                    float4 tangent: TANGENT;
                };

                struct FragInputData {
                    float4 pos: SV_POSITION;
                    float3 texCoords0: TEXCOORD0;
                };

                FragInputData VertexMain(VertexInputData vertexInputData) {
                    FragInputData fragInputData;

                    fragInputData.pos = UnityObjectToClipPos(vertexInputData.pos);
                    fragInputData.texCoords0 = mul(unity_ObjectToWorld, vertexInputData.pos); //world pos

                    return fragInputData;
                }

                fixed4 FragMain(FragInputData fragInputData): SV_Target {
                    float3 adjustedWorldPos = floor(fragInputData.texCoords0 * _ScaleFactor);

                    float lerpFactor = adjustedWorldPos.x + adjustedWorldPos.y + adjustedWorldPos.z;
                    lerpFactor = frac(lerpFactor * 0.5) * 2.0f; //frac(...) leads to 0 for even and 0.5 for odd

                    return tex2D(_MainTex, fragInputData.texCoords0) * lerp(_OddColor, _EvenColor, lerpFactor);
                }

                ENDHLSL
            }
        }
    }
}