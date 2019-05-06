using System.Collections;
using UnityEditor;

namespace aim
{
    public class ListPropertyAIDrawer : APropertyAIDrawer<IList>
    {
        public override IList Draw(IList instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            if (instance == null)
            {
                EditorGUILayout.LabelField(label + " : NULL");
                return instance;
            }

            ConfigAI_E config = ConfigAI_E.Instance;
            for (int i = 0; i < instance.Count; i++)
            {
                object item = instance[i];
                System.Type itemType = (item != null) ? item.GetType() : null;
                instance[i] = PropertyAISerializer.DrawInstance(item, itemType, "", parentAIAgent);
            }

            return instance;
        }
    }
}