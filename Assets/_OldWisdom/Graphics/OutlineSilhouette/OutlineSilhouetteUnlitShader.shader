Shader "MyUnlitShaders/OutlineSilhouetteUnlitShader" {
    SubShader {
        Pass {
            Name "OutlineFill"
            Tags {
                "LightMode" = "UniversalForward"
            }

            //* ShaderLab commands to...
            AlphaToMask Off
            Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
            BlendOp Add
            ColorMask RGB
            Conservative False
            Cull Off
            Offset 0, 0

            Stencil {
                Ref 1
                Comp NotEqual
            }

            ZClip True
            ZTest [_ZTestFill]
            ZWrite Off
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
                float3 texCoords3: TEXCOORD3;
                float3 normal: NORMAL;
                float4 tangent: TANGENT;
            };

            struct FragInputData {
                float4 pos: SV_POSITION;
                float2 texCoords0: TEXCOORD0;
            };

            FragInputData VertexMain(VertexInputData vertexInputData) {
                FragInputData fragInputData;

                float3 normal = any(vertexInputData.texCoords3) ? vertexInputData.texCoords3 : vertexInputData.normal;
                float3 viewPos = UnityObjectToViewPos(vertexInputData.pos);
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));

                fragInputData.pos = UnityViewToClipPos(viewPos + viewNormal * -viewPos.z * _OutlineThickness / 1000.0f);
                fragInputData.texCoords0 = TRANSFORM_TEX(vertexInputData.texCoords0, _MainTex);

                return fragInputData;
            }

            fixed4 FragMain(FragInputData fragInputData): SV_Target {
                return _OutlineSilhouetteShldUseTex == 1 ? tex2D(_MainTex, fragInputData.texCoords0) : _OutlineSilhouetteColor;
            }

            ENDHLSL
        }

        Pass {
            Name "OutlineMask"
            Tags {
            }

            //* ShaderLab commands to...
            AlphaToMask Off
            Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
            BlendOp Add
            ColorMask 0
            Conservative False
            Cull Off
            Offset 0, 0

            Stencil {
                Ref 1
                Pass Replace
            }

            ZClip True
            ZTest [_ZTestMask]
            ZWrite Off
            //*/
        }
    }
}