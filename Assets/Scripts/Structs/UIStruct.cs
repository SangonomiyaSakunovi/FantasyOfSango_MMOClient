//Developer : SangonomiyaSakunovi
//Discription: 

public struct MessageStruct
{
    public string Message { get; set; }
    public TextColorCode TextColor { get; set; }

    public MessageStruct(string message, TextColorCode textColor) : this()
    {
        Message = message;
        TextColor = textColor;
    }
}
