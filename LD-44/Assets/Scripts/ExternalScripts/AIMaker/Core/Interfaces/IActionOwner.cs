using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    public interface IActionOwner
    {
        AActionTaskAI ActionAI
        {
            get;
            set;
        }
    }
}