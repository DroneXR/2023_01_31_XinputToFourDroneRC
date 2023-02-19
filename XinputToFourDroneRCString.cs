using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class XinputToFourDroneRCString : MonoBehaviour
{
    public Eloi.PrimitiveUnityEvent_DoubleString m_idAndCommandEvent;

    public enum RCType { RCFull, RCInt }
    public RCType m_rcCommandType;

    public PlayerInfo[] m_players = new PlayerInfo[]{
        new PlayerInfo(){ m_playerIndex = PlayerIndex.One , m_playerUniqueID = "XBOXXINPUT_"+ PlayerIndex.One},
        new PlayerInfo(){ m_playerIndex = PlayerIndex.Two , m_playerUniqueID = "XBOXXINPUT_"+ PlayerIndex.Two},
        new PlayerInfo(){ m_playerIndex = PlayerIndex.Three , m_playerUniqueID = "XBOXXINPUT_"+ PlayerIndex.Three},
        new PlayerInfo(){ m_playerIndex = PlayerIndex.Four, m_playerUniqueID = "XBOXXINPUT_"+ PlayerIndex.Four }
    };

    [System.Serializable]
    public class PlayerInfo
    {
        public string m_playerUniqueID;
        public PlayerIndex m_playerIndex;
        public GamePadState m_gamepadState;
        public string m_lastRCComamnds;
        public Eloi.PrimitiveUnityEvent_DoubleString m_idAndCommandEvent;
        public Vector4 m_debug;
    }

    void Update()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].m_debug.x = m_players[i].m_gamepadState.ThumbSticks.Left.X;
            m_players[i].m_debug.y = m_players[i].m_gamepadState.ThumbSticks.Left.Y;
            m_players[i].m_debug.z = m_players[i].m_gamepadState.ThumbSticks.Right.X;
            m_players[i].m_debug.w = m_players[i].m_gamepadState.ThumbSticks.Right.Y;
            m_players[i].m_gamepadState = GamePad.GetState(m_players[i].m_playerIndex);
            string cmd = m_rcCommandType == RCType.RCFull ? string.Format("rc {0} {1} {2} {3}",
            m_players[i].m_gamepadState.ThumbSticks.Left.X,
            m_players[i].m_gamepadState.ThumbSticks.Left.Y,
            m_players[i].m_gamepadState.ThumbSticks.Right.X,
            m_players[i].m_gamepadState.ThumbSticks.Right.Y):
            string.Format("rcf {0:00}{1:00}{2:00}{3:00}",
            Mathf.Clamp( (m_players[i].m_gamepadState.ThumbSticks.Left.X + 1f )  * 99f / 2f, 0f, 99f),
            Mathf.Clamp( (m_players[i].m_gamepadState.ThumbSticks.Left.Y + 1f )  * 99f / 2f, 0, 99),
            Mathf.Clamp( (m_players[i].m_gamepadState.ThumbSticks.Right.X + 1f ) * 99f / 2f, 0, 99),
            Mathf.Clamp( (m_players[i].m_gamepadState.ThumbSticks.Right.Y + 1f ) * 99f / 2f, 0, 99) );

            ;
            if (m_players[i].m_lastRCComamnds.Trim().Length == 0)
            {
                m_players[i].m_lastRCComamnds = cmd;
            }
            else if (m_players[i].m_lastRCComamnds != cmd)
            {
                m_players[i].m_lastRCComamnds = cmd;
                m_players[i].m_idAndCommandEvent.Invoke(m_players[i].m_playerUniqueID, m_players[i].m_lastRCComamnds);
                m_idAndCommandEvent.Invoke(m_players[i].m_playerUniqueID, m_players[i].m_lastRCComamnds);
            }

        }
    }

}
