using UnityEditor;

namespace aim
{
    public class IntPropertyAIDrawer : APropertyAIDrawer<int>
    {
        public override int Draw(int instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.IntField(label, instance);
        }
    }
}