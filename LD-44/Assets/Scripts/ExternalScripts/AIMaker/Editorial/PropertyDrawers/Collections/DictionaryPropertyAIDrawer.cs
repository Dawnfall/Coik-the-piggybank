using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    public class DictionaryPropertyAIDrawer : APropertyAIDrawer<IDictionary>
    {
        public override IDictionary Draw(IDictionary instance, Type instanceType, string label, AIAgent aiAgent)
        {
            return instance;

        }
    }
}