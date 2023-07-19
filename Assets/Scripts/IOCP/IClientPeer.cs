using System;
using System.Collections.Generic;
using System.Net.Sockets;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public abstract class IClientPeer
    {
        public int _peerId;
        private SocketAsyncEventArgs _receiveAsyncEventArgs;
        private SocketAsyncEventArgs _sendAsyncEventArgs;

        private Socket _socket;
        private List<byte> _readList = new List<byte>();
        private Queue<byte[]> _cacheQueue = new Queue<byte[]>();
        private bool _isWrite = false;

        public Action<int> OnClientPeerCloseCallBack;
        protected ConnectionStateCode _connectionState = ConnectionStateCode.None;

        public IClientPeer()
        {
            _receiveAsyncEventArgs = new SocketAsyncEventArgs();
            _sendAsyncEventArgs = new SocketAsyncEventArgs();
            _receiveAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
            _sendAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
            _receiveAsyncEventArgs.SetBuffer(new byte[ServerConstant.ServerBufferCount], 0, ServerConstant.ServerBufferCount);
        }

        protected abstract void OnConnected();

        protected abstract void OnDisconnected();

        protected abstract void OnReceivedMessage(byte[] byteMessages);

        public void InitClientPeer(Socket skt)
        {
            IOCPLog.Info("Init Client Peer, starting Recieve Async.");
            _socket = skt;
            _connectionState = ConnectionStateCode.Connected;
            OnConnected();
            OnReceiveAsync();
        }

        private void OnReceiveAsync()
        {
            bool isConnetWaiting = _socket.ReceiveAsync(_receiveAsyncEventArgs);
            if (isConnetWaiting == false)
            {
                ProcessReceive();
            }
        }

        private void ProcessReceive()
        {
            if (_receiveAsyncEventArgs.BytesTransferred > 0 && _receiveAsyncEventArgs.SocketError == SocketError.Success)
            {
                byte[] bytes = new byte[_receiveAsyncEventArgs.BytesTransferred];
                Buffer.BlockCopy(_receiveAsyncEventArgs.Buffer, 0, bytes, 0, _receiveAsyncEventArgs.BytesTransferred);
                _readList.AddRange(bytes);
                ProcessByteList();
                OnReceiveAsync();
            }
            else
            {
                IOCPLog.Warning("IClientPeer:{0}  Close:{1}", _peerId, _receiveAsyncEventArgs.SocketError.ToString());
                OnClientClose();
            }
        }

        private void ProcessByteList()
        {
            byte[] byteMessages = IOCPTool.SplitLogicBytes(ref _readList);
            if (byteMessages != null)
            {
                OnReceivedMessage(byteMessages);
                ProcessByteList();
            }
        }

        public bool SendMessage(byte[] byteMessage)
        {
            byte[] bytes = IOCPTool.PackMessageLengthInfo(byteMessage);
            return SendPackMessage(bytes);
        }

        public byte[] GetPackMessage(byte[] byteMessage)
        {
            return IOCPTool.PackMessageLengthInfo(byteMessage);
        }

        public bool SendPackMessage(byte[] bytePackMessages)
        {
            if (_connectionState != ConnectionStateCode.Connected)
            {
                IOCPLog.Warning("Connection is break, can`t send net message.");
                return false;
            }
            if (_isWrite)
            {
                _cacheQueue.Enqueue(bytePackMessages);
                return true;
            }
            _isWrite = true;
            _sendAsyncEventArgs.SetBuffer(bytePackMessages, 0, bytePackMessages.Length);
            bool isSendWaiting = _socket.SendAsync(_sendAsyncEventArgs);
            if (isSendWaiting == false)
            {
                ProcessSend();
            }
            return true;
        }

        private void ProcessSend()
        {
            if (_sendAsyncEventArgs.SocketError == SocketError.Success)
            {
                _isWrite = false;
                if (_cacheQueue.Count > 0)
                {
                    byte[] item = _cacheQueue.Dequeue();
                    SendPackMessage(item);
                }
            }
            else
            {
                IOCPLog.Error("Process Send Error: {0}", _sendAsyncEventArgs.SocketError.ToString());
                OnClientClose();
            }
        }

        public void OnClientClose()
        {
            if (_socket != null)
            {
                _connectionState = ConnectionStateCode.Disconnected;
                OnDisconnected();
                if (OnClientPeerCloseCallBack != null)
                {
                    OnClientPeerCloseCallBack(_peerId);
                }
                _readList.Clear();
                _cacheQueue.Clear();
                _isWrite = false;
                try
                {
                    _socket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception e)
                {
                    IOCPLog.Error("Shutdown socket Error:{0}", e.ToString());
                }
                finally
                {
                    _socket.Close();
                    _socket = null;
                    IOCPLog.Done("Client is Offline");
                }
            }
        }

        private void OnIO_Completed(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            switch (socketAsyncEventArgs.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive();
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend();
                    break;
                default:
                    IOCPLog.Warning("The last operation completed on the socket was not a receive or send");
                    break;
            }
        }
    }
}
