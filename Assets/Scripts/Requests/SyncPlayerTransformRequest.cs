using SangoMMOCommons.Classs;
using SangoMMOCommons.Structs;
using SangoMMONetProtocol;
using System;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public class SyncPlayerTransformRequest : BaseRequest
{
    private TransformOnline playerTransform;

    public override void InitRequset()
    {       
        NetOpCode = OperationCode.SyncPlayerTransform;
        base.InitRequset();
    }

    public void SetPlayerTransform(Vector3 position, Quaternion rotation)
    {
        playerTransform = new TransformOnline
        {
            Account = OnlineAccountCache.Instance.LocalAccount,
            Vector3Position = new Vector3Position
            {
                X = (float)Math.Round(position.x, 2),
                Y = (float)Math.Round(position.y, 2),
                Z = (float)Math.Round(position.z, 2)
            },
            QuaternionRotation = new QuaternionRotation
            {
                X = (float)Math.Round(rotation.x, 2),
                Y = (float)Math.Round(rotation.y, 2),
                Z = (float)Math.Round(rotation.z, 2),
                W = (float)Math.Round(rotation.w, 2)
            }
        };
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        string playerTransformJson = SetJsonString(playerTransform);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, playerTransformJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        string syncPlayerTransformRspJson = sangoNetMessage.MessageBody.MessageString;
        SyncPlayerTransformRsp syncPlayerTransformRsp = DeJsonString<SyncPlayerTransformRsp>(syncPlayerTransformRspJson);
        bool isSyncTransformResult = syncPlayerTransformRsp.SyncPlayerTransformResult;
        TransformOnline targetTransform = null;
        if (isSyncTransformResult)
        {
            targetTransform = playerTransform;
        }
        else
        {
            bool isPredictPlayerTransform = syncPlayerTransformRsp.PredictPlayerTransform;
            if (isPredictPlayerTransform)
            {
                targetTransform = syncPlayerTransformRsp.TransformOnline;
            }
        }
        Vector3 targetPosition = new Vector3(targetTransform.Vector3Position.X, targetTransform.Vector3Position.Y, targetTransform.Vector3Position.Z);
        Quaternion targetRotation = new Quaternion(targetTransform.QuaternionRotation.X, targetTransform.QuaternionRotation.Y, targetTransform.QuaternionRotation.Z, targetTransform.QuaternionRotation.W);      
        //就是下面这行，如果movePlayerCubeController是GetComponent的方式得到，那么立刻就会卡在这里，也没有任何提示，但是没有崩，只是收不到消息
        MainGameSystem.Instance.movePlayerCubeController.SetTransform(targetPosition, targetRotation);
    }
}
