﻿using UnityEngine;
using static UnityEngine.Mathf;


namespace du.Ex {

    /// <summary> Vectorに対する拡張メソッド群 </summary>
    public static class ExVector {

        /// <summary> Vector2にxを加えてVec3に </summary>
        public static Vector3 AddX(this Vector2 vec2, float x) => new Vector3(x, vec2.x, vec2.y);
        /// <summary> Vector2にyを加えてVec3に </summary>
        public static Vector3 AddY(this Vector2 vec2, float y) => new Vector3(vec2.x, y, vec2.y);
        /// <summary> Vector2にzを加えてVec3に </summary>
        public static Vector3 AddZ(this Vector2 vec2, float z) => new Vector3(vec2.x, vec2.y, z);

        /// <summary> Vector3からxを除いてVec2に </summary>
        public static Vector2 DisX(this Vector3 vec3) => new Vector2(vec3.y, vec3.z);
        /// <summary> Vector3からyを除いてVec2に </summary>
        public static Vector2 DisY(this Vector3 vec3) => new Vector2(vec3.x, vec3.z);
        /// <summary> Vector3からzを除いてVec2に </summary>
        public static Vector2 DisZ(this Vector3 vec3) => new Vector2(vec3.x, vec3.y);

        /// <summary> Vector3のxを上書きしたVec3を生成 </summary>
        public static Vector3 ReX(this Vector3 vec3, float x) => new Vector3(x, vec3.y, vec3.z);
        /// <summary> Vector3のyを上書きしたVec3を生成 </summary>
        public static Vector3 ReY(this Vector3 vec3, float y) => new Vector3(vec3.x, y, vec3.z);
        /// <summary> Vector3のzを上書きしたVec3を生成 </summary>
        public static Vector3 ReZ(this Vector3 vec3, float z) => new Vector3(vec3.x, vec3.y, z);

        /// <summary> Vector2のxを上書きしたVec3を生成 </summary>
        public static Vector2 ReX(this Vector2 vec2, float x) => new Vector2(x, vec2.y);
        /// <summary> Vector2のyを上書きしたVec3を生成 </summary>
        public static Vector2 ReY(this Vector2 vec2, float y) => new Vector2(vec2.x, y);

        /// <summary> x,yをそれぞれClamp </summary>
        public static Vector2 Clamped(this Vector2 vec2, float min, float max) {
            return new Vector2(Clamp(vec2.x, min, max), Clamp(vec2.y, min, max));
        }

        /// <summary> x軸とのなす角[rad]([-π,π]) </summary>
        public static float Theta(this Vector2 vec2) => Mathf.Atan2(vec2.y, vec2.x);

        /// <summary>
        /// 同じ方向で違う長さのVec2
        /// - vec2≒0の場合、方向が判別できないためlengthにかかわらず(0,0)を返す
        /// </summary>
        public static Vector2 Resized(this Vector2 vec2, float length) {
            if (Mathf.Approximately(vec2.magnitude, 0f)) {
                return Vector2.zero;
            }
            else {
                return vec2 * length / vec2.magnitude;
            }
        }

        /// <summary> nullだったらdefaultValueを返す </summary>
        public static Vector3 DisN(this Vector3? nullable, Vector3 defaultValue) => nullable ?? defaultValue;
        /// <summary> nullだったらdefaultValueを返す </summary>
        public static Vector2 DisN(this Vector2? nullable, Vector2 defaultValue) => nullable ?? defaultValue;
        /// <summary> nullだったら(0,0,0)を返す </summary>
        public static Vector3 DisN0(this Vector3? nullable) => nullable ?? Vector3.zero;
        /// <summary> nullだったら(0,0)を返す </summary>
        public static Vector2 DisN0(this Vector2? nullable) => nullable ?? Vector2.zero;

        /// <summary> x,y,zそれぞれにprocを適用したVec3(x',y',z')を生成 </summary>
        public static Vector3 Select(this Vector3 a, System.Func<float, float> proc) => new Vector3(proc(a.x), proc(a.y), proc(a.z));
        /// <summary> x,y,zそれぞれにproc(x, x2)を適用したVec3(x',y',z')を生成 </summary>
        public static Vector3 Select(this Vector3 a, Vector3 b, System.Func<float, float, float> proc) => new Vector3(proc(a.x, b.x), proc(a.y, b.y), proc(a.z, b.z));

        /// <summary> x,yそれぞれにprocを適用したVec3(x',y')を生成 </summary>
        public static Vector2 Select(this Vector2 a, System.Func<float, float> proc) => new Vector2(proc(a.x), proc(a.y));
        /// <summary> x,yそれぞれにproc(x, x2)を適用したVec3(x',y')を生成 </summary>
        public static Vector2 Select(this Vector2 a, Vector3 b, System.Func<float, float, float> proc) => new Vector2(proc(a.x, b.x), proc(a.y, b.y));

        /// <summary> a=(x0,y0,z0), b=(x1,y1,z1)に対してret=(x0*x1,y0*y1,z0*z1) </summary>
        public static Vector3 ElemProduct(Vector3 a, Vector3 b) => Select(a, b, (float x, float y) => x * y);

    }

}