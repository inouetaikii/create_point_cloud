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

            // ���̃��j�t�H�[���ϐ��ŃJ�����̃j�A�E�t�@�[�v���[���̋t�����󂯎��܂�
            float2 _ReciprocalNearFar;

            float4 frag (v2f i) : SV_Target
            {
                // �[�x�e�N�X�`����̒l���擾����...
                float depth = tex2D(_MainTex, i.uv);

                // �v���b�g�t�H�[���ɂ���ăJ�����ɋ߂����������A���������������ς��̂ŁA�����������ɂȂ�悤���ꂵ��...
                #ifdef UNITY_REVERSED_Z
                depth = 1.0 - depth;
                #endif

                // �J������Z���ɉ������A���[�g���P�ʂ̉��s�������߂܂�
                float linearEyeDepth = 1.0 / ((_ReciprocalNearFar.y - _ReciprocalNearFar.x) * depth + _ReciprocalNearFar.x);

                // �~�����[�g���P�ʂ̐[�x��16�r�b�g1�`�����l���摜�ŕ\������ƂȂ�ƁA�ő�[�x��65.535m�ƂȂ�͂��ł�
                // �܂�linearEyeDepth��65.535�Ŋ���΁A0mm�`65535mm��\��������`�O���[�������邩�Ǝv���܂�
                return linearEyeDepth / 65.535;
            }
            ENDCG
        }
    }
}