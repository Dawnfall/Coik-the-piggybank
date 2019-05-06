using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    public abstract class ADecision : ATaskAI
    {
        [fsProperty] public bool TrueOnTrue { get; set; }

        public ADecision()
        {
            TrueOnTrue = true;
        }

        public abstract bool Decide();
    }
}