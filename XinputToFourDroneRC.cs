using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;
using static OldUnityInputFourXboxToRCInput;

public class XinputToFourDroneRC : MonoBehaviour
{


    public PlayerInfo[] m_players = new PlayerInfo[]{
        new PlayerInfo(){ m_playerIndex = PlayerIndex.One },
        new PlayerInfo(){ m_playerIndex = PlayerIndex.Two },
        new PlayerInfo(){ m_playerIndex = PlayerIndex.Three },
        new PlayerInfo(){ m_playerIndex = PlayerIndex.Four }
    };
    public PlayerInfoEvent[] m_playersInfoEvent = new PlayerInfoEvent[]{
        new PlayerInfoEvent(){ m_playerIndex = PlayerIndex.One },
        new PlayerInfoEvent(){ m_playerIndex = PlayerIndex.Two },
        new PlayerInfoEvent(){ m_playerIndex = PlayerIndex.Three },
        new PlayerInfoEvent(){ m_playerIndex = PlayerIndex.Four }
    };

    [System.Serializable]
    public class PlayerInfo
    {
        public PlayerIndex m_playerIndex;
        public GamePadState m_gamepadState;
        public float m_downUP;
        public float m_rotateLeftRight;
        public float m_leftRight;
        public float m_backFront;
        public string m_compressVersionString;
        public float m_compressVersionFloat;
    }
    [System.Serializable]
    public class PlayerInfoEvent
    {
        public PlayerIndex m_playerIndex;
        public FloatEvent m_onDownUP;
        public FloatEvent m_onRotateLeftRight;
        public FloatEvent m_onLeftRight;
        public FloatEvent m_onBackFront;
    }

    void Update()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].m_gamepadState = GamePad.GetState(m_players[i].m_playerIndex);

            m_players[i].m_rotateLeftRight = m_players[i].m_gamepadState.ThumbSticks.Left.X;
            m_players[i].m_downUP = m_players[i].m_gamepadState.ThumbSticks.Left.Y;
            m_players[i].m_leftRight = m_players[i].m_gamepadState.ThumbSticks.Right.X;
            m_players[i].m_backFront = m_players[i].m_gamepadState.ThumbSticks.Right.Y;
            m_players[i].m_compressVersionString = string.Format("{0:00}{1:00}{2:00}{3:00}",
            Mathf.Clamp( (m_players[i].m_rotateLeftRight + 1f )  * 99f / 2f, 0f, 99f),
            Mathf.Clamp( (m_players[i].m_downUP + 1f )  * 99f / 2f, 0, 99),
            Mathf.Clamp( (m_players[i].m_leftRight + 1f ) * 99f / 2f, 0, 99),
            Mathf.Clamp( (m_players[i].m_backFront + 1f ) * 99f / 2f, 0, 99) );
            float.TryParse(m_players[i].m_compressVersionString, out m_players[i]. m_compressVersionFloat);


        }
        try { 
            for (int i = 0; i < m_players.Length; i++)
            {
          
                m_playersInfoEvent[i].m_onRotateLeftRight.Invoke( m_players[i].m_rotateLeftRight );
                m_playersInfoEvent[i].m_onDownUP.Invoke(m_players[i].m_downUP);
                m_playersInfoEvent[i].m_onLeftRight.Invoke(m_players[i].m_leftRight)   ;
                m_playersInfoEvent[i].m_onBackFront.Invoke(m_players[i].m_backFront)   ;
            }
        }catch(Exception e)
        {
            Debug.LogWarning("Some code are blocking here: "+ e.StackTrace);
        }
    }

}
