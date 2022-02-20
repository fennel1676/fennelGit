using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MotionControl.Communication
{


    public enum eEqEventType
    {
        EqStatus,
        EqConv1RpmStatus,
        EqConv1BoxCountStatus,

    }


    public class EqEventArgs : EventArgs
    {
        public eEqEventType EventType;
        public object EventData;

        public EqEventArgs(eEqEventType eventType, object dataType)
        {
            this.EventType = eventType;
            this.EventData = dataType;
        }
    }



    public partial class Command
    {

        //public delegate void EqReceivedEvent(object sender, EqEventArgs e);
        public delegate void EqReceivedEventd(EqEventArgs e);
        public event EqReceivedEventd OnEqReceived = null;


        public int Port { get; set; }
        public int Timeout { get; set; }
        //private bool m_checkInitial = false;

        public bool Initial(int nPort = -1, int nTimeout = 300)
        {
            Port = nPort;
            Timeout = nTimeout;

            bool isResult = true;
            isResult = ComInitial(Port, nTimeout);
           // OnEqReceived(new EqEventArgs(eEqEventType.EqStatus, isResult ? 1 : 0));

            OnEqReceived(new EqEventArgs(eEqEventType.EqStatus, isResult));

            return isResult;
        }


        public bool Destroy()
        {
            bool isResult = true;
            isResult = ComDestroy();
            OnEqReceived(new EqEventArgs(eEqEventType.EqStatus, isResult));
            return isResult;
        }




        public int EQModeRead()
        {
            int nResult = 0;
            nResult = ComEQModeRead();
            return nResult;
        }


        public int EQConv1RunRpmRead()
        {
            int nResult = 0;
            nResult = ComEQConv1RunRpmRead();
            OnEqReceived(new EqEventArgs(eEqEventType.EqConv1RpmStatus, nResult));
            return nResult;
        }


        public int EQConv1RunRpmWrite(int nData)
        {
            int nResult = 0;
            nResult = ComEQConv1RunRpmWrite(nData);
            OnEqReceived(new EqEventArgs(eEqEventType.EqStatus, Convert.ToBoolean(nResult)));
            return nResult;
        }
    }


    }
