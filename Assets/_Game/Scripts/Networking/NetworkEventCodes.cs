using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkEventCodes
{
    public const byte ChangePlayerTeamEventCode = 1;
    public const byte LoaderEventCode = 2;
    public const byte SpawnPlayerEventCode = 3;
    public const byte GameStartsEventCode = 4;
    public const byte GameEndsWinEventCode = 5;
    public const byte GameEndsLoseEventCode = 6;
    public const byte TreeDrawEventCode = 7;
}
