using UnityEditor;

namespace aim
{
    public class LongPropertyAIDrawer : APropertyAIDrawer<long>
    {
        public override long Draw(long instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.LongField(label, instance);
        }
    }
}