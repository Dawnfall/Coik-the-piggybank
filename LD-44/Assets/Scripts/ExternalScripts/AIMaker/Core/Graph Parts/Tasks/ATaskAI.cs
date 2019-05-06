using aim.FullSerializer;
using UnityEngine;
namespace aim
{
    public abstract class ATaskAI : AElementAI
    {
        [fsProperty] [HideInInspector] public ANodeAI ParentNode { get; set; }
    }
}