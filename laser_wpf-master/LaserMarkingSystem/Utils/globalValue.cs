using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InIWorkspace.Utils
{
    class globalValue
    {
        public static string SerialPortName = "COM1";
        public static string SerialPortBaudrate = "115200";

        public static string TCPIPAddress = "127.0.0.1";
        public static string TCPIPPort = "1000";
        public static int Reconnect = 1000;

        public static string ReadyPortNumber = "7";
        public static string IOMaring = "9";
        public static bool bReadyPortNumber = false;
        public static bool bIOMarking = false;
        public static string Doorport = "8";
        public static bool bDoorport = false;

        public static bool serialReset = false;
        public static bool UDPReset = false;

        //FlyMark
        public static bool bFlyMark = false;

        //public static string IOEndSignal = "5";
        //public static bool bIOEndSignal = false;

        public static string RedLight = "5";
        public static bool bRedLight = false;
        public static string Marking = "0";
        public static string MarkDone = "1";
        public static string RedyOutport = "4";
        public static bool bRedyOutport = false;
        public static bool bMarkDone = false;
        public static bool bMarking = false;

        public static char Separator = ',';
        //public static string m_strLastFile = string.Empty;

        //마킹카운트
        public static int TotalCount = 0;
        public static int CurrentCount = 0;

        //최근메시지 로드
        public static bool bLoadMessage = false;
        public static string FilePath = "";

        //서버모드
        public static bool bServerMode = false;
        
        //시작시자동접속
        public static bool bAutoConnect = false;

        //언어
        public static string Language = "English";


        public globalValue()
        {

        }
    }
}
