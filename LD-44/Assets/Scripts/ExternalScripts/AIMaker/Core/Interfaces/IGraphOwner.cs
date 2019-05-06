using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    public interface IGraphOwner
    {
        GraphAI ChildGraphAI
        {
            get;
            set;
        }
    }
}