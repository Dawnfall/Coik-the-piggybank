using UnityEditor;

namespace aim
{
    public class StringPropertyAIDrawer : APropertyAIDrawer<string>
    {
        public override string Draw(string instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            if (instance == null)
                instance = "";
            return EditorGUILayout.TextField(label, instance);
        }
    }
}