using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public class IOCPPeer<T> where T : IClientPeer, new()
    {
        private Socket _socket;
        private SocketAsyncEventArgs _socketAsyncEventArgs;

        public IOCPPeer()
        {
            _socketAsyncEventArgs = new SocketAsyncEventArgs();
            _socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
        }

        #region Client
        public T ClientPeer;

        public void StartAsClient(string ip, int port)
        {
            IOCPLog.Start("Start as Client.");
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socketAsyncEventArgs.RemoteEndPoint = point;
            OnConnect();
        }

        private void OnConnect()
        {
            bool isConnetWaiting = _socket.ConnectAsync(_socketAsyncEventArgs);
            if (isConnetWaiting == false)
            {
                ProcessConnect();
            }
        }

        private void ProcessConnect()
        {
            ClientPeer = new T();
            ClientPeer.InitClientPeer(_socket);
        }

        public void CloseClient()
        {
            if (ClientPeer != null)
            {
                ClientPeer.OnClientClose();
                ClientPeer = null;
            }
            if (_socket != null)
            {
                _socket = null;
            }
        }
        #endregion

        #region Server
        private int _currentConnectCount = 0;
        private int _backLog = ServerConstant.ServerBackLogCount;
        private int _maxConnectCount = ServerConstant.ServerMaxConnectCount;

        private Semaphore _acceptSeamaphore;
        private IOCPClientPeerPool<T> _peerPool;
        private List<T> _peerList;

        public void StartAsServer(string ip, int port, int maxConnectCount)
        {
            IOCPLog.Start("Start as Server.");
            _currentConnectCount = 0;
            _acceptSeamaphore = new Semaphore(maxConnectCount, maxConnectCount);
            _peerPool = new IOCPClientPeerPool<T>(maxConnectCount);
            for (int i = 0; i < maxConnectCount; i++)
            {
                T peer = new T
                {
                    _peerId = i,
                };
                _peerPool.Push(peer);
            }
            _peerList = new List<T>();
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(point);
            _socket.Listen(_backLog);
            IOCPLog.Done("IOCPServer is Init");
            OnAccept();
        }

        private void OnAccept()
        {
            _socketAsyncEventArgs.AcceptSocket = null;
            _acceptSeamaphore.WaitOne();
            bool isAcceptWaiting = _socket.AcceptAsync(_socketAsyncEventArgs);
            if (isAcceptWaiting == false)
            {
                ProcessAccept();
            }
        }

        private void ProcessAccept()
        {
            Interlocked.Increment(ref _currentConnectCount);
            T peer = _peerPool.Pop();
            lock (_peerList)
            {
                _peerList.Add(peer);
            }
            peer.InitClientPeer(_socketAsyncEventArgs.AcceptSocket);
            peer.OnClientPeerCloseCallBack = RecycleClientPeerPool;
            IOCPLog.Done("Client Online, allocate ClientId:{0}", peer._peerId);
            OnAccept();
        }

        public void CloseServer()
        {
            for (int i = 0; i < _peerList.Count; i++)
            {
                _peerList[i].OnClientClose();
            }
            _peerList.Clear();
            _peerList = null;
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }

        private void RecycleClientPeerPool(int peerId)
        {
            int index = -1;
            for (int i = 0; i < _peerList.Count; i++)
            {
                if (_peerList[i]._peerId == peerId)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                _peerPool.Push(_peerList[index]);
                lock (_peerList)
                {
                    _peerList.RemoveAt(index);
                }
                Interlocked.Decrement(ref _currentConnectCount);
                _acceptSeamaphore.Release();
            }
            else
            {
                IOCPLog.Error("IClientPeer: {0} can`t find in server peerList.", peerId);
            }
        }

        public List<T> GetAllClientPeerList()
        {
            return _peerList;
        }
        #endregion

        private void OnIO_Completed(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            switch (socketAsyncEventArgs.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    ProcessAccept();
                    break;
                case SocketAsyncOperation.Connect:
                    ProcessConnect();
                    break;
                default:
                    IOCPLog.Warning("The last operation completed on the socket was not a accept or connect");
                    break;
            }
        }
    }
}
