using UnityEditor;

namespace aim
{
    public class UnityObjectAIPropertyDrawer : APropertyAIDrawer<UnityEngine.Object>
    {
        public override UnityEngine.Object Draw(UnityEngine.Object instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.ObjectField(label, instance, instanceType, true);
        }
    }
}