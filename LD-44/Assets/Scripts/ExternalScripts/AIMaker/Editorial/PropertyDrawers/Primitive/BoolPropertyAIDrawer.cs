using UnityEditor;

namespace aim
{
    public class BoolPropertyAIDrawer : APropertyAIDrawer<bool>
    {
        public override bool Draw(bool instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.Toggle(label, instance);
        }
    }
}