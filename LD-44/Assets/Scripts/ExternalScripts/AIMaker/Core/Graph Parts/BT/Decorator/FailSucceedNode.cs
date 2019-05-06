using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Decorator/Fail Succeed Node", DefaultTypeName = "Fail Succeed Node", Description = "Fail or succeed node for BT")]
    public class FailSucceedNode : ADecoratorNode
    {
        [fsProperty] public VariableAI<bool> IsSucceeder { get; set; }

        public override void OnCreate()
        {
            base.OnCreate();
            IsSucceeder = new VariableAI<bool>();
        }

        protected override EStatusAI OnUpdate()
        {
            Child.Tick();
            return (IsSucceeder.Value) ? EStatusAI.SUCCESS : EStatusAI.FAILURE;
        }
    }
}