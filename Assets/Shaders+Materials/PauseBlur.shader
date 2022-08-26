Shader "Custom/PauseBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pow("Power", Int) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        GrabPass { "_GrabTex" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grabPos = ComputeGrabScreenPos(UnityObjectToClipPos(v.vertex));
                return o;
            }
            half _Pow;
            sampler2D _MainTex;
            sampler2D _GrabTex;
            float4 _GrabTex_TexelSize;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = (0,0,0,0);//tex2Dproj(_GrabTex, i.grabPos));
                //i.grabPos: top left (0,0) -> bottom right (1,1)
                //TexelSize: 1.0/(texture size). pixels -> texels

                //  for(int dx = -_Pow; dx < _Pow; dx ++)
                //  for(int dy = -_Pow; dy < _Pow; dy ++) {
                //          col += tex2Dproj(_GrabTex, i.grabPos + float4(dx*_GrabTex_TexelSize.x,dy*_GrabTex_TexelSize.y,0,0));
                //  }
                fixed4 grabPos = i.grabPos;
                #define ADDBLUR(kernel2,weight) tex2Dproj(_GrabTex, grabPos + float4(kernel2.x*_GrabTex_TexelSize.x,kernel2.y*_GrabTex_TexelSize.y,0,0))*weight; 
                //#define const corners3x3 1


                //corners 3x3
                col += ADDBLUR(float2(-1,1), 16);
                col += ADDBLUR(float2(-1,-1), 16);
                col += ADDBLUR(float2(1,-1), 16);
                col += ADDBLUR(float2(1,1), 16);

                //sides 3x3
                col += ADDBLUR(float2(-1,0), 26);
                col += ADDBLUR(float2(1,0), 26);
                col += ADDBLUR(float2(0,-1), 26);
                col += ADDBLUR(float2(0,1), 26);

                //middle
                col += ADDBLUR(float2(0,0), 41);

                //corners 5x5
                col += ADDBLUR(float2(-2,2), 1);
                col += ADDBLUR(float2(-2,-2), 1);
                col += ADDBLUR(float2(2,-2), 1);
                col += ADDBLUR(float2(2,2), 1);

                //sides 5x5
                col += ADDBLUR(float2(-2,0), 7);
                col += ADDBLUR(float2(2,0), 7);
                col += ADDBLUR(float2(0,-2), 7);
                col += ADDBLUR(float2(0,2), 7);

                //remaining
                col += ADDBLUR(float2(-2,1), 4);
                col += ADDBLUR(float2(-2,-1), 4);
                col += ADDBLUR(float2(2,-1), 4);
                col += ADDBLUR(float2(2,1), 4);
                col += ADDBLUR(float2(-1,-2), 4);
                col += ADDBLUR(float2(1,2), 4);
                col += ADDBLUR(float2(-1,2), 4);
                col += ADDBLUR(float2(1,-2), 4);

                col /= 273;//(2*_Pow+1)*(2*_Pow+1);                


                return col;
            }
            ENDCG
        }
    }
}
