using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    public abstract class AUtilityParameter : ATaskAI
    {
        [fsProperty] public UtilityDecider ParentDecider { get; set; }

        public abstract float Max { get; }
        public abstract float Min { get; }
        public abstract float CurrentValue { get; }
        //public IFunctionAI FunctionAI { get; set; }

        public float GetUtilityValue()
        {
            return 0;
            //return FunctionAI.CalculateValue((Mathf.Clamp(CurrentValue, Min, Max) - Min) / (Max - Min));
        }

        [fsProperty]
        public EStatusAI Status
        {
            get;
            protected set;
        }
    }
}