using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Composite/Selector Node", DefaultTypeName = "Selector Node", Description = "Selector node for BT")]
    public class SelectorNode : ACompositeNode
    {
        //*******************
        // MAIN FUNCTIONALITY
        //*******************
        [fsProperty] public bool IsActive { get; private set; }
        [fsProperty] [HideInInspector] private int m_runningIndex = 0;

        protected override void OnInit()
        {
            base.OnTerminate();
            m_runningIndex = 0;
        }

        protected override EStatusAI OnUpdate()
        {
            int startIndex = (IsActive) ? 0 : m_runningIndex;
            for (int currIndex = startIndex; currIndex < Children.Count; currIndex++)
            {
                EStatusAI childStatus = Children[currIndex].Tick();
                switch (childStatus)
                {
                    case EStatusAI.SUCCESS:
                        return childStatus;
                    case EStatusAI.RUNNING:
                        m_runningIndex = currIndex;
                        return childStatus;
                }
            }
            return EStatusAI.FAILURE;
        }
    }
}