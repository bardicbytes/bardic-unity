using UnityEngine;

namespace BardicBytes.BardicUnity
{
    public interface IBardicBehaviour
    {
        public GameObject gameObject { get; }
        public bool enabled { get; }
        public bool isActiveAndEnabled { get; }
    }
}