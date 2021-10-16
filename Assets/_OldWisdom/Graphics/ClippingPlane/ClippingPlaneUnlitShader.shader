Shader "MyUnlitShaders/ClippingPlaneUnlitShader" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        
        [HDR]
        _Color("Tint", Color) = (1, 1, 1, 1)

        [HDR]
        _CutoffColor("Cutoff Color", Color) = (1, 1, 1, 1)
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
                Cull Off
                Offset 0, 0

                Stencil {
                }

                ZClip True
                ZTest LEqual
                ZWrite On
                //*/

                HLSLPROGRAM

                //* Pragmas
                #pragma target 4.0
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

                float4 _Color;
                float4 _CutoffColor;

                uniform float4 _PlaneData;

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
                    float3 worldSpacePos: TEXCOORD1;
                };

                FragInputData VertexMain(VertexInputData vertexInputData) {
                    FragInputData fragInputData;

                    fragInputData.pos = UnityObjectToClipPos(vertexInputData.pos);
                    fragInputData.texCoords0 = TRANSFORM_TEX(vertexInputData.texCoords0, _MainTex);
                    fragInputData.worldSpacePos = mul(UNITY_MATRIX_M, vertexInputData.pos);

                    return fragInputData;
                }

                fixed4 FragMain(FragInputData fragInputData, float facing: VFACE): SV_Target {
			        float displacement = dot(fragInputData.worldSpacePos, _PlaneData.xyz);
                    displacement += _PlaneData.w;
			        clip(-displacement);

			        facing = facing * 0.5 + 0.5;
			
			        fixed4 color = tex2D(_MainTex, fragInputData.texCoords0);
			        color *= _Color;
                    color.rgb *= facing;
                    color = lerp(_CutoffColor, color, facing);

                    return color;
                }

                ENDHLSL
            }
        }
    }
}