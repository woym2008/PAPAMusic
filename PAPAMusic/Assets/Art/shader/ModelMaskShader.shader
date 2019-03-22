Shader "Unlit/ModelMaskShader"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_Mask("Base (RGB)", 2D) = "white" {}

		_Color("Tint", Color) = (1,1,1,1)
		_Stencil("Stencil ID", Float) = 0

		_Specular("Specular", Color) = (1,1,1,1)
		_Gloss("Gloss", Range(1.0, 255)) = 20
    }

    SubShader
    {
        

		Pass
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}
			Stencil
			{
				Ref[_Stencil]
				Comp Always
				Pass Replace
			}

			//Cull Off
			Lighting Off
			ZWrite Off

			LOD 100

        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color    : COLOR;
			};
			fixed4 _Color;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				float4 color    : COLOR;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _Mask;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.color = v.color * _Color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 mask = tex2D(_Mask, i.uv);
				clip(0.5 - mask.r);
				//if (mask.r > 0.5)
				//{
				//	discard;
				//}
                return fixed4(0,0,0,0);
            }
            ENDCG
        }
		Pass
		{
			//定义Tags
			Tags
			{ 
				"LightMode" = "ForwardBase"
				"RenderType" = "Opaque"
			}
			Stencil
			{
				Ref 1
				Comp NotEqual
			}

			Lighting On

			CGPROGRAM
			//引入头文件
#include "Lighting.cginc"
			//定义Properties中的变量
			fixed4 _Diffuse;
			fixed4 _Specular;
			float _Gloss;

			//定义结构体：应用阶段到vertex shader阶段的数据
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			//定义结构体：vertex shader阶段输出的内容
			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float2 uv : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Mask;
			//定义顶点shader
			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				//把法线转化到世界空间
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);

				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 mask = tex2D(_Mask, i.uv);
				//clip(0.5f - mask.a);

				//环境光
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Diffuse;
				//归一化光方向
				fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
				//再次归一化worldNorml
				fixed3 worldNormal = normalize(i.worldNormal);
				//diffuse
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal, worldLight));
				//计算反射方向R,worldLight表示光源方向（指向光源），入射光线方向为-worldLight，通过reflect函数（入射方向，法线方向）获得反射方向
				fixed3 reflectDir = normalize(reflect(-worldLight, worldNormal));
				//计算该像素对应位置（顶点计算过后传给像素经过插值后）的观察向量V，相机坐标-像素位置
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				//计算高光值，高光值与反射光方向与观察方向的夹角有关，夹角为dot（R,V），最后根据反射系数计算的反射值为pow（dot（R,V），Gloss）
				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0.0,dot(reflectDir, viewDir)), _Gloss);
				//冯氏模型：Diffuse + Ambient +　Specular
				fixed3 colorlight = diffuse + ambient + specular;
				fixed4 color = tex2D(_MainTex, i.uv);
				
				return fixed4(colorlight * color.rgb, 1.0);
			}

				//使用vert函数和frag函数
#pragma vertex vert
#pragma fragment frag	

				ENDCG
		}
    }
	FallBack "Diffuse"
}
