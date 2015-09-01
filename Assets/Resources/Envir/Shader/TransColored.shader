Shader "Custom/TransColored" {
	Properties {
		_Speed("Speed",float)=8
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "black" {}
	}
	SubShader {
		LOD 100
		Tags { 
		"RenderType"="Transparent" 
		"IgnoreProjector"="True"
		"Queue"="Transparent"
		}
		//不需要剔除
		Cull Off
		Lighting Off
		ZWrite Off
		Fog{Mode Off}
		Offset -1,-1
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Speed;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex=mul(UNITY_MATRIX_MVP,v.vertex);
				o.texcoord=v.texcoord;
				o.color=v.color;
				return o;
			}
			fixed4 frag(v2f i):COLOR
			{
				fixed4 col=tex2D(_MainTex,i.texcoord)*i.color;

				float3 div=abs(cos(_Speed*_Time.x))*_Color.rgb;
				col.rgb+=div;
				return col;
			}

			ENDCG
		}
	}

	SubShader
	{
		LOD 100
		Tags
		{
			"RenderType"="Transparent" 
			"IgnoreProjector"="True"
			"Queue"="Transparent"
		}
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog{Mode Off}
			Offset -1,-1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			SetTexture[_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}


	FallBack "Diffuse"
}
