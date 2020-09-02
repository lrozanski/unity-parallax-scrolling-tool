using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [AttributeUsage(AttributeTargets.Field)]
    public class UsePropertyInInspector : PropertyAttribute {

        public readonly string FieldName;

        public UsePropertyInInspector(string fieldName) {
            FieldName = fieldName;
        }
    }
}
