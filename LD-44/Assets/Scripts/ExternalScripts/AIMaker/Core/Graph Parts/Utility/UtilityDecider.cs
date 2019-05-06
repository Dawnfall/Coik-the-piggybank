using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using aim.FullSerializer;

namespace aim
{
    //TODO: maybe add some options(ie. different thresholds, different total utility calculations,...)
    public class UtilityDecider : ATaskAI
    {
        //public override void FindNodesInSubGraph(HashSet<INodeAI> subGraph)
        //{
        //    throw new NotImplementedException();
        //}

        public UtilityDecider()
        {
            Parameters = new List<AUtilityParameter>();
        }

        [fsProperty] public UtilityNode UtilityNode { get; set; }
        [fsProperty] public List<AUtilityParameter> Parameters { get; set; }

        public void AddParameter<T>() where T : AUtilityParameter
        {
            AddParameter(typeof(T));
        }
        public void AddParameter(System.Type parameterType)
        {
            if (parameterType != null && typeof(AUtilityParameter).IsAssignableFrom(parameterType))
                Parameters.Add(System.Activator.CreateInstance(parameterType, this) as AUtilityParameter);
        }
        public void RemoveParameter(AUtilityParameter parameterToRemove)
        {
            Parameters.Remove(parameterToRemove);
        }

        public float CalculateTotalUtility()
        {
            float average = 0.0f;
            foreach (AUtilityParameter parameter in Parameters)
                average += parameter.GetUtilityValue();
            average /= Parameters.Count;

            return average;
        }

        public void DisconnectAll()
        {
            
        }

#if UNITY_EDITOR

        public void Draw_E(Vector2 windowSize)
        {

        }

        public override Rect GetScreenRect_E(Vector2 windowSize)
        {
            Vector2 screenCenter = (Position_E - UtilityNode.GraphAI.ViewOffset_E + windowSize / (2f * UtilityNode.GraphAI.Zoom_E)) * UtilityNode.GraphAI.Zoom_E;
            Vector2 screenSize = Size_E * UtilityNode.GraphAI.Zoom_E;

            return new Rect(screenCenter - screenSize / 2, screenSize);
        }

        public override void OnGUILate_E(Vector2 windowSize)
        {
            ConfigAI_E config = ConfigAI_E.Instance;

            ////selected node

            if (AIEditorWindow.SelectedDrawable_E == this)
            {
                Rect rect = GetScreenRect_E(windowSize);
                Vector2 selectedSize = rect.size * (1.08f);//TODO size percent to parameter
                Rect selectedRect = new Rect(rect.center - selectedSize / 2f, selectedSize);

                GUILayout.BeginArea(selectedRect, config.GetSelectedStyle());
                GUILayout.EndArea();
            }

            //// decider 

            Rect deciderRect = GetScreenRect_E(windowSize);

            GUILayout.BeginArea(deciderRect, ConfigAI_E.Instance.GetNodeRectStyle());
            GUILayout.BeginVertical();

            GUILayout.Label("Decider");

            GUILayout.EndVertical();
            GUILayout.EndArea();

            Rect deciderScreenRect = GetScreenRect_E(windowSize);
            if (AIEditorWindow.IsMousePressed(0) && AIEditorWindow.DetectInRect(deciderScreenRect))
            {
                AIEditorWindow.CurrDraggedDrawable_E = this;
            }
            else if (AIEditorWindow.IsMouseReleased(1) && AIEditorWindow.DetectInRect(deciderScreenRect))
            {
                ShowElementMenu_E();
            }
            else if (AIEditorWindow.IsMousePressed(2) && AIEditorWindow.DetectInRect(deciderScreenRect))
            {
                AIEditorWindow.SelectedDrawable_E = this;
            }
        }

        public override void ShowElementMenu_E()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                UtilityNode tempParent = UtilityNode;
                if (UtilityNode.TryRemoveDecider(this))
                {
                    if (AIEditorWindow.SelectedDrawable_E == this)
                        AIEditorWindow.SelectedDrawable_E = null;
                    tempParent.GraphAI.AgentAI.IsDirty_E = true;
                }
            });
            menu.ShowAsContext();
        }

        public override void OnInspectorGUI(Rect inspectorRect)
        {
            //TODO:...
        }

        public override void OnGUIEarly_E(Vector2 windowSize)
        {
           //TODO:...
        }



#endif

    }
}