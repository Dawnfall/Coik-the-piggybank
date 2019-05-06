using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Decorator/Inverter Node", DefaultTypeName = "Inverter Node", Description = "Inverter node for BT")]
    public class InverterNode : ADecoratorNode
    {
        protected override EStatusAI OnUpdate()
        {
            if (Child == null)
                return EStatusAI.FAILURE;

            EStatusAI childStatus = Child.Tick();
            switch (childStatus)
            {
                case EStatusAI.SUCCESS:
                    return EStatusAI.FAILURE;
                case EStatusAI.FAILURE:
                    return EStatusAI.SUCCESS;
                default:
                    return childStatus;
            }
        }
    }
}