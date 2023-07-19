using SangoIOCPNet;
using SangoMMOCommons.Tools;
using SangoMMONetProtocol;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public class ClientPeer : IClientPeer
{
    protected override void OnConnected()
    {
        Debug.Log("Connect to Server.");
    }

    protected override void OnDisconnected()
    {
        Debug.Log("DisConnect.");
    }

    protected override void OnReceivedMessage(byte[] byteMessages)
    {
        SangoNetMessage sangoNetMessage = ProtobufTool.DeProtoBytes<SangoNetMessage>(byteMessages);
        ProxyService.Instance.AddNetProxy(sangoNetMessage);
    }

    public void SendOperationRequest(OperationCode operationCode, string messageString)
    {
        MessageHead messageHead = new()
        {
            OperationCode = operationCode,
            MessageCommand = MessageCommand.OperationRequest
        };
        MessageBody messageBody = new()
        {
            MessageString = messageString
        };
        SangoNetMessage sangoNetMessage = new()
        {
            MessageHead = messageHead,
            MessageBody = messageBody
        };
        SendData(sangoNetMessage);
    }

    private void SendData(SangoNetMessage sangoNetMessage)
    {
        byte[] bytes = ProtobufTool.SetProtoBytes(sangoNetMessage);
        SendMessage(bytes);
    }
}
