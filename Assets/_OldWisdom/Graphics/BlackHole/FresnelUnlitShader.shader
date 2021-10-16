Shader "MyUnlitShaders/FresnelUnlitShader" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        
        [HDR]
        _Color("Tint", Color) = (0, 0, 0, 1)
        
        [HDR]
        _FresnelColor("Fresnel Color", Color) = (1, 1, 1, 1)

        [PowerSlider(4)]
        _FresnelExponent("Fresnel Pow", Range(0.25, 4)) = 1
    }

    Category {
        //* ShaderLab commands to set render state for all Passes in each SubShader in Category
        //*/

        SubShader {
            Tags {
                "RenderPipeline" = "UniversalRenderPipeline"
                "Queue" = "Geometry"
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

                fixed4 _Color;
                float3 _FresnelColor;
                float _FresnelExponent;

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
                    float2 texCoords0: TEXCOORD0;
                    float3 viewDir: TEXCOORD1;
                    float3 worldNormal: NORMAL;
                };

                FragInputData VertexMain(VertexInputData vertexInputData) {
                    FragInputData fragInputData;

                    fragInputData.pos = UnityObjectToClipPos(vertexInputData.pos);
                    fragInputData.texCoords0 = TRANSFORM_TEX(vertexInputData.texCoords0, _MainTex);

                    fragInputData.worldNormal = normalize(vertexInputData.normal);
                    fragInputData.viewDir = normalize(WorldSpaceViewDir(vertexInputData.pos));

                    return fragInputData;
                }

                fixed4 FragMain(FragInputData fragInputData): SV_Target {
                    fixed4 color = tex2D(_MainTex, fragInputData.texCoords0);
                    color *= _Color;

                    float fresnel = dot(fragInputData.worldNormal, fragInputData.viewDir);
                    fresnel = saturate(1.0f - fresnel); //invert fresnel so big vals are on the outside
                    fresnel = pow(fresnel, _FresnelExponent);
                    float3 fresnelColor = fresnel * _FresnelColor;

                    return fixed4(color.rgb + fresnelColor, 1.0f);
                }

                ENDHLSL
            }
        }
    }
}