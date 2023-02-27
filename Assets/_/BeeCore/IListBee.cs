using System.Collections.Generic;
using UnityEngine;

namespace BeeCore.Extensions {
    // ReSharper disable once InconsistentNaming
    public static class IListBee {
        public static void Shuffle<T>( this IList<T> list ) {
            var n = list.Count;
            while( n > 1 ) {
                n--;
                var k = UnityEngine.Random.Range( 0, n + 1 );
                var value = list[ k ];
                list[ k ] = list[ n ];
                list[ n ] = value;
            }
        }
        
        public static T Random<T>( this IList<T> list, int min = 0, int max = -1 ) {
            if( max <= 0 ) max = list.Count;
            Debug.Assert( list != null, "list != null" );
            Debug.Assert( list.Count > 0, "list.Count > 0" );
            return list[UnityEngine.Random.Range( min, max )];
        }
    }
}
