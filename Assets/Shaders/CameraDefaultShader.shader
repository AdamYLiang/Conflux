Shader "Custom/CameraDefaultShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors: col = 1 - col;
				// Sets to red : col = float4(1.0, 0, 0, 1);


				//Calculate HSL from rgb
				float hue, sat, lum;

				//1. Find min and max of rgb values.
				float minValue, maxValue;
				minValue = min(col.r, col.g);
				minValue = min(minValue, col.b);

				//Note: can't compare more than two
				maxValue = max(col.r, col.g);
				maxValue = max(maxValue, col.b);
				
				//2. Find luminace value. Add both min and max and divide by two.
				lum = (minValue + maxValue) / 2;

				//3. Find saturation. 
				//If min and max are the samwe we have no saturation.
				if (minValue == maxValue) {
					hue = 0;
				}//Else we need to check our brightness to see which formula.
				else if (lum < 0.5f) {
					sat = (maxValue - minValue) / (maxValue + minValue);
				}
				else if (lum >= 0.5f) {
					sat = (maxValue - minValue) / (2.0 - maxValue - minValue);
				}

				//4. Find hue, formula based on which RGB is maxed
				if (maxValue == col.r) {
					hue = (col.g - col.b) / (maxValue - minValue);
				}
				else if (maxValue == col.g) {
					hue = 2.0 + (col.b - col.r) / (maxValue - minValue);
				}
				else if (maxValue == col.b) {
					hue = 4.0 + (col.r - col.g) / (maxValue - minValue);
				}

				//Convert to a circle by multiplying by 60.
				hue *= 60;

				//If we are negative add 360 to it since it is a circle.
				if (hue < 0) {
					hue += 360;
				}


				//Do something to the HSL values.

				//Try changing lum to be from -0.5 to 0.5, instead of 0 to 1
				lum -= 0.1;

				//Increase the difference it has.
				lum *= 2;
				//lum += 0.5;

				/* Old style. Causes very sharp transitions
				if (lum < 0.035f) {
					lum = 0;
				}
				else if (lum < 0.25) {
					lum -= 0.035f;
				}
				else if (lum < 1 - 0.25){
					lum += 0.035f;
				}
				else if (lum < 1) {
					lum = 1;
				}*/

				//Convert HSL back to RGB.
				float temp1, temp2;
				
				//1. Calculate the temp1 value based on lum.
				if (lum <= 0.5) {
					temp1 = lum * (1.0 + sat);
				}
				else if (lum > 0.5) {
					temp1 = lum + sat - lum * sat;
				}
				
				//2. Calculate the temp2 value.
				temp2 = 2 * lum - temp1;

				//3. Convert hue to 0.0 - 1.0
				hue = hue/360;

				//4. Calculate more temp values.
				float tempR, tempG, tempB;
				tempR = hue + 0.333;
				if (tempR < 0) {
					tempR += 1;
				}
				else if (tempR > 1) {
					tempR -= 1;
				}

				tempG = hue;
				if (tempG < 0) {
					tempG += 1;
				}
				else if (tempG > 1) {
					tempG -= 1;
				}

				tempB = hue - 0.333;
				if (tempB < 0) {
					tempB += 1;
				}
				else if (tempB > 1) {
					tempB -= 1;
				}

				//5. Calculate the RGB values.
				if (6 * tempR < 1) {
					col.r = temp2 + (temp1 - temp2) * 6 * tempR;
				}
				else if (2 * tempR < 1) {
					col.r = temp1;
				}
				else if (3 * tempR < 2) {
					col.r = temp2 + (temp1 - temp2) * (0.666 - tempR) * 6;
				}
				else {
					col.r = temp2;
				}

				if (6 * tempG < 1) {
					col.g = temp2 + (temp1 - temp2) * 6 * tempG;
				}
				else if (2 * tempG < 1) {
					col.g = temp1;
				}
				else if (3 * tempG < 2) {
					col.g = temp2 + (temp1 - temp2) * (0.666 - tempG) * 6;
				}
				else {
					col.g = temp2;
				}

				if (6 * tempB < 1) {
					col.b = temp2 + (temp1 - temp2) * 6 * tempB;
				}
				else if (2 * tempB < 1) {
					col.b = temp1;
				}
				else if (3 * tempB < 2) {
					col.b = temp2 + (temp1 - temp2) * (0.666 - tempB) * 6;
				}
				else {
					col.b = temp2;
				}

				return col;
			}
			ENDCG
		}
	}
}
