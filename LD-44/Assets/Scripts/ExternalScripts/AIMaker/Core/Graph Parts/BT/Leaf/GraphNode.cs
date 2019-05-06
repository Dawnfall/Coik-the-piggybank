using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Leaf/Graph Node", DefaultTypeName = "Graph Node", Description = "Leaf node that stores a subgraph")]
    public class GraphNode : ALeafNode, IGraphOwner
    {
        [fsProperty] public GraphAI ChildGraphAI { get; set; }

        public override void OnCreate()
        {
            base.OnCreate();
            Instantiator.CreateNewGraphAI(this);
        }

        protected override EStatusAI OnUpdate()
        {
            if (GraphAI != null)
            {
                return GraphAI.OnTick();
            }
            return EStatusAI.FAILURE;
        }


    }
}