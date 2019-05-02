using System;
using System.Collections.ObjectModel;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

namespace Pikmini
{
    public class Publisher : IPublisher
    {
        private Collection<Action<Vector3>> actions = new Collection<Action<Vector3>>();

        public void Unregister(Action<Vector3> notifier)
        {
            actions.Remove(notifier);
        }

        public void Register(Action<Vector3> notifier)
        {
            actions.Add(notifier);
        }

        public void Notify(Vector3 transform)
        {
            // Here, we invoke every notifier inside the collection with the correct argument.
            foreach (var notifier in actions)
            {
                notifier(transform);
            }
        }
    }
}