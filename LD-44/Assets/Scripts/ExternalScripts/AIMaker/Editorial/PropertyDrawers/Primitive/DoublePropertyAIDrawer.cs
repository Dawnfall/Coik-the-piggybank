using UnityEditor;

namespace aim
{
    public class DoublePropertyAIDrawer : APropertyAIDrawer<double>
    {
        public override double Draw(double instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return EditorGUILayout.DoubleField(label, instance);
        }
    }
}