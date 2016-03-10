Shader "Custom/Proximity" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {} // Regular object texture 
		_PlayerPosition("Player Position", vector) = (0,0,0,0) // The location of the player - will be set by script
		_VisibleDistance("Visibility Distance", float) = 10.0 // How close does the player have to be to make object visible
		_OutlineWidth("Outline Width", float) = 3.0 // Used to add an outline around visible area a la Mario Galaxy - http://www.youtube.com/watch?v=91raP59am9U
		_OutlineColour("Outline Colour", color) = (1.0,1.0,0.0,1.0) // Colour of the outline
		
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
	}
		SubShader{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

			// exactly the same as in previous shader
		struct v2f {
			float3 worldPos : TEXCOORD0;
			half3 tspace0 : TEXCOORD1;
			half3 tspace1 : TEXCOORD2;
			half3 tspace2 : TEXCOORD3;
			float2 uv : TEXCOORD4;
			float4 pos : SV_POSITION;
		};
		v2f vert(float4 vertex : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float2 uv : TEXCOORD0)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, vertex);
			o.worldPos = mul(_Object2World, vertex).xyz;
			half3 wNormal = UnityObjectToWorldNormal(normal);
			half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
			half tangentSign = tangent.w * unity_WorldTransformParams.w;
			half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
			o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
			o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
			o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
			o.uv = uv;
			return o;
		}

		// textures from shader properties
		sampler2D _MainTex;
		sampler2D _OcclusionMap;
		sampler2D _BumpMap;

		fixed4 frag(v2f i) : SV_Target
		{
			// same as from previous shader...
			half3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));
			half3 worldNormal;
			worldNormal.x = dot(i.tspace0, tnormal);
			worldNormal.y = dot(i.tspace1, tnormal);
			worldNormal.z = dot(i.tspace2, tnormal);
			half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
			half3 worldRefl = reflect(-worldViewDir, worldNormal);
			half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
			half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);
			fixed4 c = 0;
			c.rgb = skyColor;

			// modulate sky color with the base texture, and the occlusion map
			fixed3 baseColor = tex2D(_MainTex, i.uv).rgb;
			fixed occlusion = tex2D(_OcclusionMap, i.uv).r;
			c.rgb *= baseColor;
			c.rgb *= occlusion;

			return c;
		}
			ENDCG
		}
		
		Pass{
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		// Access the shaderlab properties
		uniform sampler2D _MainTex;
	uniform float4 _PlayerPosition;
	uniform float _VisibleDistance;
	uniform float _OutlineWidth;
	uniform fixed4 _OutlineColour;

	// Input to vertex shader
	struct vertexInput {
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD0;
	};
	// Input to fragment shader
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 position_in_world_space : TEXCOORD0;
		float4 tex : TEXCOORD1;
	};

	// VERTEX SHADER
	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
		output.position_in_world_space = mul(_Object2World, input.vertex);
		output.tex = input.texcoord;
		return output;
	}

	// FRAGMENT SHADER
	float4 frag(vertexOutput input) : COLOR
	{
		// Calculate distance to player position
		float dist = distance(input.position_in_world_space, _PlayerPosition);

	// Return appropriate colour
	if (dist < _VisibleDistance) {
		return tex2D(_MainTex, float4(input.tex)); // Visible
	}
	else if (dist < _VisibleDistance + _OutlineWidth) {
		return _OutlineColour; // Edge of visible range
	}
	else {
		float4 tex = tex2D(_MainTex, float4(input.tex)); // Outside visible range
		tex.a = 0;
		return tex;
	}
	}

		ENDCG
	}
	}
		//FallBack "Diffuse"
}