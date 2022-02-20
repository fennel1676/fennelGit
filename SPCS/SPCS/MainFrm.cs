using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Configuration;
//using System.Reflection.Assembly;

//using AvSimMotion;
using HNS.Util;
using MotionControl.Communication;

namespace SPCS
{
    public partial class MainFrm : Form
    {
        //private bool m_isInitialize = false;

        //private string m_strFileName;
        private ConfigXml m_xmlConfig;
        // private List<eVariableType> m_listVariableType = new List<eVariableType>();
        //private List<SimulatorVariable> m_listSimulatorVariable = new List<SimulatorVariable>();

        private AppSettingsReader m_optConfig;
        public MotionControl.Communication.Command m_ComObject = null;



        private delegate void CallbackEqEvtControlDeleGate(EqEventArgs e);
        private delegate void DelegatePlayStopLoadKeepAlive();

        private int m_comPort;
        private int m_nTimeout;
        private string m_sLastFile;

        // private eCommandType m_eCommandType = eCommandType.Manual;

        public MainFrm()
        {
            InitializeComponent();

            m_ComObject = new Command();
            m_ComObject.OnEqReceived += new MotionControl.Communication.Command.EqReceivedEventd(OnEqEvent);


            InitSetting();

        }


        private void InitSetting()
        {
            m_optConfig = new AppSettingsReader();
            m_comPort = (int)m_optConfig.GetValue("Comport", typeof(int));
            m_nTimeout = (int)m_optConfig.GetValue("Timeout", typeof(int));
            m_sLastFile = (string)m_optConfig.GetValue("LastFile", typeof(string));


            if (LoadConfig(m_sLastFile))
            {


                string[] comlist = System.IO.Ports.SerialPort.GetPortNames();

                //COM Port가 있는 경우에만 콤보 박스에 추가.
                if (comlist.Length > 0)
                {
                    toolStripComboBox2.Items.AddRange(comlist);
                    toolStripComboBox2.SelectedIndex = 0;
                }
            }

        }


        //private void OnEqEvent(object sender, EqEventArgs args)
        //{
        //    switch (args.EventType)
        //    {
        //        case eEqEventType.EqStatus: ProcessEqStatus(args); break;
        //        //case eEqEventType.ReadBoardingPassEvent: ProcessEqBPassEvent(args); break;
        //        //case eEqEventType.ReadFQTVCardEvent: ProcessEqMCardMSREvent(args); break;
        //        //case eEqEventType.ReadCreditCardEvent: ProcessEqCCardMSREvent(args); break;
        //    }
        //}
        private void OnEqEvent( EqEventArgs args)
        {
            switch (args.EventType)
            {
                case eEqEventType.EqStatus:                 ProcessEqStatus(args); break;
                case eEqEventType.EqConv1RpmStatus:         ProcessEqConv1RpmStatus(args); break;
                case eEqEventType.EqConv1BoxCountStatus:    ProcessEqConv1BoxCountStatus(args); break;
                    //case eEqEventType.ReadBoardingPassEvent: ProcessEqBPassEvent(args); break;
                    //case eEqEventType.ReadFQTVCardEvent: ProcessEqMCardMSREvent(args); break;
                    //case eEqEventType.ReadCreditCardEvent: ProcessEqCCardMSREvent(args); break;
            }
        }


        private void ProcessEqStatus(EqEventArgs e)
        {
            if (this.InvokeRequired)
            {
                CallbackEqEvtControlDeleGate d = new CallbackEqEvtControlDeleGate(ProcessEqStatus);
                this.Invoke(d, new object[] { e });
            }
            else
            {
                bool recv = (bool)e.EventData;


                this.label1.Text = string.Format("{0}", recv);



                //if (recv != null)
                //{
                //    if (recv.bConnect)
                //    {
                //        eqStatus = EqState.Connect;

                //        IntPtr Handle1 = (IntPtr)FindWindow(null, "READER ERROR");
                //        if (Handle1 != (IntPtr)0)
                //        {
                //            SendMessage(Handle1, WM_CLOSE, 0, 0);
                //        }
                //    }
                //    else
                //    {
                //        eqStatus = EqState.Closed;
                //        IntPtr Handle1 = (IntPtr)FindWindow(null, "READER ERROR");
                //        if (Handle1 == (IntPtr)0)
                //        {
                //            string message = string.Format("{0}\n{1}\n{2}\n{3}",
                //                "시스템이 자동으로 재연결을 시도중입니다.",
                //                "리더기가 연결되지 않더라도",
                //                "수동입력으로 심사를 진행 하실 수 있습니다",
                //                "문제가 지속될 경우 관리자에게 문의해 주세요");
                //            ComErrFrm frm = new ComErrFrm("READER ERROR", "[리더기 DISCONNECTION]", message);
                //            frm.Show();
                //        }
                //    }
                //    picReaderStatus.Image = eqStatus.Equals(EqState.Connect) ? Image.FromFile(@"Image\ic_reader_on.gif") : Image.FromFile(@"Image\ic_reader_off.gif");
                //    sqlManager1.SetErrorInfo(CommonData.LOCAL_IP, lbVersion.Text, CommonData.UInfo.sLoungeID, recv.sErrCode, recv.sErrMsg);
                //    logManager1.WriteLog("[EQ STATUS]" + eqStatus.ToString());
                //}
            }
        }
        private void ProcessEqConv1RpmStatus(EqEventArgs e)
        {
            if (this.InvokeRequired)
            {
                CallbackEqEvtControlDeleGate d = new CallbackEqEvtControlDeleGate(ProcessEqConv1RpmStatus);
                this.Invoke(d, new object[] { e });
            }
            else
            {
                int recv = (int)e.EventData;
                m_xmlConfig.Conveyor1.RPM = Convert.ToString((int)e.EventData);

              //  this.label1.Text = string.Format("{0}", recv);



                //if (recv != null)
                //{
                //    if (recv.bConnect)
                //    {
                //        eqStatus = EqState.Connect;

                //        IntPtr Handle1 = (IntPtr)FindWindow(null, "READER ERROR");
                //        if (Handle1 != (IntPtr)0)
                //        {
                //            SendMessage(Handle1, WM_CLOSE, 0, 0);
                //        }
                //    }
                //    else
                //    {
                //        eqStatus = EqState.Closed;
                //        IntPtr Handle1 = (IntPtr)FindWindow(null, "READER ERROR");
                //        if (Handle1 == (IntPtr)0)
                //        {
                //            string message = string.Format("{0}\n{1}\n{2}\n{3}",
                //                "시스템이 자동으로 재연결을 시도중입니다.",
                //                "리더기가 연결되지 않더라도",
                //                "수동입력으로 심사를 진행 하실 수 있습니다",
                //                "문제가 지속될 경우 관리자에게 문의해 주세요");
                //            ComErrFrm frm = new ComErrFrm("READER ERROR", "[리더기 DISCONNECTION]", message);
                //            frm.Show();
                //        }
                //    }
                //    picReaderStatus.Image = eqStatus.Equals(EqState.Connect) ? Image.FromFile(@"Image\ic_reader_on.gif") : Image.FromFile(@"Image\ic_reader_off.gif");
                //    sqlManager1.SetErrorInfo(CommonData.LOCAL_IP, lbVersion.Text, CommonData.UInfo.sLoungeID, recv.sErrCode, recv.sErrMsg);
                //    logManager1.WriteLog("[EQ STATUS]" + eqStatus.ToString());
                //}
            }
        }
        private void ProcessEqConv1BoxCountStatus(EqEventArgs e)
        {
            if (this.InvokeRequired)
            {
                CallbackEqEvtControlDeleGate d = new CallbackEqEvtControlDeleGate(ProcessEqConv1BoxCountStatus);
                this.Invoke(d, new object[] { e });
            }
            else
            {
                int recv = (int)e.EventData;


                this.label1.Text = string.Format("{0}", recv);



                //if (recv != null)
                //{
                //    if (recv.bConnect)
                //    {
                //        eqStatus = EqState.Connect;

                //        IntPtr Handle1 = (IntPtr)FindWindow(null, "READER ERROR");
                //        if (Handle1 != (IntPtr)0)
                //        {
                //            SendMessage(Handle1, WM_CLOSE, 0, 0);
                //        }
                //    }
                //    else
                //    {
                //        eqStatus = EqState.Closed;
                //        IntPtr Handle1 = (IntPtr)FindWindow(null, "READER ERROR");
                //        if (Handle1 == (IntPtr)0)
                //        {
                //            string message = string.Format("{0}\n{1}\n{2}\n{3}",
                //                "시스템이 자동으로 재연결을 시도중입니다.",
                //                "리더기가 연결되지 않더라도",
                //                "수동입력으로 심사를 진행 하실 수 있습니다",
                //                "문제가 지속될 경우 관리자에게 문의해 주세요");
                //            ComErrFrm frm = new ComErrFrm("READER ERROR", "[리더기 DISCONNECTION]", message);
                //            frm.Show();
                //        }
                //    }
                //    picReaderStatus.Image = eqStatus.Equals(EqState.Connect) ? Image.FromFile(@"Image\ic_reader_on.gif") : Image.FromFile(@"Image\ic_reader_off.gif");
                //    sqlManager1.SetErrorInfo(CommonData.LOCAL_IP, lbVersion.Text, CommonData.UInfo.sLoungeID, recv.sErrCode, recv.sErrMsg);
                //    logManager1.WriteLog("[EQ STATUS]" + eqStatus.ToString());
                //}
            }
        }
        //public bool SaveConfig(string strPath = "config.xml")
        //{
        //    ConfigXml xmlConfig = new ConfigXml();
        //    xmlConfig.Offset = int.Parse(m_toolStripComboBoxOffset.Text);
        //    xmlConfig.Cycle = int.Parse(m_comboBoxCycleTime.Text);
        //    xmlConfig.CommandType = m_toolStripComboBoxCommand.Text;
        //    xmlConfig.File = m_strFileName;

        //    if (eCommandType.Contact == m_eCommandType)
        //    {
        //        xmlConfig.Serial = new ConfigXml.SerialSetting();
        //        xmlConfig.Serial.ComPort = m_communicationContact.Port;
        //    }
        //    else if (eCommandType.Serial == m_eCommandType)
        //    {
        //        xmlConfig.Serial = new ConfigXml.SerialSetting();
        //        xmlConfig.Serial.ComPort = m_communicationSerial.Port;
        //        xmlConfig.Serial.Baudrate = m_communicationSerial.Baudrate;
        //    }
        //    else if (eCommandType.Tcp_Server == m_eCommandType && null != m_tcpListener)
        //    {
        //        xmlConfig.Ethernet = new ConfigXml.EthernetSetting();
        //        IPEndPoint iep = (IPEndPoint)m_tcpListener.Server.LocalEndPoint;
        //        xmlConfig.Ethernet.Port = iep.Port;

        //    }
        //    else if (eCommandType.Tcp_Server_P2P == m_eCommandType && null != m_tcpListener)
        //    {
        //        xmlConfig.Ethernet = new ConfigXml.EthernetSetting();
        //        IPEndPoint iep = (IPEndPoint)m_tcpListener.Server.LocalEndPoint;
        //        xmlConfig.Ethernet.Port = iep.Port;

        //    }
        //    else if (eCommandType.Tcp_Client == m_eCommandType && null != m_tcpClient)
        //    {
        //        xmlConfig.Ethernet = new ConfigXml.EthernetSetting();
        //        IPEndPoint iep = (IPEndPoint)m_tcpClient.Client.RemoteEndPoint;
        //        xmlConfig.Ethernet.IP = iep.Address.ToString();
        //        xmlConfig.Ethernet.Port = iep.Port;
        //    }
        //    else if (eCommandType.Udp == m_eCommandType && null != m_udpClient)
        //    {
        //        xmlConfig.Ethernet = new ConfigXml.EthernetSetting();
        //        IPEndPoint iep = (IPEndPoint)m_udpClient.Client.RemoteEndPoint;
        //        xmlConfig.Ethernet.Port = iep.Port;
        //    }
        //    else if (eCommandType.Tcp_Server_Vive == m_eCommandType && null != m_tcpListener)
        //    {
        //        xmlConfig.Ethernet = new ConfigXml.EthernetSetting();
        //        IPEndPoint iep = (IPEndPoint)m_tcpListener.Server.LocalEndPoint;
        //        xmlConfig.Ethernet.Port = iep.Port;
        //    }
        //    else if (eCommandType.Tcp_Server_KT == m_eCommandType && null != m_tcpListener)
        //    {
        //        xmlConfig.Ethernet = new ConfigXml.EthernetSetting();
        //        IPEndPoint iep = (IPEndPoint)m_tcpListener.Server.LocalEndPoint;
        //        xmlConfig.Ethernet.Port = iep.Port;
        //    }
        //    else if (eCommandType.Tcp_Client_KT == m_eCommandType && null != m_tcpClient)
        //    {
        //        xmlConfig.Ethernet = new ConfigXml.EthernetSetting();
        //        IPEndPoint iep = (IPEndPoint)m_tcpClient.Client.RemoteEndPoint;
        //        xmlConfig.Ethernet.IP = iep.Address.ToString();
        //        xmlConfig.Ethernet.Port = iep.Port;
        //    }

        //    xmlConfig.API = (eApiType)Enum.Parse(typeof(eApiType), m_toolStripComboBoxAPI.Text);

        //    xmlConfig.KeyCommand = m_xmlConfig.KeyCommand;
        //    xmlConfig.EventCommand = m_xmlConfig.EventCommand;
        //    xmlConfig.HotKey = m_xmlConfig.HotKey;
        //    xmlConfig.UsedHotKey = m_xmlConfig.UsedHotKey;

        //    if (0 != XmlUtil.Save<ConfigXml>(xmlConfig, strPath))
        //        return false;

        //    return true;
        //}

        public bool LoadConfig(string strPath)
        {
            if (!File.Exists(strPath))
                return false;

            if (0 != XmlUtil.Load<ConfigXml>(out m_xmlConfig, strPath))
                return false;

            SaveConfig(m_sLastFile);

            //  m_xmlConfig.EM = 1;
            //  m_xmlConfig.Conveyor1.BoxCount = "200";

            // SaveConfig(strPath);

            //if (null != m_xmlConfig.File && string.Empty != m_xmlConfig.File && File.Exists(m_xmlConfig.File))
            //{
            //    m_strFileName = m_xmlConfig.File;
            //    Load2();
            //}

            //  Offset(m_xmlConfig.Offset);
            //  Cycle(m_xmlConfig.Cycle);

            //m_eCommandType = (eCommandType)Enum.Parse(typeof(eCommandType), m_xmlConfig.CommandType);

            //if (eCommandType.Serial == m_eCommandType || eCommandType.Contact == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Serial)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server_P2P == m_eCommandType || eCommandType.Tcp_Client == m_eCommandType || eCommandType.Udp == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Manual == m_eCommandType)
            //{
            //    m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server_Vive == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server_KT == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Client_KT == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}







            //if (null != m_xmlConfig.KeyCommand)
            //{
            //    if (null != m_xmlConfig.KeyCommand.Keys && 0 < m_xmlConfig.KeyCommand.Keys.Count)
            //    {
            //        foreach (KeyCommand.CommandDefine data in m_xmlConfig.KeyCommand.Keys)
            //        {
            //            m_utilKeyboardHook.AddKey(data.Key);
            //        }
            //    }
            //}

            //int nIndex = m_toolStripComboBoxCommand.Items.IndexOf(m_xmlConfig.CommandType);
            //m_toolStripComboBoxCommand.SelectedIndex = nIndex;
            return true;
        }


        public bool SaveConfig(string strPath)
        {
            if (!File.Exists(strPath))
                return false;

            if (0 != XmlUtil.Save<ConfigXml>(m_xmlConfig, strPath))
                return false;

            //  int comport = (int)m_optConfig.GetValue("Comport", typeof(int));
            //  int Timeout = (int)m_optConfig.GetValue("Timeout", typeof(int));
            //    string lastFile = (string)m_optConfig.GetValue("LastFile", typeof(string));

            //   m_comPort = (int)m_optConfig.GetValue("Comport", typeof(int));
            //    m_nTimeout = (int)m_optConfig.GetValue("Timeout", typeof(int));
            //    m_sLastFile = (string)m_optConfig.GetValue("LastFile", typeof(string));
            m_comPort = 1;


            AddOrUpdateAppSettings("Comport", m_comPort.ToString());
            AddOrUpdateAppSettings("Timeout", m_nTimeout.ToString());
            AddOrUpdateAppSettings("LastFile", m_sLastFile);

            //System.Configuration.ConfigurationSettings config1 = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            //config1.AppSettings.Settings.Remove("MySetting");
            //config1.AppSettings.Settings.Add("MySetting", "some value");

            //config.Save(ConfigurationSaveMode.Modified);

            // System.Configuration.ConfigurationSettings config = ConfigurationManager.

            //if (null != m_xmlConfig.File && string.Empty != m_xmlConfig.File && File.Exists(m_xmlConfig.File))
            //{
            //    m_strFileName = m_xmlConfig.File;
            //    Load2();
            //}

            //  Offset(m_xmlConfig.Offset);
            //  Cycle(m_xmlConfig.Cycle);

            //m_eCommandType = (eCommandType)Enum.Parse(typeof(eCommandType), m_xmlConfig.CommandType);

            //if (eCommandType.Serial == m_eCommandType || eCommandType.Contact == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Serial)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server_P2P == m_eCommandType || eCommandType.Tcp_Client == m_eCommandType || eCommandType.Udp == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Manual == m_eCommandType)
            //{
            //    m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server_Vive == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Server_KT == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}
            //else if (eCommandType.Tcp_Client_KT == m_eCommandType)
            //{
            //    if (null == m_xmlConfig.Ethernet)
            //        m_isInitialize = true;
            //}







            //if (null != m_xmlConfig.KeyCommand)
            //{
            //    if (null != m_xmlConfig.KeyCommand.Keys && 0 < m_xmlConfig.KeyCommand.Keys.Count)
            //    {
            //        foreach (KeyCommand.CommandDefine data in m_xmlConfig.KeyCommand.Keys)
            //        {
            //            m_utilKeyboardHook.AddKey(data.Key);
            //        }
            //    }
            //}

            //if (null != m_xmlConfig.EventCommand)
            //{
            //    if (null != m_xmlConfig.EventCommand.Start)
            //        m_startSimulatorVariable = m_xmlConfig.EventCommand.Start;
            //    if (null != m_xmlConfig.EventCommand.End)
            //        m_endSimulatorVariable = m_xmlConfig.EventCommand.End;
            //}

            //if (null != m_xmlConfig.HotKey)
            //{
            //    if (null != m_xmlConfig.HotKey.F1)
            //        m_utilKeyboardHook.AddKey(Keys.F1);
            //    if (null != m_xmlConfig.HotKey.F2)
            //        m_utilKeyboardHook.AddKey(Keys.F2);
            //    if (null != m_xmlConfig.HotKey.F3)
            //        m_utilKeyboardHook.AddKey(Keys.F3);
            //    if (null != m_xmlConfig.HotKey.F4)
            //        m_utilKeyboardHook.AddKey(Keys.F4);
            //    if (null != m_xmlConfig.HotKey.F5)
            //        m_utilKeyboardHook.AddKey(Keys.F5);
            //    if (null != m_xmlConfig.HotKey.F6)
            //        m_utilKeyboardHook.AddKey(Keys.F6);
            //    if (null != m_xmlConfig.HotKey.F7)
            //        m_utilKeyboardHook.AddKey(Keys.F7);
            //    if (null != m_xmlConfig.HotKey.F8)
            //        m_utilKeyboardHook.AddKey(Keys.F8);
            //    if (null != m_xmlConfig.HotKey.F9)
            //        m_utilKeyboardHook.AddKey(Keys.F9);
            //    if (null != m_xmlConfig.HotKey.F10)
            //        m_utilKeyboardHook.AddKey(Keys.F10);
            //    if (null != m_xmlConfig.HotKey.F11)
            //        m_utilKeyboardHook.AddKey(Keys.F11);
            //    if (null != m_xmlConfig.HotKey.F12)
            //        m_utilKeyboardHook.AddKey(Keys.F12);
            //}

            //int nIndex = m_toolStripComboBoxCommand.Items.IndexOf(m_xmlConfig.CommandType);
            //m_toolStripComboBoxCommand.SelectedIndex = nIndex;
            return true;
        }


        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void Load2()
        {
            //if (this.InvokeRequired)
            //{
            //    DelegatePlayStopLoadKeepAlive delegatePlayStopLoadKeepAlive = new DelegatePlayStopLoadKeepAlive(Load2);
            //    this.Invoke(delegatePlayStopLoadKeepAlive);
            //}
            //else
            //{
            //    if (m_strFileName.Contains(".xml"))
            //    {
            //        LoadXmlFile(m_strFileName);
            //    }
            //    //else if (m_strFileName.Contains(".csv"))
            //    //{
            //    //    LoadCsvFile(m_strFileName);
            //    //}
            //}
        }

        public int LoadXmlFile(string strPath)
        {
            //RecordFileXml xmlSetting;
            //if (0 != XmlUtil.Load<RecordFileXml>(out xmlSetting, strPath))
            //    return -1;

            //m_listVariableType.Clear();
            //m_listSimulatorVariable.Clear();

            //foreach (eVariableType type in xmlSetting.Type)
            //{
            //    m_listVariableType.Add(type);
            //}

            //m_listSimulatorVariable = xmlSetting.Data.ToList();
            ////  int m_nStartIndex = int.Parse(m_toolStripComboBoxOffset.Text);
            //int m_nStopIndex = m_listSimulatorVariable.Count;
            ////m_labelPlayCount.Text = m_nStopIndex.ToString();
            ////  m_labelPlayStep.Text = string.Format("00000 / {0:D5}", m_nStopIndex);

            //this.Text = string.Format("Motion Player - {0}", Path.GetFileName(strPath));
            return 0;

        }

        public bool InitializeThread()
        {
            try
            {
                Thread thread = new Thread(new ThreadStart(PlayThread2));
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }


        private void PlayThread2()
        {
            //bool isCheck = false;
            //int nResult = 0, nTime = 0;
            ////m_communicationMotionControl = new MotionControl();
            ////int nComPortOpenResult = m_communicationMotionControl.Initial();
            //foreach (KeyValuePair<SimulatorCommand.InfoDefine, MotionControl> montionControl in m_dicCommunicationMotionControl)
            //{
            //    bool isComPortOpenResult = false;
            //    if (string.Empty == montionControl.Key.IP)
            //        isComPortOpenResult = montionControl.Value.Initial(montionControl.Key.Port);
            //    else
            //        isComPortOpenResult = montionControl.Value.Initial(montionControl.Key.IP, montionControl.Key.Port);

            //    Console.WriteLine("Initial Fail - {0}", isComPortOpenResult);
            //    CallState(EventComOpenResult, eState.ComOpen, montionControl.Key.Port, isComPortOpenResult);
            //}
            //double dDurationMilisecond = 0;
            //double dTime = 0;
            //int nWaitCount = 0;

            //while (m_isMainThread)
            //{
            //    if (m_autoMainEvent.WaitOne(10))
            //    {
            //        if (!m_isMainThread)
            //            break;

            //        m_autoMainEvent.Reset();

            //        CallState(EventStatePlayStart, eState.PlayStart);

            //        isCheck = false;
            //        m_nPlayIndex = 0;
            //        nResult = 0;

            //        //  Pause가 아니면 Start Offset 실행
            //        if (!m_isPause)
            //        {
            //            if (0 != m_nStartIndex)
            //            {
            //                CallState(EventStateOffsetStart, eState.OffsetStart);

            //                isCheck = true;
            //                m_hiPerfTimerUtil.Start();
            //                while (isCheck)
            //                {
            //                    Thread.Sleep(10);
            //                    m_hiPerfTimerUtil.Stop();
            //                    if (m_nStartIndex < m_hiPerfTimerUtil.DurationMilisecond)
            //                        isCheck = false;
            //                    else
            //                    {
            //                        CallState(EventStateOffsetIng, eState.OffsetIng, (int)m_hiPerfTimerUtil.DurationMilisecond);
            //                        nWaitCount++;
            //                        if (500 == nWaitCount)
            //                        {
            //                            nWaitCount = 0;
            //                            if (null != m_xmlConfig.EventCommand && null != m_xmlConfig.EventCommand.Wait)
            //                                Play(m_xmlConfig.EventCommand.Wait, false);
            //                        }
            //                    }
            //                }

            //                CallState(EventStateOffsetComplete, eState.OffsetComplete);
            //            }
            //        }

            //        m_hiPerfTimerUtil.Start();

            //        while (m_isThread)
            //        {
            //            nResult = 0;
            //            isCheck = true;

            //            if (m_autoEvent.WaitOne(10))
            //                break;

            //            while (isCheck)
            //            {
            //                if (!m_isThread)
            //                    break;

            //                if (m_listSimulatorVariable.Count == m_nPlayIndex)
            //                {
            //                    nResult = -1;
            //                    break;
            //                }
            //                Thread.Sleep(5);
            //                m_hiPerfTimerUtil.Stop();
            //                dDurationMilisecond = m_hiPerfTimerUtil.DurationMilisecond + m_dPauseTime;

            //                if (0 < m_dEndTime && m_dEndTime < dDurationMilisecond)
            //                {
            //                    nResult = -1;
            //                    goto RepeatIntervalEnd;
            //                }

            //                dTime = m_listSimulatorVariable[m_nPlayIndex].Time * m_dExpansion;
            //                if (dTime < dDurationMilisecond)
            //                {
            //                    while (true)
            //                    {
            //                        m_nPlayIndex++;
            //                        if (m_listSimulatorVariable.Count == m_nPlayIndex)
            //                        {
            //                            nResult = -1;
            //                            break;
            //                        }

            //                        dTime = m_listSimulatorVariable[m_nPlayIndex].Time * m_dExpansion;
            //                        if (dTime >= dDurationMilisecond)
            //                            break;
            //                    }
            //                    if (-1 == nResult)
            //                        break;
            //                }

            //                if (0 == dTime)
            //                {
            //                    isCheck = false;
            //                }
            //                else if (((dTime - 20) < dDurationMilisecond) && (dDurationMilisecond <= dTime))
            //                {
            //                    isCheck = false;
            //                }

            //                if (dTime != dDurationMilisecond / m_nSampling)
            //                {
            //                    if (!m_isThread)
            //                        break;
            //                    //nTime = (int)dDurationMilisecond / m_nSampling;
            //                    //nTime = (int)dDurationMilisecond;
            //                    //PlayTime((int)dDurationMilisecond);

            //                    CallState(EventPlayTime, eState.PlayTime, (int)(dDurationMilisecond / m_dExpansion));
            //                }
            //            }
            //            if (m_listSimulatorVariable.Count == m_nPlayIndex)
            //            {
            //                nResult = -1;
            //                break;
            //            }

            //            if (m_isThread)
            //            {
            //                nResult = Play(m_nPlayIndex);

            //                CallState(EventPlaying, eState.Playing, m_nPlayIndex, m_nStopIndex);
            //            }

            //            m_nPlayIndex++;
            //            if (-1 == nResult || -2 == nResult)
            //            {
            //                break;
            //            }
            //            else
            //            {
            //                if (m_nStopIndex == nResult)
            //                {
            //                    nResult = -1;
            //                    break;
            //                }
            //            }
            //        }

            //        RepeatIntervalEnd:

            //        if (-1 == nResult)
            //        {
            //            m_autoEvent.Set();

            //            CallState(EventStatePlayEnd, eState.PlayEnd);

            //            if (m_isMainThread && null != m_xmlConfig.EventCommand)
            //            {
            //                if (null != m_xmlConfig.EventCommand.End)
            //                    Play(m_xmlConfig.EventCommand.End);
            //            }
            //        }
            //        else
            //        {
            //            if (m_isStop)
            //            {
            //                m_isStop = false;
            //                CallState(EventStatePlayEnd, eState.PlayEnd);

            //                if (m_isMainThread && null != m_xmlConfig.EventCommand)
            //                {
            //                    if (null != m_xmlConfig.EventCommand.End)
            //                        Play(m_xmlConfig.EventCommand.End);
            //                }
            //            }
            //        }
            //        m_isThread = false;
            //        m_autoCloseEvent.Set();
            //    }
            //    else
            //    {
            //        nWaitCount++;
            //        if (500 == nWaitCount)
            //        {
            //            nWaitCount = 0;
            //            if (null != m_xmlConfig.EventCommand && null != m_xmlConfig.EventCommand.Ping)
            //                Play(m_xmlConfig.EventCommand.Ping, false);
            //        }
            //    }
            //}
            ////m_communicationMotionControl.Destroy();
            //foreach (KeyValuePair<SimulatorCommand.InfoDefine, MotionControl> montionControl in m_dicCommunicationMotionControl)
            //{
            //    montionControl.Value.Destroy();
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  MotionControl.Communication.Command motion = new Command();
            //bool b = o1.Initial(0);

            //if (b)
            //{
            //    o1.EQConv1RunRpmRead();
            //}




        }

        private void button2_Click(object sender, EventArgs e)
        {
           // bool b = o1.Initial(0);

           //// if (b)
           // {
           //     int i = Convert.ToInt32(textBox1.Text);
           //     o1.EQConv1RunRpmWrite(i);
           // }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            bool b = m_ComObject.Initial(0);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            bool b = m_ComObject.Destroy();
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
           // SaveConfig(m_sLastFile);
        }
    }
    }
