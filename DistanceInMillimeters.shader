Shader "Unlit/DistanceInMillimeters"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            // このユニフォーム変数でカメラのニア・ファープレーンの逆数を受け取ります
            float2 _ReciprocalNearFar;

            float4 frag (v2f i) : SV_Target
            {
                // 深度テクスチャ上の値を取得して...
                float depth = tex2D(_MainTex, i.uv);

                // プラットフォームによってカメラに近い側が白か、遠い側が白かが変わるので、遠い側が白になるよう統一して...
                #ifdef UNITY_REVERSED_Z
                depth = 1.0 - depth;
                #endif

                // カメラのZ軸に沿った、メートル単位の奥行きを求めます
                float linearEyeDepth = 1.0 / ((_ReciprocalNearFar.y - _ReciprocalNearFar.x) * depth + _ReciprocalNearFar.x);

                // ミリメートル単位の深度を16ビット1チャンネル画像で表現するとなると、最大深度は65.535mとなるはずです
                // つまりlinearEyeDepthを65.535で割れば、0mm～65535mmを表現する線形グレーが得られるかと思います
                return linearEyeDepth / 65.535;
            }
            ENDCG
        }
    }
}