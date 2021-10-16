Shader "MyUnlitShaders/InstancingWithColorUnlitShader" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
    }

    Category {
        //* ShaderLab commands to set render state for all Passes in each SubShader in Category
        //*/

        SubShader {
            Tags {
                "RenderPipeline" = "UniversalRenderPipeline"
                "Queue" = "Background"
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
                float instanceCount;

                #if SHADER_TARGET >= 45
                    StructuredBuffer<float4> posStructuredBuffer;
                    StructuredBuffer<float3> colorStructuredBuffer;
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
                    float4 color: COLOR;
                    float4 texCoords0: TEXCOORD0;
                    float3 texCoords1: TEXCOORD1;
                    float3 texCoords2: TEXCOORD2;
                };

                void Rotate2D(inout float2 v, float r) {
                    float s, c;
                    sincos(r, s, c);
                    v = float2(v.x * c - v.y * s, v.x * s + v.y * c);
                }

                FragInputData VertexMain(VertexInputData vertexInputData, uint instanceID: SV_InstanceID) {
                    #if SHADER_TARGET >= 45
                        float4 data = posStructuredBuffer[instanceID];
                    #else
                        float4 data = vertexInputData.pos;
                    #endif

                    float rotation = data.w * data.w * _Time.x
                        * (float(instanceID) < float(instanceCount) * 0.5f ? 0.4f : -0.4f)
                        * float(instanceID + 1u) / float(instanceCount);
                    Rotate2D(data.xz, rotation);

                    float3 localPos = vertexInputData.pos.xyz * data.w;
                    float3 worldPos = data.xyz + localPos;

                    FragInputData fragInputData;

                    fragInputData.pos = mul(UNITY_MATRIX_VP, float4(worldPos, 1.0f));

                    #if SHADER_TARGET >= 45
                        fragInputData.color = float4(colorStructuredBuffer[instanceID], 1.0f);
                    #else
                        fragInputData.color = vertexInputData.color;
                    #endif

                    fragInputData.texCoords0 = vertexInputData.texCoords0;

                    float3 val = saturate(dot(vertexInputData.normal, _WorldSpaceLightPos0.xyz));
                    fragInputData.texCoords1 = ShadeSH9(float4(vertexInputData.normal, 1.0f)); //Ambient
                    fragInputData.texCoords2 = (val * _LightColor0.rgb); //Diffuse

                    return fragInputData;
                }

                fixed4 FragMain(FragInputData fragInputData): SV_Target {
                    fixed4 albedo = tex2D(_MainTex, fragInputData.texCoords0);

                    float3 lighting = fragInputData.texCoords1 + fragInputData.texCoords2;

                    return fixed4(albedo.rgb * fragInputData.color.rgb * lighting, albedo.w);
                }

                ENDHLSL
            }
        }
    }
}