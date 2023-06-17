using SangoMMONetProtocol;
using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

public class ProxyService : BaseService
{
    public static ProxyService Instance;

    private Queue<SangoNetMessage> netProxyQueue = new Queue<SangoNetMessage>();

    public override void InitService()
    {
        base.InitService();
        Instance = this;
    }

    private void Update()
    {
        if (netProxyQueue.Count > 0)
        {
            SangoNetMessage sangoNetMessage = netProxyQueue.Dequeue();
            OnRecieveNetProxyMessage(sangoNetMessage);
        }
    }

    public void AddNetProxy(SangoNetMessage sangoNetMessage)
    {
        netProxyQueue.Enqueue(sangoNetMessage);
    }

    private void OnRecieveNetProxyMessage(SangoNetMessage sangoNetMessage)
    {
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
}
