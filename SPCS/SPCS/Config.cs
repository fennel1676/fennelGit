using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPCS
{

    [XmlRootAttribute("Config")]
    public class ConfigXml
    {

        [XmlElement(ElementName = "Mode")]
        public int Mode = 0;

        [XmlElement(ElementName = "EM")]
        public int EM = 0;

        [XmlElement(ElementName = "TactTime")]
        public int TactTime;


        [XmlElement(ElementName = "PerTime")]
        public int PerTime;


        [XmlElement(ElementName = "Conveyor1")]
        public ConvParameter Conveyor1;


        [XmlElement(ElementName = "Conveyor2")]
        public ConvParameter Conveyor2;

        [XmlElement(ElementName = "ConveyorChain")]
        public ConvChainParameter ConveyorChain;


        public class ConvParameter
        {

            [XmlAttribute(AttributeName = "RPM")]
            public string RPM;

            [XmlAttribute(AttributeName = "BoxCount")]
            public string BoxCount;

            [XmlAttribute(AttributeName = "StopRPM5")]
            public string StopRPM5;

            [XmlAttribute(AttributeName = "StopRPM4")]
            public string StopRPM4;

            [XmlAttribute(AttributeName = "StopRPM3")]
            public string StopRPM3;

            [XmlAttribute(AttributeName = "StopRPM2")]
            public string StopRPM2;

            [XmlAttribute(AttributeName = "StopRPM1")]
            public string StopRPM1;

            [XmlAttribute(AttributeName = "RepeatDelayTime")]
            public string RepeatDelayTime;

            [XmlAttribute(AttributeName = "ManualRPM")]
            public string ManualRPM;

        }

        public class ConvChainParameter
        {

            [XmlAttribute(AttributeName = "BlockPosOffset")]
            public string RPM;

            [XmlAttribute(AttributeName = "RPM")]
            public string BoxCount;

            [XmlAttribute(AttributeName = "DelayTime")]
            public string StopRPM5;

            [XmlAttribute(AttributeName = "ManualRPM")]
            public string StopRPM4;

        }
    }
}
