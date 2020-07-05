// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|normal-2260-RGB,emission-7241-RGB,custl-2260-RGB,alpha-6412-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32321,y:32690,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:2260,x:32321,y:32868,ptovrint:False,ptlb:NoiseTexture,ptin:_NoiseTexture,varname:node_2260,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-8536-OUT;n:type:ShaderForge.SFN_TexCoord,id:7300,x:31529,y:32743,varname:node_7300,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:5721,x:31768,y:32743,varname:node_5721,prsc:2,spu:1,spv:0|UVIN-7300-UVOUT,DIST-4410-OUT;n:type:ShaderForge.SFN_Time,id:5576,x:31471,y:32920,varname:node_5576,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:3094,x:31889,y:32905,varname:node_3094,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-5721-UVOUT;n:type:ShaderForge.SFN_RemapRange,id:8374,x:32090,y:32827,varname:node_8374,prsc:2,frmn:0,frmx:1,tomn:-0.01,tomx:0.01|IN-3094-OUT;n:type:ShaderForge.SFN_Add,id:8536,x:32214,y:33058,varname:node_8536,prsc:2|A-8374-OUT,B-4181-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:4181,x:31952,y:33102,varname:node_4181,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:4410,x:31669,y:32947,varname:node_4410,prsc:2|A-5576-T,B-6544-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6544,x:31452,y:33091,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_6544,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:100;n:type:ShaderForge.SFN_Vector1,id:6412,x:32474,y:33048,varname:node_6412,prsc:2,v1:0.5;proporder:7241-2260-6544;pass:END;sub:END;*/

Shader "Shader Forge/LaserEffect" {
    Properties {
        _Color ("Color", Color) = (1,0,0,1)
        _NoiseTexture ("NoiseTexture", 2D) = "white" {}
        _Speed ("Speed", Float ) = 100
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _NoiseTexture; uniform float4 _NoiseTexture_ST;
            uniform float _Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float4 node_5576 = _Time;
                float2 node_8536 = (((i.uv0+(node_5576.g*_Speed)*float2(1,0)).rg*0.02+-0.01)+i.uv0);
                float4 _NoiseTexture_var = tex2D(_NoiseTexture,TRANSFORM_TEX(node_8536, _NoiseTexture));
                float3 normalLocal = _NoiseTexture_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
////// Lighting:
////// Emissive:
                float3 emissive = _Color.rgb;
                float3 finalColor = emissive + _NoiseTexture_var.rgb;
                return fixed4(finalColor,0.5);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
