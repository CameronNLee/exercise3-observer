using System;
using UnityEngine;

namespace Pikmini
{
    public class Publisher : IPublisher
    {
        public void Unregister(Action<Vector3> notifier)
        {
        }

        public void Register(Action<Vector3> notifier)
        {
        }

        public void Notify(Vector3 transform)
        {
        }
    }
}