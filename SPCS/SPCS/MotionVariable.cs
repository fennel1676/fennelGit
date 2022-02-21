using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SPCS
{
    public class InfoSetting
    {
        [XmlElement(ElementName = "Path")]
        public string Path;

        [XmlElement(ElementName = "Sampling")]
        public int Sampling = 100;

        [XmlElement(ElementName = "YAxis")]
        public double YAxis = 2000;

        [XmlElement(ElementName = "FrameDisplay")]
        public int FrameDisplay = 0;
    }

    public class VariableSetting
    {
        [XmlAttribute(AttributeName = "Default")]
        public double Default = 0;

        [XmlAttribute(AttributeName = "Min")]
        public double Min = 0;

        [XmlAttribute(AttributeName = "Max")]
        public double Max = 0;

        [XmlAttribute(AttributeName = "Interval")]
        public double Interval = 0;

        public VariableSetting() { }

        public VariableSetting(double dDefault, double dMin, double dMax, double dInterval)
        {
            Default = dDefault;
            Min = dMin;
            Max = dMax;
            Interval = dInterval;
        }
    }

    public class TypeSetting
    {
        public List<eVariableType> Type;
    }

    public class MappingDatas
    {
        public List<MappingData> Data = new List<MappingData>();
    }

    public class MappingData
    {
        [XmlAttribute(AttributeName = "VariableType")]
        public string VariableType = string.Empty;

        [XmlAttribute(AttributeName = "JoystickPart")]
        public string JoystickPart = string.Empty;
    }

    public class VariableDatas
    {
        public List<VariableData> Data = new List<VariableData>();
    }

    public class VariableData
    {
        [XmlAttribute(AttributeName = "Key")]
        public string Key = string.Empty;

        [XmlAttribute(AttributeName = "Start")]
        public int Start;

        [XmlAttribute(AttributeName = "End")]
        public int End;

        [XmlAttribute(AttributeName = "Width")]
        public int Width;

        [XmlAttribute(AttributeName = "Height")]
        public int Height;

        public List<RangeData> RangeData;

        public List<SimulatorVariable> Variable;
    }

    public class RangeData
    {
        [XmlAttribute(AttributeName = "Key")]
        public string Key = string.Empty;

        [XmlAttribute(AttributeName = "Start")]
        public int Start;

        [XmlAttribute(AttributeName = "End")]
        public int End;

        [XmlAttribute(AttributeName = "Width")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = "Height")]
        public int Height { get; set; }

        [XmlIgnore]
        public Bitmap SelectImage { get; set; }

        [XmlIgnore]
        public Bitmap FirstImage { get; set; }

        public RangeData() { }

        public RangeData(string strKey, int nStart, int nEnd, int nWidth, int nHeight, Bitmap bmpFirst, Bitmap bmpSelect)
        {
            Key = strKey;
            Start = nStart;
            End = nEnd;
            Width = nWidth;
            Height = nHeight;
            FirstImage = bmpFirst;
            SelectImage = bmpSelect;
        }
    }

    [XmlRootAttribute("RecordFile")]
    public class RecordFileXml
    {
        [XmlElement(ElementName = "Path")]
        public string Path;

        [XmlElement(ElementName = "Sampling")]
        public int Sampling = 100;

        [XmlElement(ElementName = "YAxis_Part")]
        public double YAxis_Part = 2000;

        [XmlElement(ElementName = "YAxis_All")]
        public double YAxis_All = 5000;

        [XmlElement(ElementName = "Min")]
        public double Min = 0;

        [XmlElement(ElementName = "Max")]
        public double Max = 20000;

        [XmlElement(ElementName = "MinSpeed")]
        public double MinSpeed = 0;

        [XmlElement(ElementName = "MaxSpeed")]
        public double MaxSpeed = 255;

        [XmlElement(ElementName = "MinBlower")]
        public double MinBlower = 0;

        [XmlElement(ElementName = "MaxBlower")]
        public double MaxBlower = 100;

        [XmlElement(ElementName = "MinStrobe")]
        public double MinStrobe = 0;

        [XmlElement(ElementName = "MaxStrobe")]
        public double MaxStrobe = 1;

        [XmlElement(ElementName = "MinSmoke")]
        public double MinSmoke = 0;

        [XmlElement(ElementName = "MaxSmoke")]
        public double MaxSmoke = 1;

        [XmlElement(ElementName = "MinWater")]
        public double MinWater = 0;

        [XmlElement(ElementName = "MaxWater")]
        public double MaxWater = 1;

        [XmlElement(ElementName = "Default")]
        public double Default = 10000;

        [XmlElement(ElementName = "DefaultSpeed")]
        public double DefaultSpeed = 10;

        [XmlElement(ElementName = "DefaultBlower")]
        public double DefaultBlower = 0;

        [XmlElement(ElementName = "DefaultStrobe")]
        public double DefaultStrobe = 0;

        [XmlElement(ElementName = "DefaultSmoke")]
        public double DefaultSmoke = 0;

        [XmlElement(ElementName = "DefaultWater")]
        public double DefaultWater = 0;

        [XmlElement(ElementName = "UseType")]
        public List<eVariableType> Type = new List<eVariableType>();

        [XmlElement(ElementName = "Data")]
        public SimulatorVariable[] Data;
    }

    [XmlRootAttribute("RecordFile")]
    public class SimulatorRecordFileXml
    {
        [XmlElement(ElementName = "Info")]
        public InfoSetting Info = new InfoSetting();

        [XmlElement(ElementName = "Dof")]
        public VariableSetting Dof = new VariableSetting(10000, 0, 20000, 2000);

        [XmlElement(ElementName = "Speed")]
        public VariableSetting Speed = new VariableSetting(10, 0, 255, 50);

        [XmlElement(ElementName = "Blower")]
        public VariableSetting Blower = new VariableSetting(0, 0, 100, 10);

        [XmlElement(ElementName = "Do")]
        public VariableSetting Do = new VariableSetting(0, 0, 1, 1);

        [XmlElement(ElementName = "AxisPos")]
        public VariableSetting AxisPos = new VariableSetting(10000, 0, 20000, 2000);

        [XmlElement(ElementName = "AxisSpeed")]
        public VariableSetting AxisSpeed = new VariableSetting(10, 0, 255, 50);

        [XmlElement(ElementName = "UseType")]
        public TypeSetting Types = new TypeSetting();

        [XmlElement(ElementName = "Datas")]
        public VariableDatas Datas = new VariableDatas();

        [XmlElement(ElementName = "Joystick")]
        public MappingDatas Joystick = new MappingDatas();
    }

    [XmlRootAttribute("Setting")]
    public class SimulatorVariableSettingXml
    {
        [XmlElement(ElementName = "Value")]
        public SimulatorVariable Value = new SimulatorVariable(0.0);

        [XmlElement(ElementName = "Min")]
        public SimulatorVariable Min = new SimulatorVariable(0.0);

        [XmlElement(ElementName = "Max")]
        public SimulatorVariable Max = new SimulatorVariable(0.0);

        public SimulatorVariableSettingXml() { }
    }

    public class SimulatorVariable
    {
        [XmlIgnore]
        public static uint DO;
        [XmlIgnore]
        public static uint DI;

        [XmlAttribute(AttributeName = "Range")]
        public string Range;
        [XmlAttribute(AttributeName = "None")]
        public double None;
        [XmlAttribute(AttributeName = "Time")]
        public double Time;
        [XmlAttribute(AttributeName = "Yaw")]
        public double Yaw;
        [XmlAttribute(AttributeName = "Pitch")]
        public double Pitch;
        [XmlAttribute(AttributeName = "Roll")]
        public double Roll;
        [XmlAttribute(AttributeName = "Sway")]
        public double Sway;
        [XmlAttribute(AttributeName = "Surge")]
        public double Surge;
        [XmlAttribute(AttributeName = "Heave")]
        public double Heave;
        [XmlAttribute(AttributeName = "Speed")]
        public double Speed;
        [XmlAttribute(AttributeName = "Blower1")]
        public double Blower1;
        [XmlAttribute(AttributeName = "Blower2")]
        public double Blower2;
        [XmlAttribute(AttributeName = "Blower3")]
        public double Blower3;
        [XmlAttribute(AttributeName = "Blower4")]
        public double Blower4;
        [XmlAttribute(AttributeName = "Strobe")]
        public double Strobe;
        [XmlAttribute(AttributeName = "Smoke")]
        public double Smoke;
        [XmlAttribute(AttributeName = "Water")]
        public double Water;

        [XmlAttribute(AttributeName = "Rolling")]
        public double Rolling;
        [XmlAttribute(AttributeName = "RollingSpeed")]
        public double RollingSpeed;
        [XmlAttribute(AttributeName = "RollingMode")]
        public double RollingMode;
        [XmlAttribute(AttributeName = "Pitching")]
        public double Pitching;
        [XmlAttribute(AttributeName = "PitchingSpeed")]
        public double PitchingSpeed;
        [XmlAttribute(AttributeName = "PitchingMode")]
        public double PitchingMode;
        [XmlAttribute(AttributeName = "Yawing")]
        public double Yawing;
        [XmlAttribute(AttributeName = "YawingSpeed")]
        public double YawingSpeed;
        [XmlAttribute(AttributeName = "YawingMode")]
        public double YawingMode;

        [XmlAttribute(AttributeName = "AxisPos1")]
        public double AxisPos1;
        [XmlAttribute(AttributeName = "AxisSpeed1")]
        public double AxisSpeed1;
        [XmlAttribute(AttributeName = "AxisPos2")]
        public double AxisPos2;
        [XmlAttribute(AttributeName = "AxisSpeed2")]
        public double AxisSpeed2;
        [XmlAttribute(AttributeName = "AxisPos3")]
        public double AxisPos3;
        [XmlAttribute(AttributeName = "AxisSpeed3")]
        public double AxisSpeed3;
        [XmlAttribute(AttributeName = "AxisPos4")]
        public double AxisPos4;
        [XmlAttribute(AttributeName = "AxisSpeed4")]
        public double AxisSpeed4;
        [XmlAttribute(AttributeName = "AxisPos5")]
        public double AxisPos5;
        [XmlAttribute(AttributeName = "AxisSpeed5")]
        public double AxisSpeed5;
        [XmlAttribute(AttributeName = "AxisPos6")]
        public double AxisPos6;
        [XmlAttribute(AttributeName = "AxisSpeed6")]
        public double AxisSpeed6;
        [XmlAttribute(AttributeName = "AxisPos7")]
        public double AxisPos7;
        [XmlAttribute(AttributeName = "AxisSpeed7")]
        public double AxisSpeed7;

        private double m_dDefault = 10000;
        private double m_dSpeedDefault = 10;
        private double m_dBlowerDefault = 0;
        private double m_dStrobeDefault = 0;
        private double m_dSmokeDefault = 0;
        private double m_dWaterDefault = 0;

        public SimulatorVariable() { }

        public SimulatorVariable(double dDefault, string strRange = null)
        {
            Range = strRange;

            m_dDefault = dDefault;
            m_dSpeedDefault = dDefault;
            m_dBlowerDefault = dDefault;
            m_dStrobeDefault = dDefault;
            m_dSmokeDefault = dDefault;
            m_dWaterDefault = dDefault;

            Time = 0;
            Yaw = dDefault;
            Pitch = dDefault;
            Roll = dDefault;
            Sway = dDefault;
            Surge = dDefault;
            Heave = dDefault;
            Speed = dDefault;
            Blower1 = dDefault;
            Blower2 = dDefault;
            Blower3 = dDefault;
            Blower4 = dDefault;
            Strobe = dDefault;
            Smoke = dDefault;
            Water = dDefault;

            Rolling = dDefault;
            RollingSpeed = dDefault;
            RollingMode = 0;
            Pitching = dDefault;
            PitchingSpeed = dDefault;
            PitchingMode = 0;
            Yawing = dDefault;
            YawingSpeed = dDefault;
            YawingMode = 0;

            None = dDefault;
            AxisPos1 = dDefault;
            AxisSpeed1 = dDefault;
            AxisPos2 = dDefault;
            AxisSpeed2 = dDefault;
            AxisPos3 = dDefault;
            AxisSpeed3 = dDefault;
            AxisPos4 = dDefault;
            AxisSpeed4 = dDefault;
            AxisPos5 = dDefault;
            AxisSpeed5 = dDefault;
            AxisPos6 = dDefault;
            AxisSpeed6 = dDefault;
            AxisPos7 = dDefault;
            AxisSpeed7 = dDefault;
        }

        public SimulatorVariable(double dDefault, double dSpeedDefault, double dBlowerDefault,
                                double dDoDefault, string strRange = null)
        {
            Range = strRange;

            m_dDefault = dDefault;
            m_dSpeedDefault = dSpeedDefault;
            m_dBlowerDefault = dBlowerDefault;
            m_dStrobeDefault = dDoDefault;
            m_dSmokeDefault = dDoDefault;
            m_dWaterDefault = dDoDefault;

            Time = 0;
            Yaw = dDefault;
            Pitch = dDefault;
            Roll = dDefault;
            Sway = dDefault;
            Surge = dDefault;
            Heave = dDefault;
            Speed = dSpeedDefault;
            Blower1 = dBlowerDefault;
            Blower2 = dBlowerDefault;
            Blower3 = dBlowerDefault;
            Blower4 = dBlowerDefault;
            Strobe = dDoDefault;
            Smoke = dDoDefault;
            Water = dDoDefault;

            Rolling = dDefault;
            RollingSpeed = dSpeedDefault;
            RollingMode = 0;
            Pitching = dDefault;
            PitchingSpeed = dSpeedDefault;
            PitchingMode = 0;
            Yawing = dDefault;
            YawingSpeed = dSpeedDefault;
            YawingMode = 0;

            None = dDefault;
            AxisPos1 = dDefault;
            AxisSpeed1 = dSpeedDefault;
            AxisPos2 = dDefault;
            AxisSpeed2 = dSpeedDefault;
            AxisPos3 = dDefault;
            AxisSpeed3 = dSpeedDefault;
            AxisPos4 = dDefault;
            AxisSpeed4 = dSpeedDefault;
            AxisPos5 = dDefault;
            AxisSpeed5 = dSpeedDefault;
            AxisPos6 = dDefault;
            AxisSpeed6 = dSpeedDefault;
            AxisPos7 = dDefault;
            AxisSpeed7 = dSpeedDefault;
        }

        public SimulatorVariable(double dDefault, double dSpeedDefault, double dBlowerDefault,
                                double dStrobeDefault, double dSmokeDefault, double dWaterDefault, string strRange = null)
        {
            Range = strRange;

            m_dDefault = dDefault;
            m_dSpeedDefault = dSpeedDefault;
            m_dBlowerDefault = dBlowerDefault;
            m_dStrobeDefault = dStrobeDefault;
            m_dSmokeDefault = dSmokeDefault;
            m_dWaterDefault = dWaterDefault;

            Time = 0;
            Yaw = dDefault;
            Pitch = dDefault;
            Roll = dDefault;
            Sway = dDefault;
            Surge = dDefault;
            Heave = dDefault;
            Speed = dSpeedDefault;
            Blower1 = dBlowerDefault;
            Blower2 = dBlowerDefault;
            Blower3 = dBlowerDefault;
            Blower4 = dBlowerDefault;
            Strobe = dStrobeDefault;
            Smoke = dSmokeDefault;
            Water = dWaterDefault;

            Rolling = dDefault;
            RollingSpeed = dSpeedDefault;
            RollingMode = 0;
            Pitching = dDefault;
            PitchingSpeed = dSpeedDefault;
            PitchingMode = 0;
            Yawing = dDefault;
            YawingSpeed = dSpeedDefault;
            YawingMode = 0;

            None = dDefault;
            AxisPos1 = dDefault;
            AxisSpeed1 = dSpeedDefault;
            AxisPos2 = dDefault;
            AxisSpeed2 = dSpeedDefault;
            AxisPos3 = dDefault;
            AxisSpeed3 = dSpeedDefault;
            AxisPos4 = dDefault;
            AxisSpeed4 = dSpeedDefault;
            AxisPos5 = dDefault;
            AxisSpeed5 = dSpeedDefault;
            AxisPos6 = dDefault;
            AxisSpeed6 = dSpeedDefault;
            AxisPos7 = dDefault;
            AxisSpeed7 = dSpeedDefault;
        }

        public SimulatorVariable(SimulatorVariable simulatorVariable)
        {
            SetSimulatorVariable(simulatorVariable);
        }

        public void Clear()
        {
            Time = 0;
            Yaw = m_dDefault;
            Pitch = m_dDefault;
            Roll = m_dDefault;
            Sway = m_dDefault;
            Surge = m_dDefault;
            Heave = m_dDefault;
            Speed = m_dSpeedDefault;
            Blower1 = m_dBlowerDefault;
            Blower2 = m_dBlowerDefault;
            Blower3 = m_dBlowerDefault;
            Blower4 = m_dBlowerDefault;
            Strobe = m_dStrobeDefault;
            Smoke = m_dSmokeDefault;
            Water = m_dWaterDefault;

            Rolling = m_dDefault;
            RollingSpeed = m_dSpeedDefault;
            RollingMode = 0;
            Pitching = m_dDefault;
            PitchingSpeed = m_dSpeedDefault;
            PitchingMode = 0;
            Yawing = m_dDefault;
            YawingSpeed = m_dSpeedDefault;
            YawingMode = 0;

            None = m_dDefault;
            AxisPos1 = m_dDefault;
            AxisSpeed1 = m_dSpeedDefault;
            AxisPos2 = m_dDefault;
            AxisSpeed2 = m_dSpeedDefault;
            AxisPos3 = m_dDefault;
            AxisSpeed3 = m_dSpeedDefault;
            AxisPos4 = m_dDefault;
            AxisSpeed4 = m_dSpeedDefault;
            AxisPos5 = m_dDefault;
            AxisSpeed5 = m_dSpeedDefault;
            AxisPos6 = m_dDefault;
            AxisSpeed6 = m_dSpeedDefault;
            AxisPos7 = m_dDefault;
            AxisSpeed7 = m_dSpeedDefault;
        }

        public void SetSimulatorVariable(SimulatorVariable simulatorVariable)
        {
            Time = simulatorVariable.Time;
            Yaw = simulatorVariable.Yaw;
            Pitch = simulatorVariable.Pitch;
            Roll = simulatorVariable.Roll;
            Sway = simulatorVariable.Sway;
            Surge = simulatorVariable.Surge;
            Heave = simulatorVariable.Heave;
            Speed = simulatorVariable.Speed;
            Blower1 = simulatorVariable.Blower1;
            Blower2 = simulatorVariable.Blower2;
            Blower3 = simulatorVariable.Blower3;
            Blower4 = simulatorVariable.Blower4;
            Strobe = simulatorVariable.Strobe;
            Smoke = simulatorVariable.Smoke;
            Water = simulatorVariable.Water;

            Rolling = simulatorVariable.Rolling;
            RollingSpeed = simulatorVariable.RollingSpeed;
            RollingMode = simulatorVariable.RollingMode;
            Pitching = simulatorVariable.Pitching;
            PitchingSpeed = simulatorVariable.PitchingSpeed;
            PitchingMode = simulatorVariable.PitchingMode;
            Yawing = simulatorVariable.Yawing;
            YawingSpeed = simulatorVariable.YawingSpeed;
            YawingMode = simulatorVariable.YawingSpeed;

            None = simulatorVariable.None;
            AxisPos1 = simulatorVariable.AxisPos1;
            AxisSpeed1 = simulatorVariable.AxisSpeed1;
            AxisPos2 = simulatorVariable.AxisPos2;
            AxisSpeed2 = simulatorVariable.AxisSpeed2;
            AxisPos3 = simulatorVariable.AxisPos3;
            AxisSpeed3 = simulatorVariable.AxisSpeed3;
            AxisPos4 = simulatorVariable.AxisPos4;
            AxisSpeed4 = simulatorVariable.AxisSpeed4;
            AxisPos5 = simulatorVariable.AxisPos5;
            AxisSpeed5 = simulatorVariable.AxisSpeed5;
            AxisPos6 = simulatorVariable.AxisPos6;
            AxisSpeed6 = simulatorVariable.AxisSpeed6;
            AxisPos7 = simulatorVariable.AxisPos7;
            AxisSpeed7 = simulatorVariable.AxisSpeed7;
        }

        public double this[eVariableType e]
        {
            get
            {
                if (eVariableType.Yaw == e) return Yaw;
                else if (eVariableType.Pitch == e) return Pitch;
                else if (eVariableType.Roll == e) return Roll;
                else if (eVariableType.Sway == e) return Sway;
                else if (eVariableType.Surge == e) return Surge;
                else if (eVariableType.Heave == e) return Heave;
                else if (eVariableType.Speed == e) return Speed;
                else if (eVariableType.Blower1 == e) return Blower1;
                else if (eVariableType.Blower2 == e) return Blower2;
                else if (eVariableType.Blower3 == e) return Blower3;
                else if (eVariableType.Blower4 == e) return Blower4;
                else if (eVariableType.Strobe == e) return Strobe;
                else if (eVariableType.Smoke == e) return Smoke;
                else if (eVariableType.Water == e) return Water;

                else if (eVariableType.Rolling == e) return Rolling;
                else if (eVariableType.RollingSpeed == e) return RollingSpeed;
                else if (eVariableType.RollingMode == e) return RollingMode;
                else if (eVariableType.Pitching == e) return Pitching;
                else if (eVariableType.PitchingSpeed == e) return PitchingSpeed;
                else if (eVariableType.PitchingMode == e) return PitchingMode;
                else if (eVariableType.Yawing == e) return Yawing;
                else if (eVariableType.YawingSpeed == e) return YawingSpeed;
                else if (eVariableType.YawingMode == e) return YawingMode;

                else if (eVariableType.None == e) return None;
                else if (eVariableType.Time == e) return Time;
                else if (eVariableType.AxisPos1 == e) return AxisPos1;
                else if (eVariableType.AxisSpeed1 == e) return AxisSpeed1;
                else if (eVariableType.AxisPos2 == e) return AxisPos2;
                else if (eVariableType.AxisSpeed2 == e) return AxisSpeed2;
                else if (eVariableType.AxisPos3 == e) return AxisPos3;
                else if (eVariableType.AxisSpeed3 == e) return AxisSpeed3;
                else if (eVariableType.AxisPos4 == e) return AxisPos4;
                else if (eVariableType.AxisSpeed4 == e) return AxisSpeed4;
                else if (eVariableType.AxisPos5 == e) return AxisPos5;
                else if (eVariableType.AxisSpeed5 == e) return AxisSpeed5;
                else if (eVariableType.AxisPos6 == e) return AxisPos6;
                else if (eVariableType.AxisSpeed6 == e) return AxisSpeed6;
                else if (eVariableType.AxisPos7 == e) return AxisPos7;
                else if (eVariableType.AxisSpeed7 == e) return AxisSpeed7;
                else if (eVariableType.DI == e) return DI;
                else if (eVariableType.DO == e) return DO;
                else return 0.0;
            }

            set
            {
                if (eVariableType.Yaw == e) Yaw = value;
                else if (eVariableType.Pitch == e) Pitch = value;
                else if (eVariableType.Roll == e) Roll = value;
                else if (eVariableType.Sway == e) Sway = value;
                else if (eVariableType.Surge == e) Surge = value;
                else if (eVariableType.Heave == e) Heave = value;
                else if (eVariableType.Speed == e) Speed = value;
                else if (eVariableType.Blower1 == e) Blower1 = value;
                else if (eVariableType.Blower2 == e) Blower2 = value;
                else if (eVariableType.Blower3 == e) Blower3 = value;
                else if (eVariableType.Blower4 == e) Blower4 = value;
                else if (eVariableType.Strobe == e) Strobe = value;
                else if (eVariableType.Smoke == e) Smoke = value;
                else if (eVariableType.Water == e) Water = value;

                else if (eVariableType.Rolling == e) Rolling = value;
                else if (eVariableType.RollingSpeed == e) RollingSpeed = value;
                else if (eVariableType.RollingMode == e) RollingMode = value;
                else if (eVariableType.Pitching == e) Pitching = value;
                else if (eVariableType.PitchingSpeed == e) PitchingSpeed = value;
                else if (eVariableType.PitchingMode == e) PitchingMode = value;
                else if (eVariableType.Yawing == e) Yawing = value;
                else if (eVariableType.YawingSpeed == e) YawingSpeed = value;
                else if (eVariableType.YawingMode == e) YawingMode = value;

                else if (eVariableType.None == e) None = value;
                else if (eVariableType.Time == e) Time = value;
                else if (eVariableType.AxisPos1 == e) AxisPos1 = value;
                else if (eVariableType.AxisSpeed1 == e) AxisSpeed1 = value;
                else if (eVariableType.AxisPos2 == e) AxisPos2 = value;
                else if (eVariableType.AxisSpeed2 == e) AxisSpeed2 = value;
                else if (eVariableType.AxisPos3 == e) AxisPos3 = value;
                else if (eVariableType.AxisSpeed3 == e) AxisSpeed3 = value;
                else if (eVariableType.AxisPos4 == e) AxisPos4 = value;
                else if (eVariableType.AxisSpeed4 == e) AxisSpeed4 = value;
                else if (eVariableType.AxisPos5 == e) AxisPos5 = value;
                else if (eVariableType.AxisSpeed5 == e) AxisSpeed5 = value;
                else if (eVariableType.AxisPos6 == e) AxisPos6 = value;
                else if (eVariableType.AxisSpeed6 == e) AxisSpeed6 = value;
                else if (eVariableType.AxisPos7 == e) AxisPos7 = value;
                else if (eVariableType.AxisSpeed7 == e) AxisSpeed7 = value;
                else if (eVariableType.DI == e) DI = (uint)value;
                else if (eVariableType.DO == e) DO = (uint)value;
            }
        }
    }
}

