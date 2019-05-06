using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace aim
{
    public class NodeAIAttribute : Attribute
    {
        public string DefaultTypeName;
        public string CreatePath;
        public string Description;
    }
}