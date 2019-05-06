using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    public class ValueContainerAIPropertyAIDrawer : APropertyAIDrawer<AValueContainerAI>
    {
        public override AValueContainerAI Draw(AValueContainerAI instance, Type instanceType, string label, AIAgent aiAgent)
        {
            instance.ValueAsObject = PropertyAISerializer.DrawInstance(instance.ValueAsObject, instance.GetValueType(), "", aiAgent);
            return instance;
        }
    }
}