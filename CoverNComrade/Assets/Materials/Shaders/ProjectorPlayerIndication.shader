Shader "Projector/PlayerIndication"
{
	Properties
	{
		_Color("Color", Color) = (1,0,0,1)
		_Texture("Cookie", 2D) = "White"
		_ShapeScales("Shape Scales", Vector) = (1, 0, 0, 0)
	}
	Subshader
	{
		Tags{ "RenderType" = "Opaque" }
		Pass
		{
			ZWrite Off
			ColorMask RGB
			Blend DstColor Zero

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				struct v2f {
					float4 uv : TEXCOORD0;
					UNITY_FOG_COORDS(2)
					float4 pos : SV_POSITION;
				};

				float4x4 _Projector;

				v2f vert(float4 vertex : POSITION)
				{
					v2f o;
					o.pos = mul(UNITY_MATRIX_MVP, vertex);
					o.uv = mul(_Projector, vertex);
					UNITY_TRANSFER_FOG(o,o.pos);
					return o;
				}

				sampler2D _Texture;
				fixed4 _Color;
				fixed4 _ShapeScales;

				fixed4 frag(v2f i) : SV_Target
				{
					// Add scaled cookies
					float4 uv = i.uv;
					uv.xy = ((uv.xy - .5) / _ShapeScales.x) + 0.5;
					fixed4 col = tex2Dproj(_Texture, UNITY_PROJ_COORD(uv) );
					uv.xy = ((i.uv.xy - .5) / _ShapeScales.y) + 0.5;
					col += tex2Dproj(_Texture, UNITY_PROJ_COORD(uv));
					uv.xy = ((i.uv.xy - .5) / _ShapeScales.z) + 0.5;
					col += tex2Dproj(_Texture, UNITY_PROJ_COORD(uv));
					uv.xy = ((i.uv.xy - .5) / _ShapeScales.w) + 0.5;
					col += tex2Dproj(_Texture, UNITY_PROJ_COORD(uv));

					col.x = clamp(col.x, 0, 1);
					col.y = clamp(col.y, 0, 1);
					col.z = clamp(col.z, 0, 1);
					col.w = clamp(col.w, 0, 1);

					// Colorize
					col.x *= _Color.x;
					col.y *= _Color.y;
					col.z *= _Color.z;
					col.w *= _Color.w;

					// Clip
					clip(col.a - .5);

					UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(1,1,1,1));
					return col;
				}
			ENDCG
		}
	}
}
