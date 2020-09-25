using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace RoomBasedParallax {

    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayAsRange : PropertyAttribute {

        public readonly int minValue = int.MinValue;
        public readonly int maxValue = int.MinValue;
        public readonly string minField;
        public readonly string maxField;

        public DisplayAsRange(string minField, string maxField) {
            this.minField = minField;
            this.maxField = maxField;
        }

        public DisplayAsRange(int minValue, int maxValue) {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public DisplayAsRange(int minValue, string maxField) {
            this.minValue = minValue;
            this.maxField = maxField;
        }

        public DisplayAsRange(string minField, int maxValue) {
            this.minField = minField;
            this.maxValue = maxValue;
        }
    }
}
