using UnityEngine;

namespace BardicBytes.BardicUnity
{
    public static class AsyncHelper
    {
        public static bool AsyncIsValid(MonoBehaviour monoBehaviour)
        {
            if(Debug.isDebugBuild) Debug.Assert(monoBehaviour != null);

            bool isValid = true;

            isValid &= monoBehaviour.isActiveAndEnabled;
            isValid &= Application.isPlaying;

            return isValid;
        }
    }
}
