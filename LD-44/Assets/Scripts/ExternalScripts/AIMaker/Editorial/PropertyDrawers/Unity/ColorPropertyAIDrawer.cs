using UnityEditor;
using UnityEngine;

namespace aim
{
    public class ColorPropertyAIDrawer : APropertyAIDrawer<Color>
    {
        public override Color Draw(Color instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.ColorField(label, instance);
        }
    }
}