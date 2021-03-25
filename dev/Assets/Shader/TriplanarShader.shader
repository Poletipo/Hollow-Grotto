Shader "Unlit/TriplanarShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TopTex("Texture", 2D) = "white" {}
		_Tiling("Tiling", Float) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_Blend("Blend", range(0,100)) = 1.0
	}
		SubShader
		{
			Tags {"LightMode" = "ForwardBase"}
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc" // for UnityObjectToWorldNormal
				#include "UnityLightingCommon.cginc" // for _LightColor0

				#include "Lighting.cginc"

				// compile shader into multiple variants, with and without shadows
				// (we don't care about any lightmaps yet, so skip these variants)
				#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
				// shadow helper functions and macros
				#include "AutoLight.cginc"



				struct v2f
				{
					half3 objNormal : TEXCOORD0;
					float3 coords : TEXCOORD1;
					float2 uv : TEXCOORD2;
					float4 pos : SV_POSITION;
					fixed4 diff : COLOR0;
				};

				float _Tiling;
				float _Blend;

				v2f vert(float4 pos : POSITION, float3 normal : NORMAL, float2 uv : TEXCOORD0)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(pos);
					o.coords = pos.xyz * _Tiling;
					o.objNormal = normal;
					o.uv = uv;

					half3 worldNormal = UnityObjectToWorldNormal(normal);
					half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
					o.diff = nl * _LightColor0;
					o.diff.rgb += ShadeSH9(half4(worldNormal, 1));
					// compute shadows data
					TRANSFER_SHADOW(o)
					return o;
				}

				sampler2D _MainTex;
				sampler2D _TopTex;
				sampler2D _OcclusionMap;

				fixed4 frag(v2f i) : SV_Target
				{
					// use absolute value of normal as texture weights
					half3 blend = pow(abs(i.objNormal),_Blend);
					// make sure the weights sum up to 1 (divide by sum of x+y+z)
					blend /= dot(blend,1);
					// read the three texture projections, for x,y,z axes
					fixed4 cx = tex2D(_MainTex, i.coords.yz);
					fixed4 cy = tex2D(_TopTex, i.coords.xz);
					fixed4 cz = tex2D(_MainTex, i.coords.xy);
					// blend the textures based on weights
					fixed4 c = cx * blend.x + cy * blend.y + cz * blend.z;
					// modulate by regular occlusion map
					c *= tex2D(_OcclusionMap, i.uv);
					c *= i.diff;
					return c;
				}
				ENDCG
			}
			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
}
