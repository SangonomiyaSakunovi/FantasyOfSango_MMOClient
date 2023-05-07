using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using SangoCommon.Structs;
using SangoCommon.Tools;

//Developer : SangonomiyaSakunovi
//Discription: The Synv Transform Request.

public class SyncPlayerTransformRequest : BaseRequest
{
    public string Account { get; private set; }
    private TransformOnline PlayerTransform;

    public override void InitRequset()
    {
        base.InitRequset();
    }

    public void SetPlayerTransform(Vector3 position, Quaternion rotation)
    {
        PlayerTransform = new TransformOnline
        {
            Account = this.Account,
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
        string playerTransformJson = SetJsonString(PlayerTransform);
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.PlayerTransform, playerTransformJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        TransformOnline targetTransform = null;
        bool syncTransformResult = (bool)DictTools.GetDictValue<byte, object>(operationResponse.Parameters, (byte)ParameterCode.SyncPlayerTransformResult);
        if (syncTransformResult)
        {
            targetTransform = PlayerTransform;
        }
        else
        {
            string predictTransformJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.PredictPlayerTransform);
            targetTransform = DeJsonString<TransformOnline>(predictTransformJson);
        }
        Vector3 targetPosition = new Vector3(targetTransform.Vector3Position.X, targetTransform.Vector3Position.Y, targetTransform.Vector3Position.Z);
        Quaternion targetRotation = new Quaternion(targetTransform.QuaternionRotation.X, targetTransform.QuaternionRotation.Y, targetTransform.QuaternionRotation.Z, targetTransform.QuaternionRotation.W);
        MainGameSystem.Instance.playerCube.GetComponent<MovePlayerCubeController>().SetTransform(targetPosition, targetRotation);
    }

    public void SetAccount(string account)
    {
        Account = account;
    }
}
