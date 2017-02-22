///
/// Simple shader that paints the entire room a solid color. This makes it 
/// easy to project textures on them additively without lighting having an explicit effect
///
Shader "RoomCanvasShader"
{
	Properties
	{
		_BaseColor("Base color", Color) = (1.0, 1.0, 1.0, 1.0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Background"}

		Lighting On
		Blend One One

		Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

			// Target Unity 5.0+ (Hololens capable)
	#pragma target 5.0
	#pragma only_renderers d3d11

	#include "UnityCG.cginc"

			float4 _BaseColor;
	
			struct v2f
			{
				float4 tex : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
	
			float4 frag(v2f i) : COLOR
			{
				float4 color = _BaseColor;

				return color;
			}
			ENDCG
		}
	}
}
