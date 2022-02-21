using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCS
{

    public enum eCommandType
    {
        Manual,
        Tcp_Server,
        Tcp_Server_P2P,
        Tcp_Client,
        Udp,
        Serial,
        Contact,
        Tcp_Server_Vive,
        Tcp_Server_KT,
        Tcp_Client_KT,
    }


    public enum eVariableType : int
    {
        DI = -3,
        DO = -2,
        None = -1,
        Yaw = 0,
        Pitch = 1,
        Roll = 2,
        Sway = 3,
        Surge = 4,
        Heave = 5,
        Speed = 6,
        Blower1 = 7,
        Blower2 = 8,
        Blower3 = 9,
        Blower4 = 10,
        Strobe = 11,
        Smoke = 12,
        Water = 13,

        Rolling = 41,
        RollingSpeed = 42,
        RollingMode = 43,
        Pitching = 44,
        PitchingSpeed = 45,
        PitchingMode = 46,
        Yawing = 47,
        YawingSpeed = 48,
        YawingMode = 49,

        Time = 19,
        AxisPos1 = 20,
        AxisSpeed1 = 21,
        AxisPos2 = 22,
        AxisSpeed2 = 23,
        AxisPos3 = 24,
        AxisSpeed3 = 25,
        AxisPos4 = 26,
        AxisSpeed4 = 27,
        AxisPos5 = 28,
        AxisSpeed5 = 29,
        AxisPos6 = 30,
        AxisSpeed6 = 31,
        AxisPos7 = 32,
        AxisSpeed7 = 33,
    }

    public enum eJoystickPart : int
    {
        X_Axix = 0,
        Y_Axix = 1,
        Z_Axix = 2,
        Throttle = 3,
        Hat = 4,
        Trigger = 5,
        Button1 = 6,
        Button2 = 7,
        Button3 = 8,
        Button4 = 9,
        Button5 = 10,
        Button6 = 11,
        Button7 = 12,
        Button8 = 13,
        Button9 = 14,
        Button10 = 15,
        Button11 = 19,
        Button12 = 20,
    }

    public enum eValueType
    {
        Value,
        Min,
        Max,
    }

    public enum eApiType
    {
        DOF_and_Blower = 0,
        V2_DOF_and_Blower = 1,
        V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis = 2,
        V2_DOF_and_Blower_and_Circling_and_DO_and_DI_and_Axis_and_Alarm = 3,
    }

    public enum eMove
    {
        Before10 = -10000,
        Before1 = -1000,
        After1 = 1000,
        After10 = 10000,
    }

    public enum eState
    {
        PlayStart,
        Playing,
        PlayEnd,
        Paused,
        OffsetStart,
        OffsetIng,
        OffsetComplete,
        PlayTime,
        ReadDiDo,
        ComOpen,
        WriteDo,
    }

    public enum eKeyCommandType
    {
        Start,
        Stop,
        StartStop,
        Pause,
        Exit,
        Load,
        Load_Config,
        Save_Config,
        Recenter,
    }

}

