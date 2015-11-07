Shader "Custom/Trail" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			ZWrite On
			ZTest Always
			Lighting Off

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				fixed4 _Color;

				float4 vert(float4 v:POSITION) : SV_POSITION
				{
					return mul(UNITY_MATRIX_MVP, v);
				}
				fixed4 frag() : SV_Target 
				{
					//clip(_EnableSeethrough == 0 ? -1 : 1);
					return _Color;
				}
			ENDCG
		}

	}
	FallBack "Diffuse"
}
