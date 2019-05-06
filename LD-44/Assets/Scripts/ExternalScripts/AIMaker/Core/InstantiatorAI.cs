using System;

namespace aim
{
    public static partial class Instantiator
    {
        public static GraphAI CreateNewGraphAI<T>(T graphOwner) where T : IGraphOwner, IIdable
        {
            if (graphOwner != null)
            {
                GraphAI newGraph = graphOwner.AgentAI.Identifier.CreateIdable<GraphAI>();
                graphOwner.ChildGraphAI = newGraph;
                newGraph.GraphOwner = graphOwner;

                return newGraph;
            }

            return null;
        }
        public static void DestroyGraph(GraphAI graph) //TODO: correct this prolly
        {
            if (graph != null)
            {
                graph.AgentAI.Identifier.DestroyID(graph);
                graph.GraphOwner = null;
            }
            //if (graph != null)
            //{
            //    while (graph.Nodes.Count > 0)
            //    {
            //        DestroyNode(graph.Nodes[graph.Nodes.Count - 1]);
            //    }
            //    if (graph.GraphOwner != null)
            //        graph.GraphOwner.GraphAI = null;

            //    graph.AgentAI.DestroyID(graph);
            //}
        }

        public static T CreateNode<T>(GraphAI graphAI) where T : ANodeAI, new()
        {
            if (graphAI != null)
            {
                return graphAI.CreateNode<T>();
            }
            return null;
        }
        public static ANodeAI CreateNode(System.Type nodeType, GraphAI graphAI)
        {
            if (graphAI != null)
            {
                return graphAI.CreateNode(nodeType);
            }
            return null;
        }
        public static void DestroyNode(ANodeAI nodeToRemove)
        {
            if (nodeToRemove != null)
            {
                nodeToRemove.GraphAI.DestroyNode(nodeToRemove);
            }
        }

        //    public static T CreateAction<T, U>(U actionOwner) where T : AActionTaskAI, new() where U : ANodeAI, IActionOwner
        //    {
        //        if (actionOwner != null)
        //        {
        //            return actionOwner.
        //        }
        //        return null;
        //    }
        //    public static AActionTaskAI CreateActionTaskAI<T>(Type actionType, T actionOwner) where T : ANodeAI, IActionOwner
        //    {
        //        if (actionType != null && actionOwner != null && typeof(AActionTaskAI).IsAssignableFrom(actionType) && !actionType.IsAbstract)
        //        {
        //            AActionTaskAI newAction = actionOwner.AgentAI.CreateIdable(actionType) as AActionTaskAI;
        //            newAction.ParentNode = actionOwner;
        //            actionOwner.ActionAI = newAction;
        //        }
        //        return null;
        //    }
        //    public static void DestroyAction(AActionTaskAI actionToRemove)
        //    {
        //        if (actionToRemove != null)
        //        {
        //            actionToRemove.AgentAI.DestroyID(actionToRemove);
        //            //TODO: remove from parent
        //        }
        //    }
    }
}