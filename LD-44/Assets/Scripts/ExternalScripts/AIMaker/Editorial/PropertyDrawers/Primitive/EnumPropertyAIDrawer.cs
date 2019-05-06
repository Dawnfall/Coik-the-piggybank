using UnityEditor;

namespace aim
{
    public class EnumPropertyAIDrawer : APropertyAIDrawer<System.Enum>
    {
        public override System.Enum Draw(System.Enum instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.EnumPopup(label, instance);
        }
    }
}