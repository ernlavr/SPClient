using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    /// <summary>
    /// Address to which Python receives position updates
    /// </summary>
    public const string OSC_POS = "/position";
    /// <summary>
    /// Address to which Python receives rotation updates
    /// </summary>
    public const string OSC_ROT = "/rotation";
    /// <summary>
    /// Address to instruct Python to send available destinations
    /// </summary>
    public const string OSC_GET_DEST = "/getDestinations";
    /// <summary>
    /// Address to which Unity receives available navigation destinations
    /// </summary>
    public const string OSC_REC_DEST = "/availableDestinations";
    /// <summary>
    /// Address through which Python can be instructed of destinations to navigate to
    /// </summary>
    public const string OSC_SET_DEST = "/setDestinations";
    /// <summary>
    /// Address to which Unity receives guidance directions
    /// </summary>
    public const string OSC_GUIDE = "/guidanceDirection";
}