Shader "FallGuys/Loading Background"
{
       Properties
       {
           [HideInInspector] ScreenRatio ("Screen Ratio", Vector) = (1, 0.563, 0, 0)
           
           RingsAmount ("Rings Amount", Range(1, 5)) = 3.5
           RingSpeed ("Ring Speed", float) = 0.2
           
           CircleSize1 ("1st Circle Size", float) = 0.067
           CircleSize2 ("2nd Circle Size", float) = 0.15
           CircleSize3 ("3rd Circle Size", float) = 0.02
           CircleSize4 ("4th Circle Size", float) = 0.1
           
           Gap12 ("Gap between 1st and 2nd", float) = 0.025
           Gap23 ("Gap between 2nd and 3rd", float) = 0.055
           Gap34 ("Gap between 3rd and 4th", float) = 0.03
           
           FastCircleSize("Fast Circle Size", float) = 0.033
           
           BGColor ("Background Color", Color) = (0.168, 0.332, 0.73, 1)
           SlowColor ("Slow Ring Color", Color) = (0.227, 0.459, 0.839, 1)
           FastColor ("Fast Ring Color", Color) = (0.334, 0.715, 0.969, 1)
           BottomColor ("Bottom Color", Color) = (0.445, 0.584, 0.815, 1)
           
           BottomGradientCoverage ("Bottom Gradient Coverage", float) = 3.0
           BottomGradientIntensity ("Bottom Gradient Intensity", float) = 2.0
       }

       SubShader
       {
           Pass
           {
               CGPROGRAM

               #pragma vertex vert
               #pragma fragment frag Lambert alpha
               #pragma multi_compile_fog
               #include "UnityCG.cginc"

               struct appdata
               {
                   float4 vertex : POSITION;
                   float2 uv : TEXCOORD0;
               };
               struct v2f
               {
                   float2 uv : TEXCOORD0;
                   UNITY_FOG_COORDS(1)
                   float4 vertex : SV_POSITION;
               };

               float4 ScreenRatio;
               
               float RingsAmount;
               float RingSpeed;
               
               float CircleSize1;
               float CircleSize2;
               float CircleSize3;
               float CircleSize4;
               
               float Gap12;
               float Gap23;
               float Gap34;
               
               float FastCircleSize;
               
               float4 BGColor;
               float4 SlowColor;
               float4 FastColor;
               float4 BottomColor;
               
               float BottomGradientCoverage;
               float BottomGradientIntensity;
               
               v2f vert(appdata v)
               {
                   v2f o;
                   o.vertex = UnityObjectToClipPos(v.vertex);
                   o.uv = v.uv;

                   UNITY_TRANSFER_FOG(o,o.vertex);
                   return o;
               }

               fixed4 frag(v2f i) : SV_Target
               {
                    // radial
                    float2 texCoord = (i.uv - float2(0.5, 0.5)) * ScreenRatio.xy;
                    float distance = sqrt(pow(texCoord.x, 2) + pow(texCoord.y, 2));
                    float radial = distance * RingsAmount;
                    
                    // panding anim
                    float time = _Time.z;
                    float anim1 = abs(radial - time * RingSpeed);
                    float anim2 = abs(radial - (time * RingSpeed * 1.5));
                    
                    // slow rings
                    float add1 = ceil(frac(anim1) - (1 - CircleSize1)) + ceil(frac(CircleSize1 + anim1 + Gap12) - (1 - CircleSize2));
                    float add2 = add1 + ceil(frac(CircleSize2 + anim1 + Gap23) - (1 - CircleSize3));
                    float add3 = add2 + ceil(frac(CircleSize3 + anim1 + Gap34) - (1 - CircleSize4));
                    
                    // fast rings
                    float fast = ceil(frac(anim2) - FastCircleSize);
                    
                    float alpha = add3 + fast;
                    float4 col = lerp(BGColor.rgba, SlowColor.rgba, alpha);
                    col = lerp(col.rgba, FastColor.rgba, fast);
                    
                    float alpha2 = saturate(pow(1-i.uv.y, BottomGradientCoverage) * BottomGradientIntensity);
                    
                    col = lerp(col.rgba, BottomColor.rgba, alpha2);
                    
                    return col;
               }

               ENDCG
           }
       }
}