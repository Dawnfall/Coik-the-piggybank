using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    [NodeAIAttribute(DefaultTypeName = "Wait for Node", CreatePath = "Decorator/Wait for Node", Description = "Wait for node for BT")]
    public class WaitForNode : ADecoratorNode
    {
        [fsProperty] public VariableAI<float> WaitTime { get; set; }
        [fsProperty] private float m_startWaitTime = 0f;

        public override void OnCreate()
        {
            base.OnCreate();
            WaitTime = new VariableAI<float>();
        }
        protected override void OnInit()
        {
            m_startWaitTime = Time.time;
        }
        protected override EStatusAI OnUpdate()
        {
            if (Time.time - m_startWaitTime >= WaitTime.Value)
            {
                m_startWaitTime = Time.time;
                return Child.Tick();
            }
            return EStatusAI.FAILURE;
        }
    }
}