using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace MotionControl.Communication
{
    public partial class Command
    {
        #region Initial
        [DllImport("MotionControlDll.dll")]
        private extern static bool ComInitial(int nPort, int nTimeout);
        #endregion

        #region Destroy
        [DllImport("MotionControlDll.dll")]
        private extern static bool ComDestroy();
        #endregion


        #region Eq Mode Read
        [DllImport("MotionControlDll.dll")]
        private extern static int ComEQModeRead();
        #endregion

        #region Conv1RunRpm read
        [DllImport("MotionControlDll.dll")]
        private extern static int ComEQConv1RunRpmRead();
        #endregion

        #region Conv1RunRpm write
        [DllImport("MotionControlDll.dll")]
        private extern static int ComEQConv1RunRpmWrite(int nData);
        #endregion
    }
}
