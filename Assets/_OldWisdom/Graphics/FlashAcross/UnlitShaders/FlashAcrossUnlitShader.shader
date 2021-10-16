Shader "MyUnlitShaders/FlashAcrossUnlitShader" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}

        [MaterialToggle]
        _ShldUseTex("ShldUseTex", Int) = 0

        [HDR]
        _Color("Color", Color) = (1, 1, 1, 1)
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
                uniform int _ShldUseTex;
                uniform fixed4 _Color;

                uniform float coord;
                uniform float thickness;
                uniform fixed4 color;
                uniform int dir;

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
                };

                FragInputData VertexMain(VertexInputData vertexInputData) {
                    FragInputData fragInputData;

                    fragInputData.pos = UnityObjectToClipPos(vertexInputData.pos);
                    fragInputData.texCoords0 = TRANSFORM_TEX(vertexInputData.texCoords0, _MainTex);

                    return fragInputData;
                }

                fixed4 FragMain(FragInputData fragInputData): SV_Target {
                    fixed4 myColor = _ShldUseTex == 1 ? tex2D(_MainTex, fragInputData.texCoords0) : _Color;
                    
                    float texCoord = dir < 2 ? fragInputData.texCoords0.x : fragInputData.texCoords0.y;

                    if(texCoord >= coord && texCoord <= coord + thickness) {
                        color.a = 0.0f;
                        myColor += color;
                    }

                    return myColor;
                }

                ENDHLSL
            }
        }
    }
}