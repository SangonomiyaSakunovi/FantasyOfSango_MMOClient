using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using SangoCommon.Structs;

//Developer : SangonomiyaSakunovi
//Discription: The Synv Transform Request.

public class SyncPlayerTransformRequest : BaseRequest
{
    public string Account { get; private set; }
    private TransformOnline playerTransform;

    public override void InitRequset()
    {
        base.InitRequset();
    }

    public void SetPlayerTransform(Vector3 position, Quaternion rotation)
    {
        playerTransform = new TransformOnline
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
        string playerTransformJson = SetJsonString(playerTransform);
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.PlayerTransform, playerTransformJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

    public void SetAccount(string account)
    {
        Account = account;
    }
}
