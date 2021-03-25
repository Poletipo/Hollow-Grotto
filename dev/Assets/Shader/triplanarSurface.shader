Shader "Custom/triplanarSurface"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_TopTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_Tiling("Tiling", Float) = 1.0
		_Blend("Blend", range(0,100)) = 1.0

	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard vertex:vert fullforwardshadows addshadow

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _TopTex;

			struct Input
			{
				float2 uv_MainTex;
				float3 coords;
				half3 objNormal;
			};

			float _Tiling;
			void  vert(inout appdata_full v, out Input data)
			{
				UNITY_INITIALIZE_OUTPUT(Input, data);
				data.coords = v.vertex.xyz * _Tiling;
				data.objNormal = v.normal.xyz;
			}


			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float _Blend;


			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{

				// use absolute value of normal as texture weights
				half3 blend = pow(abs(IN.objNormal), _Blend);
				// make sure the weights sum up to 1 (divide by sum of x+y+z)
				blend /= dot(blend, 1);
				// read the three texture projections, for x,y,z axes
				fixed4 cx = tex2D(_MainTex, IN.coords.yz);
				fixed4 cy = tex2D(_TopTex, IN.coords.xz);
				fixed4 cz = tex2D(_MainTex, IN.coords.xy);
				// blend the textures based on weights
				fixed4 c = (cx * blend.x + cy * blend.y + cz * blend.z) * _Color;




				// Albedo comes from a texture tinted by color
				//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
				ENDCG
		}
			FallBack "Diffuse"
}
