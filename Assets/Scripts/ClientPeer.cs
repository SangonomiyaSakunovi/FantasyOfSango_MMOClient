using SangoIOCPNet;
using SangoMMOCommons.Tools;
using SangoMMONetProtocol;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public class ClientPeer : IClientPeer
{
    protected override void OnConnect()
    {
        Debug.Log("Connect to Server.");
    }

    protected override void OnDisconnect()
    {
        Debug.Log("DisConnect.");
    }

    protected override void OnRecieveMessage(byte[] byteMessages)
    {
        SangoNetMessage sangoNetMessage = ProtobufTool.DeProtoBytes<SangoNetMessage>(byteMessages);        
        switch (sangoNetMessage.MessageHead.MessageCommand)
        {
            case MessageCommand.OperationResponse:
                {
                    NetService.Instance.OnOperationResponseDistribute(sangoNetMessage);
                }
                break;
            case MessageCommand.EventData:
                {
                    NetService.Instance.OnEventDataDistribute(sangoNetMessage);
                }
                break;
        }
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
