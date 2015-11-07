Shader "Custom/GroundUI" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EnableSeethrough ("See through objects", Int) = 1
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

			ZWrite Off
			ZTest Greater
			Lighting Off

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
	            #pragma fragment frag
	            #include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _Color;
	
				struct appdata {
        		    float4 vertex : POSITION;
        		    float4 texcoord : TEXCOORD0;
        		};

	            struct v2f {
	                float4 pos : SV_POSITION;
	                float2 texCoord : TEXCOORD0;
	            };
	
	            v2f vert (appdata v)
	            {
	                v2f o;
	                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	                o.texCoord = v.texcoord;
	                return o;
	            }
	
	            float4 frag (v2f i) : SV_Target
	            {
	            	float4 col = tex2D(_MainTex, i.texCoord);
	            	clip(col.a < 0.5f?-1:1);
	                return col;
	            }
			ENDCG
		}
	}
	FallBack "Diffuse"
}
