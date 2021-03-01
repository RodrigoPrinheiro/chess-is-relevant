Shader "Stylized/DitheringShadowShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [HDR] _Emission ("Emission", Color) = (0,0,0)
        
        [Header(Lighting Parameters)]
        [IntRange]_Steps("Light Steps", Range(1, 16)) = 2
        _StepSize ("Step Size", Range(0, 1)) = 0.5
        _Halftone ("Shadow Lookup", 2D) = "white" {}
        _HalftoneSize ("Shadow Lookup Size", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Stepped fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _LightingRamp;
        sampler2D _Halftone;
        float4 _Halftone_ST;
        float _StepSize;
        float _Steps;
        float _HalftoneSize;

        struct SteppedSurfaceOutput
        {
            fixed3 Albedo;
            float2 ScreenPos;
            half3 Emission;
            fixed Alpha;
            fixed3 Normal;
        };

        float4 LightingStepped(SteppedSurfaceOutput s, float3 lightDir, half3 viewDir, float atten)
        {
            float towardsLight = dot(s.Normal, lightDir);
            towardsLight = towardsLight / _StepSize;
            float lightIntensity = ceil(towardsLight);
            lightIntensity = lightIntensity / _Steps;
            lightIntensity = saturate(lightIntensity);
            float halftoneValue = tex2D(_Halftone, s.ScreenPos).r;

            lightIntensity = halftoneValue * lightIntensity;

            float attenChange = fwidth(atten);
            float shadow = smoothstep(0, attenChange, atten);

            float4 color;
            color.rgb = s.Albedo * lightIntensity * shadow * _LightColor0.rgb;
            color.a = s.Alpha;
            
            return color;
        }

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        fixed4 _Color;
        fixed4 _Emission;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input i, inout SteppedSurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, i.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            o.Emission = _Emission;

            float aspect = _ScreenParams.x / _ScreenParams.y; // Aspect ratio of the screen
            o.ScreenPos = i.screenPos.xy / i.screenPos.w;
            o.ScreenPos = TRANSFORM_TEX(o.ScreenPos, _Halftone) * _HalftoneSize;
            o.ScreenPos.x = o.ScreenPos.x * aspect;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
