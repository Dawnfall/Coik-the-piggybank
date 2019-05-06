using UnityEditor;
using UnityEngine;

namespace aim
{
    public class Vector2PropertyAIDrawer : APropertyAIDrawer<Vector2>
    {
        public override Vector2 Draw(Vector2 instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.Vector2Field(label, instance);
        }
    }
    public class Vector3PropertyAIDrawer : APropertyAIDrawer<Vector3>
    {
        public override Vector3 Draw(Vector3 instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.Vector3Field(label, instance);
        }
    }
    public class Vector4PropertyAIDrawer : APropertyAIDrawer<Vector4>
    {
        public override Vector4 Draw(Vector4 instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.Vector4Field(label, instance);
        }
    }
}