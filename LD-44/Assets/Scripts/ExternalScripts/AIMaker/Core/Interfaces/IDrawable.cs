using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    public interface IDrawable
    {
#if UNITY_EDITOR

        Vector2 Position_E
        {
            get;
            set;
        }
        string Name_E
        {
            get;
            set;
        }
        Vector2 Size_E
        {
            get;
        }
        string Description_E
        {
            get;
            set;
        }

        void OnGUIEarly_E(Vector2 windowSize);
        void OnGUILate_E(Vector2 windowSize);
        void OnInspectorGUI(Rect inspectorRect);
#endif
    }
}