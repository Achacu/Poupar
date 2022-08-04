Shader "Custom/FadeByDstGrassShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _UpperBlindTh ("Upper Blind Threshold", Range(0,20)) = 5.0        
        _LowerBlindTh ("Lower Blind Threshold", Range(0,20)) = 5.0
    }
    SubShader
    {
	Tags { "Queue"="Transparent" "Rendering"="Transparent" "IgnoreProjector" = "True"}

    //    Pass {
    //         ZWrite On
    //         ColorMask 0
    //     }
	// ZWrite Off
	//Blend SrcAlpha OneMinusSrcAlpha
    //Blend One One
    CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
       // #pragma vertex vert
        #pragma surface surf Lambert alpha:fade 
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 2.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            half3 worldPos;
        };

        fixed4 _Color;
        fixed _LowerBlindTh;
        fixed _UpperBlindTh;

        // void vert (inout appdata_full v, out Input o) {
        //   //v.vertex.xyz += v.normal;
        //   UNITY_INITIALIZE_OUTPUT(Input,o);
        //   o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        // }
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            float dstToCam = length(IN.worldPos.xyz - _WorldSpaceCameraPos.xyz);//length(ObjSpaceViewDir(v.vertex)); //distance to camera

            //If _Visible is non-negative, that will be the alpha (set from outside: i.e. objects that make sound)
            //If the object is closer to cam than _LowerBlindTh it is completely visible; if between both thresholds, a gradient; if above _UpperBlindTh, completely invisible 
            o.Alpha = (dstToCam > _UpperBlindTh)? 0 : (dstToCam < _LowerBlindTh)? 1
            : (_UpperBlindTh - dstToCam)/(_UpperBlindTh - _LowerBlindTh);
            
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
