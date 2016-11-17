Shader "Unlit" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("face", 2D) = "white"
		_MainTexB("pupils", 2D) = "white" 
		_MainTexC("halfClosed", 2D) = "white"
		_MainTexD("brow", 2D) = "white"
		//_EyeWhites("eye whites", Range(0,10)) = 1

	_Illum("Illumin (A)", 2D) = "white" {}
	_EmissionLM("Emission (Lightmapper)", Float) = 0
	}

		SubShader{
		Tags{ "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
		LOD 200
		Lighting On Cull Off ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma surface surf Lambert 
		sampler2D _MainTexB;
	sampler2D _MainTexC;
	sampler2D _MainTexD;
	//sampler2D _EyeWhites;

		sampler2D _MainTex;
	sampler2D _Illum;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
		float2 uv_MainTexB;
		float2 uv_MainTexC;
		float2 uv_MainTexD;
		//float2 uv_EyeWhites;

		float2 uv2_Illum; // Originally float2 uv_Illum;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		//fixed4 c = tex * _Color;
		//fixed4 c = (max(tex2D(_MainTexC, IN.uv_MainTex).a, tex2D(_MainTex, IN.uv_MainTex))) + tex2D(_MainTexC, IN.uv_MainTex);
		//fixed4 c = tex2D(_MainTexC, IN.uv_MainTexC).a;

		fixed4 c = _Color*tex2D(_MainTex, IN.uv_MainTex)*(1-tex2D(_MainTexC, IN.uv_MainTexC).a)*(1 - tex2D(_MainTexB, IN.uv_MainTexB).a)*(1 - tex2D(_MainTexD, IN.uv_MainTexD).a)
			+ (tex2D(_MainTexD, IN.uv_MainTexD).a*tex2D(_MainTexD, IN.uv_MainTexD)) + _Color*(tex2D(_MainTexC, IN.uv_MainTexC).a*tex2D(_MainTexC, IN.uv_MainTexC))*(1 - tex2D(_MainTexD, IN.uv_MainTexD).a) + (tex2D(_MainTexB, IN.uv_MainTexB).a*tex2D(_MainTexB, IN.uv_MainTexB));
		//half4 c = tex2D(_MainTex, IN.uv_MainTex);
	 		c.a = 1;
		o.Albedo = c.rgb;

		//o.Albedo = _Color;
		o.Alpha = 1;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
