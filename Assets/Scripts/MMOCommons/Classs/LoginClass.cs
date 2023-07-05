using System;

//Developer: SangonomiyaSakunovi

[Serializable]
public class LoginReq
{
    public string Account { get; set; }
    public string Password { get; set; }
}

[Serializable]
public class RegisterReq
{
    public string Account { get; set; }
    public string Password { get; set; }
    public string Nickname { get; set; }
}
