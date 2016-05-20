Shader "Chibi Studio/Chibi" {
	Properties {
		_Color ("Skin Color", Color) = (1,1,1,1)
		_EyeColor ("Eye Color", Color) = (1,1,1,1)
		_ClothingColor ("Clothing Color", Color) = (1,1,1,1)
		_AccentColor ("Accent Color", Color) = (1,1,1,1)
		_AccentColor2 ("Accent Color 2", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_DecalTex ("Face (RGBA)", 2D) = "black" {}
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 250
		
		CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			sampler2D _DecalTex;
			fixed4 _Color;
			fixed4 _EyeColor;
			fixed4 _ClothingColor;
			fixed4 _AccentColor;
			fixed4 _AccentColor2;

			struct Input {
				float2 uv_MainTex;
				float2 uv_DecalTex;
			};

			void surf (Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				c = _Color*(1-c.a) + _ClothingColor*c.r + _AccentColor*c.g + _AccentColor2*c.b;
				half4 decal = tex2D(_DecalTex, IN.uv_DecalTex*3.5 - 2.33);
				decal *= _EyeColor;
				c.rgb = lerp(c.rgb, decal.rgb, decal.a);
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		ENDCG
	}//End SubShader

	Fallback "Diffuse"
}//	End Shader ChibiGenerator/Chibi
