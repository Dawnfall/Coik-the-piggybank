using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    public abstract class AActionTaskAI : ATaskAI
    {
        public virtual void OnBeginAct() { }
        public abstract EStatusAI OnAct();
        public virtual void OnEndAct() { }
    }
}