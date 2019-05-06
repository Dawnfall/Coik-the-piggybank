using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    public abstract class ALeafNode : ANodeAI
    {
        public sealed override bool TryDisconnectChild(ANodeAI childToDisconnect)
        {
            return false;
        }

        public sealed override void DisconnectAll()
        {
            if (m_parent != null)
                Parent.TryDisconnectChild(this);
        }

        public sealed override void Terminate()
        {
            OnTerminate();
            Status = EStatusAI.INVALID;
        }

#if UNITY_EDITOR

        public override List<ANodeAI> GetAllChildren_E()
        {
            return new List<ANodeAI>();
        }

        public sealed override bool ConnectToOutPoint_E(ANodeAI nodeToConnect, int outPointIndex)
        {
            return false;
        }

#endif

    }
}