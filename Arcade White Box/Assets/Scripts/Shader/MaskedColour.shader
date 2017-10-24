Shader "Custom/MaskedColour" {
	Properties{
		_Color("ColorMask", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_MaskTex("Mask (RGB)", 2D) = "white" {}
	_Normal("Normal", 2D) = "bump" {}
	_Metallic("Metallic/Roughness/AO (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

		sampler2D _MainTex, _MaskTex, _Metallic, _Normal;
	fixed3 _Color;

	struct Input {
		float2 uv_MainTex;
		float2 uv_MaskTex;
		float2 uv_Metallic;
		float2 uv_Normal;
	};

	/*half _Glossiness;*/
	//half _Metallic;

	void surf(Input IN, inout SurfaceOutputStandard o) {

		float4 c = tex2D(_MainTex, IN.uv_MainTex);
		float mask = tex2D(_MaskTex, IN.uv_MainTex).r;
		float3 mro = tex2D(_Metallic, IN.uv_Metallic);
		float4 n = tex2D(_Normal, IN.uv_Normal);
		c.rgb = c.rgb * (1 - mask) + _Color * mask;


		o.Albedo = c.rgb;
		o.Normal = UnpackNormal(n);
		// Metallic and smoothness come from slider variables
		o.Metallic = mro.r; //_Metallic;
		o.Smoothness = mro.g; //_Glossiness;
		o.Occlusion = mro.b;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
