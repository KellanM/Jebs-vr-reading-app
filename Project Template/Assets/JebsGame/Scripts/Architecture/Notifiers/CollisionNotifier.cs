using JebsReadingGame.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Notifiers
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionNotifier : MonoBehaviour
    {
        public RigidbodyEvent onRbEnter = new RigidbodyEvent();
        public RigidbodyEvent onRbExit = new RigidbodyEvent();

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody)
                onRbEnter.Invoke(collision.rigidbody);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.rigidbody)
                onRbExit.Invoke(collision.rigidbody);
        }
    }
}
