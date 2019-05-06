using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    public interface IInspectable
    {
#if UNITY_EDITOR

        void OnDrawInspector();

#endif
    }
}