using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	SaveHelper, Helper functions to load/save stuff. Partial class so you can add your own helper functions (in another file)
    and safely update this package.
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace MagicSave
{
    public static partial class SaveHelper
    {
        [Serializable]
        public struct Vector3Serializer
        {
            // problem: Vector3, Quaternion cannot be serialized by Binary Formatter
            public float x;
            public float y;
            public float z;

            public void Set(Vector3 v3)
            {
                x = v3.x;
                y = v3.y;
                z = v3.z;
            }

            public Vector3 Get()
            {
                return new Vector3(x, y, z);
            }
        }

        [Serializable]
        public struct QuaternionSerializer
        {
            public float x;
            public float y;
            public float z;
            public float w;

            public void Set(Quaternion q)
            {
                x = q.x;
                y = q.y;
                z = q.z;
                w = q.w;
            }

            public Quaternion Get()
            {
                return new Quaternion(x, y, z, w);
            }
        }


        [Serializable]
        public struct TransformSerializer
        {
            public Vector3Serializer pos;
            public QuaternionSerializer rot;
            public Vector3Serializer scale;

            public void From(Transform t)
            {
                pos.Set(t.position);
                rot.Set(t.rotation);
                scale.Set(t.localScale);
            }

            public void To(Transform t)
            {
                t.position = pos.Get();
                t.rotation = rot.Get();
                t.localScale = scale.Get();
            }
        }



    }// SaveHelper
}// ns
