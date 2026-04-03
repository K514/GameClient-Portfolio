
using System;
using UnityEngine;

namespace k514
{
    public static partial class TableTool
    {
        [Serializable]
        public struct SerializableVector2
        {
            #region <Fields>

            public float x, y;

            #endregion

            #region <Constructor>

            public SerializableVector2(float p_X, float p_Y)
            {
                x = p_X;
                y = p_Y;
            }

            #endregion

            #region <Operator>

            public static implicit operator Vector2(SerializableVector2 p_Vector2) => p_Vector2.GetValue();
            public static implicit operator SerializableVector2(Vector2 p_Vector2) => new SerializableVector2(p_Vector2.x, p_Vector2.y);
            
            public static bool operator==(SerializableVector2 p_Left, SerializableVector2 p_Right)
            {
                return p_Left.x.IsReachedValue(p_Right.x)
                       && p_Left.y.IsReachedValue(p_Right.y);
            }
            
            public static bool operator!=(SerializableVector2 p_Left, SerializableVector2 p_Right)
            {
                return !(p_Left == p_Right);
            }
            
            #endregion
            
            #region <Methods>

            public Vector2 GetValue() => new Vector2(x, y);

            #endregion
        }
        
        [Serializable]
        public struct SerializableVector3
        {
            #region <Fields>

            public float x, y, z;

            #endregion

            #region <Constructor>

            public SerializableVector3(Vector3 p_Vector3)
            {
                x = p_Vector3.x;
                y = p_Vector3.y;
                z = p_Vector3.z;
            }
            
            public SerializableVector3(float p_X, float p_Y, float p_Z)
            {
                x = p_X;
                y = p_Y;
                z = p_Z;
            }

            #endregion

            #region <Operator>

            public static implicit operator Vector3(SerializableVector3 p_Vector3) => p_Vector3.GetValue();
            public static implicit operator SerializableVector3(Vector3 p_Vector3) => new SerializableVector3(p_Vector3);
   
            public static bool operator==(SerializableVector3 p_Left, SerializableVector3 p_Right)
            {
                return p_Left.x.IsReachedValue(p_Right.x)
                       && p_Left.y.IsReachedValue(p_Right.y)
                       && p_Left.z.IsReachedValue(p_Right.z);
            }
            
            public static bool operator!=(SerializableVector3 p_Left, SerializableVector3 p_Right)
            {
                return !(p_Left == p_Right);
            }

            #endregion
            
            #region <Methods>

            public Vector3 GetValue() => new Vector3(x, y, z);

            #endregion
        }
        
        [Serializable]
        public struct SerializableColor
        {
            #region <Fields>

            public float r, g, b, a;

            #endregion

            #region <Constructor>

            public SerializableColor(float p_R, float p_G, float p_B, float p_A)
            {
                r = p_R;
                g = p_G;
                b = p_B;
                a = p_A;
            }
            
            public SerializableColor(float p_R, float p_G, float p_B) : this(p_R, p_G, p_B, 1f)
            {
            }

            #endregion

            #region <Operator>

            public static implicit operator Color(SerializableColor p_Color) => p_Color.GetValue();
            public static implicit operator SerializableColor(Color p_Color) => new SerializableColor(p_Color.r, p_Color.g, p_Color.b, p_Color.a);
   
            public static bool operator==(SerializableColor p_Left, SerializableColor p_Right)
            {
                return p_Left.r.IsReachedValue(p_Right.r)
                       && p_Left.g.IsReachedValue(p_Right.g)
                       && p_Left.b.IsReachedValue(p_Right.b)
                       && p_Left.a.IsReachedValue(p_Right.a);
            }
            
            public static bool operator!=(SerializableColor p_Left, SerializableColor p_Right)
            {
                return !(p_Left == p_Right);
            }

            #endregion
            
            #region <Methods>

            public Color GetValue() => new Color(r, g, b, a);

            #endregion
        }
    }
}