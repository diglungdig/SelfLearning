Shader "Custom/rippleEffectTest" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_Scale ("Scale", float) = 1
		_Speed ("Speed", float) = 1
		_Frequency ("Frequency", float) = 1
		_Cube ("Cubemap", CUBE) = "" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 5.0

		sampler2D _MainTex;
		half4 _Color;
		float _Scale, _Speed, _Frequency;
		samplerCUBE _Cube;


		struct Input {
			float2 uv_MainTex;
			float3 worldRefl;
		};

		void vert(inout appdata_full v){
			half offsetvert = ((v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z));

			half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);

			v.vertex.y = value;
		}


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) *_Color;
			o.Albedo = c.rgb * 0.5;
			o.Emission = texCUBE (_Cube, IN.worldRefl).rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
