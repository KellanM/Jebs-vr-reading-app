using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Events
{
    [Serializable]
    public class IntEvent : UnityEvent<int> { }

    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [Serializable]
    public class RigidbodyEvent : UnityEvent<Rigidbody> { }

    [Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
}
