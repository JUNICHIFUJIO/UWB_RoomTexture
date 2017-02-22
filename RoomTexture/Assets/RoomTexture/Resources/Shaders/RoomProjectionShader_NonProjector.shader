Shader "RoomProjectionShader_NonProjector"
{
	Properties
	{

		_TextureArray("Texture Array", 2DArray) = "" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#include "UnityCG.cginc"
			#define MAX_SIZE 30

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldSpace : TEXCOORD0;
				float4 vertexInProjectionSpace : TEXCOORD1;
			};

			float4x4 _MyObjectToWorld;
			float4x4 _WorldToCameraMatrixArray[MAX_SIZE];
			float4x4 _CameraProjectionMatrixArray[MAX_SIZE];
			float4 _VertexInProjectionSpaceArray[MAX_SIZE];
			float4 _VertexPositionInCameraSpaceArray[MAX_SIZE];
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldSpace = mul(_MyObjectToWorld, float4(v.vertex.xyz, 1));
				return o;
			}

			// Bring in the texture array to use
			UNITY_DECLARE_TEX2DARRAY(_TextureArray);

			fixed4 frag (v2f info) : SV_Target
			{
				// Calculate uv, world to camera to image

				for (int i = 0; i < MAX_SIZE; i++) {
					_VertexPositionInCameraSpaceArray[i] = mul(_WorldToCameraMatrixArray[i], float4(info.worldSpace.xyz, 1));
					_VertexInProjectionSpaceArray[i] = mul(_CameraProjectionMatrixArray[i], float4(_VertexPositionInCameraSpaceArray[i]));
				}

				for (int i = 0; i < MAX_SIZE; i++) {
					if (_VertexPositionInCameraSpaceArray[i].z > 0)
						continue;

					float2 projectedTex = (_VertexInProjectionSpaceArray[i].xy / _VertexInProjectionSpaceArray[i].w);
					if (abs(projectedTex.x) < 1.0 && abs(projectedTex.y) < 1.0) {
						float2 unitTexCoord = (projectedTex * 0.5) + float4(0.5, 0.5, 0, 0);

						// Remap the uv
						if ((unitTexCoord.x < (1024.0 / 1280.0)) && (unitTexCoord.y >(208.0 / 720.0))) {
							unitTexCoord.x = unitTexCoord.x * (1280.0 / 1024.0);
							unitTexCoord.y = unitTexCoord.y * (720.0 / 512.0) - (208.0 / 512.0);

							float3 texIndex = { unitTexCoord.x, unitTexCoord.y, i };

							return UNITY_SAMPLE_TEX2DARRAY(_TextureArray, texIndex);
						}
						else
							continue;
					}
				}

				return fixed4(0, 0, 0, 0);
			}
			ENDCG
		}
	}
}