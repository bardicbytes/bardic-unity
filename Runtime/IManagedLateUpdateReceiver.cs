//alex@bardicbytes.com

namespace BardicBytes.BardicUnity
{
    public interface IManagedLateUpdateReceiver : ISortableBehaviour
    {
        public void ManagedLateUpdate();
    }
}
