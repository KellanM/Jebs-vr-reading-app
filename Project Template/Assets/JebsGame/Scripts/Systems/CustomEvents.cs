using JebsReadingGame.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingame.Events
{
    [Serializable]
    public class IntEvent : UnityEvent<int> { }
    [Serializable]
    public class CharEvent : UnityEvent<char> { }
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }
    [Serializable]
    public class StringEvent : UnityEvent<string> { }
    [Serializable]
    public class IntIntEvent : UnityEvent<int,int> { }
    [Serializable]
    public class IntCharEvent : UnityEvent<int, char> { }
    [Serializable]
    public class ActivityEvent : UnityEvent<Activity> { }
    [Serializable]
    public class ActivityLetterGroupEvent : UnityEvent<Activity, LetterGroup> { }
    [Serializable]
    public class SceneStateEvent : UnityEvent<SceneState> { }
}