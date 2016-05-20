Shader "Chibi Studio/Indexed" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_AccentColor ("Accent Color", Color) = (1,1,1,1)
		_AccentColor2 ("Accent Color 2", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 250

		CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _AccentColor;
			fixed4 _AccentColor2;

			struct Input {
				float2 uv_MainTex;
			};

			void surf (Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				c = _Color*c.r + _AccentColor*c.g + _AccentColor2*c.b;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		ENDCG
	}//	End SubShader

	Fallback "VertexLit"
}//	End shader ChibiGenerator/Indexed
