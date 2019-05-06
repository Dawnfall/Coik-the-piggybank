using UnityEditor;

namespace aim
{
    public class FloatPropertyAIDrawer : APropertyAIDrawer<float>
    {
        public override float Draw(float instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.FloatField(label, (float)instance);
        }
    }
}