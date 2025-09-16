using System.Globalization;
using Mirror;

public struct EnviromentMessage : NetworkMessage
{
    public string Action;
    public string[] Data; 
    public EnviromentMessage(string action, params string[] data)
    {
        Action = action;
        Data = data;
    } 
}
