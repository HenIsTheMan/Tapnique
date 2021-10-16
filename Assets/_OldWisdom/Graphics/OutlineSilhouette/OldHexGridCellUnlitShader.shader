Shader "MyUnlitShaders/OldHexGridCellUnlitShader" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}

        [MaterialToggle]
        _ShldUseTex("ShldUseTex", Int) = 0

        [HDR]
        _Color("Color", Color) = (1, 1, 1, 1)

        [HideInInspector]
        _ZTestFill("ZTestFill", Float) = 8

        [HideInInspector]
        _ZTestMask("ZTestMask", Float) = 8

        [MaterialToggle]
        _OutlineSilhouetteShldUseTex("OutlineSilhouetteShldUseTex", Int) = 0

        [HDR]
        _OutlineSilhouetteColor("OutlineSilhouetteColor", Color) = (1, 1, 1, 1)

        [HideInInspector]
        _OutlineThickness("OutlineThickness", Float) = 40
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

                CBUFFER_START(UnityPerMaterial)
                sampler2D _MainTex;
                float4 _MainTex_ST;
                uniform int _ShldUseTex;
                uniform fixed4 _Color;
                uniform int _OutlineSilhouetteShldUseTex;
                uniform float _OutlineThickness;
                uniform fixed4 _OutlineSilhouetteColor;
                CBUFFER_END

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
                    float3 texCoords1: TEXCOORD1;
                    float3 texCoords2: TEXCOORD2;
                };

                FragInputData VertexMain(VertexInputData vertexInputData) {
                    FragInputData fragInputData;

                    fragInputData.pos = UnityObjectToClipPos(vertexInputData.pos);
                    fragInputData.texCoords0 = TRANSFORM_TEX(vertexInputData.texCoords0, _MainTex);

                    float3 val = saturate(dot(vertexInputData.normal, _WorldSpaceLightPos0.xyz));
                    fragInputData.texCoords1 = ShadeSH9(float4(vertexInputData.normal, 1.0f)); //Ambient
                    fragInputData.texCoords2 = (val * _LightColor0.rgb); //Diffuse

                    return fragInputData;
                }

                fixed4 FragMain(FragInputData fragInputData): SV_Target {
                    float3 lighting = fragInputData.texCoords1 + fragInputData.texCoords2;
                    fixed4 myColor = _ShldUseTex == 1 ? tex2D(_MainTex, fragInputData.texCoords0) : _Color;

                    return fixed4(myColor.rgb * lighting, myColor.a);
                }

                ENDHLSL
            }

            UsePass "MyUnlitShaders/OutlineSilhouetteUnlitShader/OutlineMask"

            UsePass "MyUnlitShaders/OutlineSilhouetteUnlitShader/OutlineFill"
        }
    }
}