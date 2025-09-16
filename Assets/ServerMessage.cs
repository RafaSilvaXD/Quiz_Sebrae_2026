using Mirror;

public struct ServerMessage : NetworkMessage
{
    public string Action;

    public ServerMessage(string action)
    {
        Action = action;
    }
}
