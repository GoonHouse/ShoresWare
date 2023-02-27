using UnityEngine;

namespace BeeCore {
    public static class MathBee {
        public static float Scale( float valueIn, float baseMin, float baseMax, float limitMin, float limitMax ) {
            return ( limitMax - limitMin ) * ( valueIn - baseMin ) / ( baseMax - baseMin ) + limitMin;
        }

        public static float Mod( float a, float b ) {
            return a - b * Mathf.Floor( a / b );
        }

        public static float Round( float value, int digits = 2 ) {
            var mult = Mathf.Pow( 10.0f, digits );
            return Mathf.Round( value * mult ) / mult;
        }
    }
}
