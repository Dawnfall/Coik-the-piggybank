using UnityEditor;

namespace aim
{
    public class DefaultPropertyDrawer : APropertyAIDrawer<object>
    {
        public override object Draw(object instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            string name = instanceType.ToString() + " , ";
            name += (instance == null) ? "NULL" : instance.ToString();

            EditorGUILayout.LabelField(label, name);
            return instance;
        }
    }
}