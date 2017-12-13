//We check our messages from the IRC server for any of these codes.
[System.Serializable]
public class IRC_STATUS_CODES 
{

	public const string
	JOIN_STATUS_CODE = "001",
	INVALID_CMD_CODE = "423",
	MSG_CODE = "PRIVMSG",
	SERVER_REPLY_CODE = "PING";

}
