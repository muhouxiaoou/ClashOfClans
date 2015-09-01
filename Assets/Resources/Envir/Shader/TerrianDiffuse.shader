Shader "Custom/TerrianDiffuse" {
	Properties {
		_Color("Main Color",Color)=(1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}	
	}
	Category
	{
	Offset -2,-2
	ZWrite Off
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent-1"}
		//精细化区分
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)*_Color;
			o.Albedo = c.rgb;//环境光
			o.Alpha = c.a;
		}
		ENDCG
	}
	}
	FallBack "VertexLit"
}
