using System.Collections.Generic;
using UnityEngine;

namespace BeeCore.Extensions {
    // ReSharper disable once InconsistentNaming
    public static class Vector2Bee {
        public static float Random( this Vector2 vec) {
            return UnityEngine.Random.Range( vec.x, vec.y );
        }
    }
}
