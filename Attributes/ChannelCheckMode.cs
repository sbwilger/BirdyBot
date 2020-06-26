using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Serana Wilger
/// 06/22/2020
/// ChannelCheckMode.cs
/// 
/// The enum for setting the mode ofchecking if a command is allowed in a given channel
/// </summary>

namespace sbwilger.BirdyBot.Attributes
{
    public enum ChannelCheckMode
    {
        Any = 0,
        None = 1,
        MineOrParentAny = 2
    }
}
