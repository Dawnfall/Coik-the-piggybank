using UnityEditor;
using UnityEngine;

namespace aim
{
    public class CurvePropertyAIDrawer : APropertyAIDrawer<AnimationCurve>
    {
        public override AnimationCurve Draw(AnimationCurve instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.CurveField(label, instance);
        }
    }
}