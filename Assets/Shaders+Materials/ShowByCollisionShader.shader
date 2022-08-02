Shader "Custom/ShowByCollisionShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Normalmap", 2D) = "bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _UpperBlindTh ("Upper Blind Threshold", Range(0,20)) = 5.0        
        _LowerBlindTh ("Lower Blind Threshold", Range(0,20)) = 5.0
        _Colliding ("Colliding", Range(0,1)) = 0
        _Sounding ("Sounding", Range(0,1)) = 0
        //_ColPos ("Collision Position", Vector) = (0,0,0,0)
        _ColAreaRadius ("Collision Area Radius", Range(0,5)) = 1        
    }
    SubShader
    {
	Tags { "Queue"="Transparent+2"   "Rendering"="Transparent" "IgnoreProjector" = "True"}

       Pass {
            ZWrite On
            ColorMask 0
        }
	ZWrite Off
    //Cull off
	//Blend SrcAlpha OneMinusSrcAlpha
    //Blend One One
    CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma vertex vert
        #pragma surface surf Standard fullforwardshadows alpha:fade 
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MetallicGlossMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MetallicGlossMap;
            //float3 posWorld;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed _LowerBlindTh;
        fixed _UpperBlindTh;

        fixed _Colliding;
        fixed _Sounding;
        fixed _ColAreaRadius;

        fixed4 _ColPoints[15];

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

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            
            fixed dstToHitPoint = 0;
            bool closeToColPos = false; 

            //The loop is aborted when there's no collision or the current point is within reach of a colPoint.
            for(int i = 0; (_Colliding != 0) && (i < 15) && !closeToColPos; i++) {

                //1st check avoid calculating distance to null positions
                dstToHitPoint = (_ColPoints[i].xyz == float3(0,0,0))? 100 : distance(_ColPoints[i], IN.worldPos);
                closeToColPos = (dstToHitPoint < _ColAreaRadius);
            }
            o.Alpha = closeToColPos? (max(_Colliding, _Sounding) * c.a) : (_Sounding * c.a);
            
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = tex2D (_MetallicGlossMap, IN.uv_MetallicGlossMap).r * _Metallic;
            o.Smoothness = tex2D (_MetallicGlossMap, IN.uv_MetallicGlossMap).a * _Glossiness;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            //o.Alpha = IN.color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
