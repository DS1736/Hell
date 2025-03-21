using Ezcad;
using Fluent;
using InIWorkspace.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InIWorkspace.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {

        private EzdKernel.E3_ID m_idEM = EzdKernel.E3_ID.INVALID;
        private EzdKernel.E3_ID m_idCurLayer = EzdKernel.E3_ID.INVALID;
        private EzdKernel.E3_ID m_idMarker = EzdKernel.E3_ID.INVALID;
        private EzdKernel.E3_ID[] m_idMarkerList;
        private EzdKernel.E3_ID[] m_idLayerList;
        private List<EzdKernel.E3_ID> m_idForListMarkEntList;
        private int[] m_CardSn;
        private int m_nCurrentLayerIndex = 0;
        private int m_nCurrentMarkerIndex = 0;
        private bool m_bIsInitailOK = false;

        private bool _isBitMap = false;
        


        public class USB_Message
        {
            public const int USER = 0x0400;
            public const int WM_LOST = USER + 17;
            public const int WM_GETNEW = USER + 18;
        }




        private SKPoint? _startPoint = null; // Stores the starting point of the rectangle
        private SKRect _rectangle = SKRect.Empty; // Stores the current rectangle
        private bool _isDrawing = false; // Indicates if the user is currently drawing


        private float _scale = 1.0f; // Current zoom scale
        private const float ZoomFactor = 1.1f; // Zoom step factor

        public MainWindowView()
        {
            InitializeComponent();
            // WorkspaceMenuControl.FileSelected += OnFileSelected;
            // var ezCadService = new EzCadService_Windows();
            //string appPath = @"C:\Users\91997\workspace\IZICode - Touch - Documents\EzCAD SDK\Ezcad3_demo\Ezcad3_demo\bin\Debug"; // System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            //EzdKernel.E3_ID NewDevID = EzdKernel.E3_ID.INVALID;
            //// var usbErr = E3_MarkerUsbMonitorGetNewDevice(ref NewDevID);
            //// _logger.LogInformation("MarkerUsbMonitorGetNewDevice: {0}", usbErr);
            //string ezcad_exe_path = $"{appPath}\\";

            //var initErr = EzdKernel.E3_Initial($"{ezcad_exe_path}", 0);
            //if (initErr != EzdKernel.E3_ERR.SUCCESS)
            //{
            //    // Show error message
            //    MessageBox.Show("E3_Initial=" + initErr.ToString());
            //    return;
            //}

            //m_idMarker = EzdKernel.E3_MarkerGetFirstValidId();
            //m_idEM = EzdKernel.E3_CreateEntMgr(0);
        }                   
    }
}