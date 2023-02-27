using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace BeeCore {
    public static class RandomBee {
        public static T EnumValue<T>() {
            var v = Enum.GetValues( typeof( T ) );
            return (T) v.GetValue( Random.Range( 0, v.Length ) );
        }

        public static DateTime Day() {
            var start = new DateTime( 1995, 1, 1 );
            var range = ( DateTime.Today - start ).Days;
            return start.AddDays( Random.Range( 0, range ) );
        }

        public static string String( int length = 16,
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" ) {
            var stringChars = new List<char>();

            for( var i = 0; i < length; i++ ) stringChars.Add( chars[ Random.Range( 0, chars.Length ) ] );

            return new string( stringChars.ToArray() );
        }

        [Serializable]
        public struct FloatRange {
            public float min, max;

            // ReSharper disable once InconsistentNaming
            public float value {
                get { return Random.Range( min, max ); }
            }

            // ReSharper disable once UnusedMember.Local
            private FloatRange( float min, float max ) {
                this.min = min;
                this.max = max;
            }
        }
    }
}
