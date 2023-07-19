using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public class IOCPClientPeerPool<T> where T : IClientPeer, new()
    {
        private Stack<T> _clientPeerStack;
        public int Size => _clientPeerStack.Count;

        public IOCPClientPeerPool(int capacity)
        {
            _clientPeerStack = new Stack<T>(capacity);
        }

        public T Pop()
        {
            lock (_clientPeerStack)
            {
                return _clientPeerStack.Pop();
            }
        }

        public void Push(T peer)
        {
            if (peer == null)
            {
                IOCPLog.Error("The clientPeer to pool can`t be null");
            }
            lock (_clientPeerStack)
            {
                _clientPeerStack.Push(peer);
            }
        }
    }
}
