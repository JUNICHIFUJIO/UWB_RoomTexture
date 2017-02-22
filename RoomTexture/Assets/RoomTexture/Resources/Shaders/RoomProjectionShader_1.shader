Shader "RoomProjectionShader_1" {
	Properties {
		_ProjectionTexture("Texture", 2D) = "" {}

		//_Color ("Color", Color) = (1,1,1,1)
	}
		SubShader{
			Lighting On
			ColorMask RGB
			Cull Back
			ZWrite On
			ZTest LEqual
			Offset -1, -1
		//Blend One DstColor, SrcAlpha DstAlpha
		Blend One DstColor, SrcAlpha DstAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _ProjectionTexture;
	float4x4 unity_Projector;

	struct vIn {
		float4 vertex : POSITION;
		float4 color : COLOR;
	};

	struct v2f {
		float4 uv : TEXCOORD0;
		float4 pos : SV_POSITION;
		float4 uvMax : TEXCOORD1;
		float4 color : COLOR;
		float4 grabPos : TEXCOORD2;
	};

	v2f vert(vIn vertexInput) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, vertexInput.vertex);
		o.uv = mul(unity_Projector, vertexInput.vertex);
		o.color = vertexInput.color;
		//o.uvMax = mul(unity_Projector, _MaxUV);

		return o;
	}



		//fixed4 surf (Input IN, inout SurfaceOutputStandard o) {
		fixed4 frag(v2f i) : SV_Target {

			fixed4 tex = tex2Dproj(_ProjectionTexture, UNITY_PROJ_COORD(i.uv));

			if (i.uv.x > 1
				|| i.uv.y > 1
				|| i.uv.x < 0
				|| i.uv.y < 0) {
				clip(1);
			}

			/*
			if (In.uv_MainTex.x > 1
				|| In.uv_MainTex.y > 1
				|| In.uv_MainTex.x < 0
				|| In.uv_MainTex.y < 0) {
				clip(1);
			}
			*/
			return tex;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
