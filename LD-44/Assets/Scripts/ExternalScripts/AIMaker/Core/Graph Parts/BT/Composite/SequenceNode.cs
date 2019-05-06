using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Composite/Sequencer Node", DefaultTypeName = "Sequencer Node", Description = "Sequencer node for BT")]
    public class SequenceNode : ACompositeNode
    {
        //*********************
        // MAIN FUNCTIONALITY
        //*********************
        [fsProperty] public bool IsActive { get; private set; }
        [fsProperty] private int m_runningIndex = 0;

        protected override void OnInit()
        {
            base.OnInit();
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
                    case EStatusAI.FAILURE:
                        return childStatus;
                    case EStatusAI.RUNNING:
                        m_runningIndex = currIndex;
                        return childStatus;
                }
            }

            m_runningIndex = 0;
            return EStatusAI.SUCCESS;
        }

    }
}