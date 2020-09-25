using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {
    internal static class TransformExtensions {

        internal static List<T> GetComponentsInChildrenRecursive<T>(this Transform targetTransform) where T : Component {
            var componentList = new List<T>();
            componentList.AddRange(targetTransform.GetComponents<T>());

            for (var i = 0; i < targetTransform.childCount; i++) {
                var childTransform = targetTransform.GetChild(i);
                componentList.AddRange(GetComponentsInChildrenRecursive<T>(childTransform));
            }
            return componentList;
        }
    }
}
