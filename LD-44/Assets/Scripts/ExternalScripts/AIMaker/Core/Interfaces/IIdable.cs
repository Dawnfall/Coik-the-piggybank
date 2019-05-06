using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    public interface IIdable
    {
        AIAgent AgentAI { get; set; }
        int ID { get; set; }
        string Tag { get; set; }

        void OnCreate();
        void OnDestroy();

        void OnAwake();
        void OnStart();
        void OnDisable();
        void OnEnable();

    }
}

