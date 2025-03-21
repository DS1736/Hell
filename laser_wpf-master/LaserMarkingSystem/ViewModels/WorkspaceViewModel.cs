using Caliburn.Micro;
using Infini.Draw2D.Core.Shapes.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infini.Draw2D.Core.Constants;
using Infini.Draw2D.Core;
using System.Windows.Media;
using Infini.Draw2D.Core.Shapes.FigureExtensions;
using System.Windows.Input;
using InIWorkspace.Commands;
using Infini.Draw2D.Core.Policies.RouterPolicy;
using Ezcad;
using System.Windows.Forms;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using InIWorkspace.Utils;
using Infini.Draw2D.Core.Handles;
using System.Windows.Documents;


namespace InIWorkspace.ViewModels
{
    public class WorkspaceViewModel : BaseViewModel
    {
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        // EZ Cad
        private EzdKernel.E3_ID m_idEM = EzdKernel.E3_ID.INVALID;
        private EzdKernel.E3_ID m_idCurLayer = EzdKernel.E3_ID.INVALID;
        private EzdKernel.E3_ID m_idMarker = EzdKernel.E3_ID.INVALID;
        private EzdKernel.E3_ID[] m_idMarkerList;
        private EzdKernel.E3_ID[] m_idLayerList;
        private List<EzdKernel.E3_ID> m_idForListMarkEntList;

        EzdKernel.E3_ID m_idEntbyIndex = EzdKernel.E3_ID.INVALID;
        private EzdKernel.E3_ID m_idEntbyName = EzdKernel.E3_ID.INVALID;
        EzdKernel.E3_ID[] m_IdEntList;
        int m_nEntCount = 0;

        private int[] m_CardSn;
        private int m_nCurrentLayerIndex = 0;
        private int m_nCurrentMarkerIndex = 0;
        private bool m_bIsInitailOK = false;

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker bw_IO = null;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        private ICommand _createLineCommand;
        private ICommand _createPolylineCommand;
        private ICommand _createRectangleCommand;
        private ICommand _createCircleCommand;
        private ICommand _createEllipseCommand;

        private ICommand _createBarcodeCommand;
        private ICommand _createTextCommand;
        private ICommand _createImageCommand;

        // Lazer and Marking
        private ICommand _initLazerCommand;
        private ICommand _stopLazerCommand;
        private ICommand _startMarkingCommand;
        private ICommand _startRedCommand;
        private ICommand _stopMarkingCommand;
        private ICommand _stopRedCommand;

        // Lazer Status
        private String _lazerStatus;
        public String LazerStatus { get; set; }

        //bw_IO_DoWork IO check        
        int LaserCHK = 0;
        int LockCHK = 0;
        int COMCHK = 0;
        int OldData = 0;
        int OldOutData = 0;

        // File COmmands
        private ICommand _openEzcadFileCommand;
        private ICommand _saveEzcadFileCommand;
        private ICommand _openHardwareSettingsCommand;
        private ICommand _openPenSettingsCommand;

        public ICommand CreateLineCommand
        {
            get { return _createLineCommand; }
            set
            {
                if (Equals(value, _createLineCommand)) return;
                _createLineCommand = value;
                NotifyOfPropertyChange(() => CreateLineCommand);
            }
        }


        public ICommand CreatePolylineCommand
        {
            get { return _createPolylineCommand; }
            set
            {
                if (Equals(value, _createPolylineCommand)) return;
                _createPolylineCommand = value;
                NotifyOfPropertyChange(() => CreatePolylineCommand);
            }
        }


        public ICommand CreateRectangleCommand
        {
            get { return _createRectangleCommand; }
            set
            {
                if (Equals(value, _createRectangleCommand)) return;
                _createRectangleCommand = value;
                NotifyOfPropertyChange(() => CreateRectangleCommand);
            }
        }

        public ICommand CreateCircleCommand
        {
            get { return _createCircleCommand; }
            set
            {
                if (Equals(value, _createCircleCommand)) return;
                _createCircleCommand = value;
                NotifyOfPropertyChange(() => CreateCircleCommand);
            }
        }

        public ICommand CreateEllipseCommand
        {
            get { return _createEllipseCommand; }
            set
            {
                if (Equals(value, _createEllipseCommand)) return;
                _createEllipseCommand = value;
                NotifyOfPropertyChange(() => CreateEllipseCommand);
            }
        }

        public ICommand CreateBarcodeCommand
        {
            get { return _createBarcodeCommand; }
            set
            {
                if (Equals(value, _createBarcodeCommand)) return;
                _createBarcodeCommand = value;
                NotifyOfPropertyChange(() => CreateBarcodeCommand);
            }
        }

        public ICommand CreateTextCommand
        {
            get { return _createTextCommand; }
            set
            {
                if (Equals(value, _createTextCommand)) return;
                _createTextCommand = value;
                NotifyOfPropertyChange(() => CreateTextCommand);
            }
        }

        public ICommand CreateImageCommand
        {
            get { return _createImageCommand; }
            set
            {
                if (Equals(value, _createImageCommand)) return;
                _createImageCommand = value;
                NotifyOfPropertyChange(() => CreateImageCommand);
            }
        }

        public ICommand InitLazerCommand
        {
            get { return _initLazerCommand; }
            set
            {
                if (Equals(value, _initLazerCommand)) return;
                _initLazerCommand = value;
                NotifyOfPropertyChange(() => InitLazerCommand);
            }
        }

        public ICommand StopLazerCommand
        {
            get { return _stopLazerCommand; }
            set
            {
                if (Equals(value, _stopLazerCommand)) return;
                _stopLazerCommand = value;
                NotifyOfPropertyChange(() => StopLazerCommand);
            }
        }


        public ICommand StartMarkingCommand
        {
            get { return _startMarkingCommand; }
            set
            {
                if (Equals(value, _startMarkingCommand)) return;
                _startMarkingCommand = value;
                NotifyOfPropertyChange(() => StartMarkingCommand);
            }
        }

        public ICommand StopMarkingCommand
        {
            get { return _stopMarkingCommand; }
            set
            {
                if (Equals(value, _stopMarkingCommand)) return;
                _stopMarkingCommand = value;
                NotifyOfPropertyChange(() => StopLazerCommand);
            }
        }

        public ICommand StartRedCommand
        {
            get { return _startRedCommand; }
            set
            {
                if (Equals(value, _startRedCommand)) return;
                _startRedCommand = value;
                NotifyOfPropertyChange(() => StartRedCommand);
            }
        }


        public ICommand StopRedCommand
        {
            get { return _stopRedCommand; }
            set
            {
                if (Equals(value, _stopRedCommand)) return;
                _stopRedCommand = value;
                NotifyOfPropertyChange(() => StopRedCommand);
            }
        }


        public ICommand OpenEzcadFileCommand
        {
            get { return _openEzcadFileCommand; }
            set
            {
                if (Equals(value, _openEzcadFileCommand)) return;
                _openEzcadFileCommand = value;
                NotifyOfPropertyChange(() => OpenEzcadFileCommand);
            }
        }

        public ICommand SaveEzcadFileCommand
        {
            get { return _saveEzcadFileCommand; }
            set
            {
                if (Equals(value, _saveEzcadFileCommand)) return;
                _saveEzcadFileCommand = value;
                NotifyOfPropertyChange(() => SaveEzcadFileCommand);
            }
        }

        public ICommand OpenHardwareSettingsCommand
        {

            get { return _openHardwareSettingsCommand; }
            set
            {
                if (Equals(value, _openHardwareSettingsCommand)) return;
                _openHardwareSettingsCommand = value;
                NotifyOfPropertyChange(() => OpenHardwareSettingsCommand);

            }

        }
        public ICommand OpenPenSettingsCommand
        {

            get { return _openPenSettingsCommand; }
            set
            {
                if (Equals(value, _openPenSettingsCommand)) return;
                _openPenSettingsCommand = value;
                NotifyOfPropertyChange(() => OpenPenSettingsCommand);

            }

        }

        public WorkspaceViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            // Subscribe to events on the UI thread
            _eventAggregator.SubscribeOnUIThread(this); 
            DisplayName = "Workspace";

            InitLazerCommandExecute();

            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            bw_IO = new BackgroundWorker();

            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);


            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);

            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker4_DoWork);
            this.bw_IO.DoWork += new DoWorkEventHandler(bw_IO_DoWork);
            this.bw_IO.RunWorkerAsync();
            CreateCommands();
        }

        // Create Commands
        private void CreateCommands()
        {
            LineCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new LineTool(), (tool) => LineCommand.RaiseCanExecuteChanged());
            }, () => Canvas.ActiveTool == null);


            PolylineCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new PolylineTool(), (tool) => CommandManager.InvalidateRequerySuggested() /*PolylineCommand.RaiseCanExecuteChanged()*/);
            }, () => Canvas.ActiveTool == null);

            CreateRectangleCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new RectangleTool(), (tool) => CommandManager.InvalidateRequerySuggested() /*RectangleCommand.RaiseCanExecuteChanged()*/);
            }, () => Canvas.ActiveTool == null);

            CreateCircleCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new CircleTool(), (tool) => CommandManager.InvalidateRequerySuggested() /*CircleCommand.RaiseCanExecuteChanged()*/);
            }, () => Canvas.ActiveTool == null);

            CreateEllipseCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new EllipseTool(), (tool) => CommandManager.InvalidateRequerySuggested() /*EllipseCommand.RaiseCanExecuteChanged()*/);
            }, () => Canvas.ActiveTool == null);


            CreateBarcodeCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new BarcodeTool(), (tool) => CommandManager.InvalidateRequerySuggested() /*BarcodeCommand.RaiseCanExecuteChanged()*/);
            }, () => Canvas.ActiveTool == null);

            
            CreateTextCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new TextTool(), (tool) => CommandManager.InvalidateRequerySuggested() /*TextCommand.RaiseCanExecuteChanged()*/);
            }, () => Canvas.ActiveTool == null);

            CreateImageCommand = new RelayCommand(() =>
            {
                Canvas.InstallTool(new ImageTool(), (tool) => CommandManager.InvalidateRequerySuggested() /*ImageCommand.RaiseCanExecuteChanged()*/);
            }, () => Canvas.ActiveTool == null);

            CreateLineCommand = LineCommand;
            CreatePolylineCommand = PolylineCommand;


            InitLazerCommand = new RelayCommand(() =>
            {
                // Init Lazer
                InitLazerCommandExecute();
            }, () => true);

            StopLazerCommand = new RelayCommand(() =>
            {
                // Stop Lazer
                StopLazerCommandExecute();

            }, () => true);

            StartMarkingCommand = new RelayCommand(() =>
            {
                // Start Marking
                StartMarkingCommandExecute();
            }, () => true);

            StopMarkingCommand = new RelayCommand(() =>
            {
                // Stop Marking
                StopMarkingCommandExecute();
            });

            StartRedCommand = new RelayCommand(() =>
            {
                // Start Red
                StartRedCommandExecute();
            }, () => true);

            StopRedCommand = new RelayCommand(() =>
            {
                // Stop Red
                StopRedCommandExecute();
            }, () => true);

            OpenEzcadFileCommand = new RelayCommand(() =>
            {
                // Open File and Read Ezcad file
                OpenEzcadFileCommandExecute();
            });

            SaveEzcadFileCommand = new RelayCommand(() =>
            {
                // Open File and Read Ezcad file
            });
            OpenHardwareSettingsCommand = new RelayCommand(() =>
            {
                StartOpenHardwareSettingCommandExecute();
            }, () => true);
            OpenPenSettingsCommand = new RelayCommand(() =>
            {
                StartOpenPenSettingCommandExecute();
            }, () => true);
        }

        // over ride the OnInitializeAsync method to add the graph to the canvas
        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);
            Canvas.CoordinateSystem = new CartesianCoordinateSystem(Canvas.Width / 2, Canvas.Height / 2);

            //var rect1 = new Rectangle(0, 0, 100, 100)
            //{
            //    FillColor = Colors.CornflowerBlue,
            //    StrokeColor = Colors.Coral,
            //    StrokeThickness = 8
            //};
            //rect1.AddHandlesCornerDirections(Canvas, HandleSizes.Small, HandleShapeType.Round);
            //rect1.InstallEditPolicy(SelectionFeedbackPolicy);

            //Canvas.AddFigure(rect1);

            //var rect2 = new Rectangle(250, 100, 100, 100)
            //{
            //    FillColor = Color.FromArgb(100, 100, 170, 80)
            //};

            //rect2.AddHandlesCornerDirections(Canvas, HandleSizes.Small, HandleShapeType.Round);
            //rect2.InstallEditPolicy(SelectionFeedbackPolicy);
            //Canvas.AddFigure(rect2);

            //var ellipse2 = new Ellipse(100, 300, 100, 100)
            //{
            //    FillColor = Colors.Transparent
            //};
            //ellipse2.AddHandlesCornerDirections(Canvas, HandleSizes.Small, HandleShapeType.Round);
            //ellipse2.InstallEditPolicy(SelectionFeedbackPolicy);
            //Canvas.AddFigure(ellipse2);

            //var ellipse3 = new Ellipse(250, 300, 100, 100);
            //ellipse3.FillColor = Colors.Brown;
            //ellipse3.AddHandlesCornerDirections(Canvas, HandleSizes.Small, HandleShapeType.Round);
            //ellipse3.InstallEditPolicy(SelectionFeedbackPolicy);
            //Canvas.AddFigure(ellipse3);
        }

        // Init Lazer
        private void InitLazerCommandExecute()
        {
            // Init Lazer
            EzdKernel.E3_DisableInitialPrompt();

            EzdKernel.E3_ERR err = EzdKernel.E3_Initial(Application.StartupPath, 0);

            if (err == EzdKernel.E3_ERR.EZCADRUN)
            {
                MessageBox.Show("Software is open", "EzcadDemo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("Software licence is invalid!" + "ERR:" + err, "EzcadDemo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                m_bIsInitailOK = true;

                m_idEM = EzdKernel.E3_CreateEntMgr(0);
                EzdKernel.E3_GetLayerId(m_idEM, 0, ref m_idCurLayer);


                bool GetMarkerOK = GetAllMark(ref m_idMarkerList, ref m_CardSn);
                if (GetMarkerOK)
                {
                    m_nCurrentMarkerIndex = 0;
                    m_idMarker = m_idMarkerList[m_nCurrentMarkerIndex];
                }
                MessageBox.Show("Ready", "EzcadDemo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool GetAllMark(ref EzdKernel.E3_ID[] MarkerIDList, ref int[] MarkerSnList)
        {
            EzdKernel.E3_ID[] ResultAll = new EzdKernel.E3_ID[8];
            int nMarkCount = 0;
            for (int i = 0; i < 8; i++)
            {
                EzdKernel.E3_ID OneMark = EzdKernel.E3_MarkerGetFirstValidId();
                if (OneMark != EzdKernel.E3_ID.INVALID)
                {
                    ResultAll[i] = OneMark;
                    nMarkCount++;
                }
                else
                {
                    break;
                }
            }
            if (nMarkCount < 1)
            {
                return false;
            }
            MarkerIDList = new EzdKernel.E3_ID[nMarkCount];

            for (int i = 0; i < nMarkCount; i++)
            {
                MarkerIDList[i] = ResultAll[i];
            }
            MarkerSnList = new int[nMarkCount];
            for (int i = 0; i < nMarkCount; i++)
            {
                int Sn = -1;
                EzdKernel.E3_GetMarkerSN(MarkerIDList[i], ref Sn);
                MarkerSnList[i] = Sn;
            }
            return true; ;
        }

        // Stop Lazer
        private void StopLazerCommandExecute()
        {
            // Stop Lazer
            EzdKernel.E3_FreeEntMgr(m_idEM);//release object manager 
            EzdKernel.E3_CloseMarker(m_idMarker);//Closed card 
            EzdKernel.E3_Close(); //Close the system

        }

        // Start Marking
        private void StartMarkingCommandExecute()
        {
            // Start Marking
            // Get all supported figures
            var figures = Canvas.Figures.Where(p => IsSupportedFigureType(p) == true);

            foreach (var item in figures)
            {
                if (item.GetType() == typeof(Rectangle))
                {
                    var errRect = EzdKernel.E3_CreateRect(m_idEM, 0, new Pt2d { X = item.X, Y = item.Y }, new Pt2d { X = item.X + item.Width, Y = item.Y + item.Height }, false, false, "");
                    if (errRect != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("CreateRect=" + errRect.ToString());
                        return;
                    }
                }
                if (item.GetType() == typeof(Ellipse))
                {
                    var errEllipse = EzdKernel.E3_CreateEllipse(m_idEM, 0, new Pt2d { X = item.X, Y = item.Y }, new Pt2d { X = item.X + item.Width, Y = item.Y + item.Height }, false, false, "");
                    if (errEllipse != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("E3_CreateEllipse=" + errEllipse.ToString());
                        return;
                    }
                }
                if (item.GetType() == typeof(Line))
                {
                    Pt2d[] ptBase = new Pt2d[2];
                    ptBase[0].X = item.X;
                    ptBase[0].Y = 0;
                    ptBase[1].X = item.Y;
                    ptBase[1].Y = 0;
                    var errLine = EzdKernel.E3_CreateLines(m_idEM, 0, ptBase, 2, false, false, "");

                    if (errLine != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("E3_CreateLines=" + errLine.ToString());
                        return;
                    }
                }

                // Create Barcode
                if (item.GetType() == typeof(Barcode))
                {

                }
                // Create Text
                if (item.GetType() == typeof(BoxedText))
                {
                    int nPen = 2; //Pen number
                                  //int FontId = cboxFont.SelectedIndex; //Order number in font type
                    int FontId = 0; //Order number in font type
                    double dTextHeight = 12; //Font height
                    double dTextWidthRatio = 1; //Character width ratio
                    int TextSpaceType = 1; //Font separation type under object list column
                    double dTextSpace = 2.5; //Text spacing
                    double dTextAngle = 0; //Font radian
                    int nAlignType = 0; //Font alignment type in font properties
                    double dNullCharWidthRatio = 0.34; //Null character width ratio
                    double dLineSpace = 0.21; //Line spacing
                    bool bVerText = false; //Font arrangement direction, true means vertical arrangement
                    bool bBold = false; //Bold
                    bool bItalic = false; //Italic
                    bool bPick = false; //Object is automatically selected, false means it cannot be selected
                    bool bUndo = false;
                    string strUndo = "";
                    Pt2d ptBase = new Pt2d(); //XY lower left position in the workspace            ptBase.X = 0;
                    ptBase.Y = 0;

                    var text = (BoxedText)item;

                    EzdKernel.E3_CreateText(m_idEM,
                                        nPen,
                                        ptBase,
                                        text.Message,
                                       FontId,
                                        dTextHeight,
                                        dTextWidthRatio,
                                        TextSpaceType,
                                       dTextSpace,
                                        dTextAngle,
                                      nAlignType,
                                        dNullCharWidthRatio,
                                        dLineSpace,
                                        bVerText,
                                        bBold,
                                        bItalic,
                                        bPick,
                                        bUndo,
                                        strUndo);

                }
                // Create Image
                if (item.GetType() == typeof(Image))
                {
                    var image = (Image)item;
                    // (E3_ID idEM, int nPen, string strBmpFile, int nBmpAttrib, int nScanAttrib, Pt2d ptBase, bool bPick, bool bUndo, string strUndoName)
                    //EzdKernel.E3_CreateBitmap(m_idEM, 0, dlg.FileName, 0, 0, ptBase, false, false, "");
                    Pt2d ptBase = new Pt2d();
                    ptBase.X = 0;
                    ptBase.Y = 0;
                    if (string.IsNullOrEmpty(image.ImagePath))
                        EzdKernel.E3_CreateBitmap(m_idEM, 0, image.OriginalFileName, 0, 0, ptBase, false, false, "");
                    else
                        EzdKernel.E3_CreateBitmap(m_idEM, 0, image.ImagePath, 0, 0, ptBase, false, false, "");
                }
            }


            backgroundWorker1.RunWorkerAsync();
            // backgroundWorker1_DoWork(this, null);

        }

        //
        // Stop Marking
        private void StopMarkingCommandExecute()
        {
            // Stop Marking
            if (backgroundWorker1.IsBusy)
            {
                // Stop Red
                if (m_idMarker != EzdKernel.E3_ID.INVALID)
                {
                    EzdKernel.E3_ERR err = EzdKernel.E3_MarkerStop(m_idMarker);
                    if (err != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("CloseMarker=" + err.ToString());
                    }
                }
            }
        }


        // Start Red
        private void StartRedCommandExecute()
        {

            // Start Marking
            // Add Canvas items to entity manager
            var figures = Canvas.Figures.Where(p => IsSupportedFigureType(p) == true);

            foreach (var item in figures)
            {
                if (item.GetType() == typeof(Rectangle))
                {
                    var errRect = EzdKernel.E3_CreateRect(m_idEM, 0, new Pt2d { X = item.X, Y = item.Y }, new Pt2d { X = item.X + item.Width, Y = item.Y + item.Height }, false, false, "");
                    if (errRect != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("CreateRect=" + errRect.ToString());
                        return;
                    }
                }
                if (item.GetType() == typeof(Ellipse))
                {
                    var errEllipse = EzdKernel.E3_CreateEllipse(m_idEM, 0, new Pt2d { X = item.X, Y = item.Y }, new Pt2d { X = item.X + item.Width, Y = item.Y + item.Height }, false, false, "");
                    if (errEllipse != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("E3_CreateEllipse=" + errEllipse.ToString());
                        return;
                    }
                }
                if (item.GetType() == typeof(Line))
                {
                    var line = (Line)item;
                    Pt2d[] ptBase = new Pt2d[2];
                    ptBase[0].X = line.Points[0].X;
                    ptBase[0].Y = line.Points[0].Y;
                    ptBase[1].X = line.Points[1].X;
                    ptBase[1].Y = line.Points[1].Y;
                    var errLine = EzdKernel.E3_CreateLines(m_idEM, 0, ptBase, 2, false, false, "");

                    if (errLine != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("E3_CreateLines=" + errLine.ToString());
                        return;
                    }
                }
                // Create Barcode
                if (item.GetType() == typeof(Barcode))
                {

                }
                // Create Text
                if (item.GetType() == typeof(BoxedText))
                {
                    int nPen = 2; //Pen number
                                  //int FontId = cboxFont.SelectedIndex; //Order number in font type
                    int FontId = 0; //Order number in font type
                    double dTextHeight = 12; //Font height
                    double dTextWidthRatio = 1; //Character width ratio
                    int TextSpaceType = 1; //Font separation type under object list column
                    double dTextSpace = 2.5; //Text spacing
                    double dTextAngle = 0; //Font radian
                    int nAlignType = 0; //Font alignment type in font properties
                    double dNullCharWidthRatio = 0.34; //Null character width ratio
                    double dLineSpace = 0.21; //Line spacing
                    bool bVerText = false; //Font arrangement direction, true means vertical arrangement
                    bool bBold = false; //Bold
                    bool bItalic = false; //Italic
                    bool bPick = false; //Object is automatically selected, false means it cannot be selected
                    bool bUndo = false;
                    string strUndo = "";
                    Pt2d ptBase = new Pt2d(); //XY lower left position in the workspace            ptBase.X = 0;
                    ptBase.Y = 0;

                    var text = (BoxedText) item;

                    EzdKernel.E3_CreateText(m_idEM,
                                        nPen,
                                        ptBase,
                                        text.Message,
                                       FontId,
                                        dTextHeight,
                                        dTextWidthRatio,
                                        TextSpaceType,
                                       dTextSpace,
                                        dTextAngle,
                                      nAlignType,
                                        dNullCharWidthRatio,
                                        dLineSpace,
                                        bVerText,
                                        bBold,
                                        bItalic,
                                        bPick,
                                        bUndo,
                                        strUndo);

                }
                // Create Image
                if (item.GetType() == typeof(Image))
                {
                    var image = (Image)item;
                    // (E3_ID idEM, int nPen, string strBmpFile, int nBmpAttrib, int nScanAttrib, Pt2d ptBase, bool bPick, bool bUndo, string strUndoName)
                    //EzdKernel.E3_CreateBitmap(m_idEM, 0, dlg.FileName, 0, 0, ptBase, false, false, "");
                    Pt2d ptBase = new Pt2d();
                    ptBase.X = 0;
                    ptBase.Y = 0;
                    if (string.IsNullOrEmpty(image.ImagePath))
                        EzdKernel.E3_CreateBitmap(m_idEM, 0, image.OriginalFileName, 0, 0, ptBase, false, false, "");
                    else
                        EzdKernel.E3_CreateBitmap(m_idEM, 0, image.ImagePath, 0, 0, ptBase, false, false, "");
                }
            }

            backgroundWorker2.RunWorkerAsync();
            // backgroundWorker2_DoWork(this, null);
        }


        // Stop Red
        private void StopRedCommandExecute()
        {
            if (backgroundWorker2.IsBusy)
            {
                // Stop Red
                if (m_idMarker != EzdKernel.E3_ID.INVALID)
                {
                    EzdKernel.E3_ERR err = EzdKernel.E3_MarkerStop(m_idMarker);
                    if (err != EzdKernel.E3_ERR.SUCCESS)
                    {
                        MessageBox.Show("CloseMarker=" + err.ToString());
                    }
                }
            }
        }

        private void StartOpenHardwareSettingCommandExecute()
        {

            // backgroundWorker3.RunWorkerAsync();

            backgroundWorker3_DoWork(this, null);
        }
        private async void StartOpenPenSettingCommandExecute()
        {

            // backgroundWorker3.RunWorkerAsync();

            await _windowManager.ShowDialogAsync(new PenSettingsViewModel(_eventAggregator));
        }
        #region Worker Threads

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show("Marking Started");
            if (m_idMarker == EzdKernel.E3_ID.INVALID)
            {
                MessageBox.Show("Marker invalid!");
                return;
            }
            //Set the angle and position you want to transform
            EzdKernel.E3_ERR err, err2;
            err2 = EzdKernel.E3_ERR.SUCCESS;
            err = EzdKernel.E3_MarkerSetRotateMoveParam(m_idMarker, 0, 0, 0, 0, 0);
            if (err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("RotateMove=" + err.ToString());
                return;
            }
            int nErr = 0;

            int nMarkFlag = 0;

            if (err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("E3_MarkerFlyEnableToList=" + err.ToString());
                return;
            }

            nMarkFlag |= EzdKernel.MAKRFLAG_FLYMODE;

            int iErr = 0;
            err = EzdKernel.E3_MarkEnt2(m_idMarker, m_idEM, m_idCurLayer, EzdKernel.MAKRFLAG_FLYMODE, 0, 1, ref iErr);
            if (err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("E3_MarkEnt2=" + err.ToString() + ",Err=" + nErr.ToString());
                return;
            }
            else
            {
                MessageBox.Show("Marking finish!");
            }


            return;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show("Red Started");
            if (m_idMarker == EzdKernel.E3_ID.INVALID)
            {
                MessageBox.Show("Marker invalid!");
                return;
            }
            //设置希望变换的角度和位置
            EzdKernel.E3_ERR err, err2;
            err2 = EzdKernel.E3_ERR.SUCCESS;
            err = EzdKernel.E3_MarkerSetRotateMoveParam(m_idMarker, 0, 0, 0, 0, 0);
            if (err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("RotateMove=" + err.ToString());
                return;
            }
            int nErr = 0;

            int nMarkFlag = 0;

            if (err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("E3_MarkerFlyEnableToList=" + err.ToString());
                return;
            }

            nMarkFlag |= EzdKernel.MAKRFLAG_REDLIGHT;
        
            int iErr = 0;

            try
            {
                err = EzdKernel.E3_MarkEnt2(m_idMarker, m_idEM, m_idCurLayer, EzdKernel.MAKRFLAG_REDLIGHT, 0, 1, ref iErr);
                if (err != EzdKernel.E3_ERR.SUCCESS)
                {
                    MessageBox.Show("E3_MarkEnt2=" + err.ToString() + ",Err=" + nErr.ToString());
                    return;
                }
                else
                {
                    MessageBox.Show("Red finish!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }



            return;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Marking finish!");
        }

        private void bw_IO_DoWork(object sender, DoWorkEventArgs e)
        {
            int nStart, nEnd;
            nStart = Environment.TickCount;

            while (true)
            {
                if (bw_IO.CancellationPending) { e.Cancel = true; return; }

                nEnd = Environment.TickCount;
                if (nEnd - nStart >= 100)
                {
                    ChangeStatus("Checking!");
                    nStart = nEnd;
                    try
                    {
                        ushort uData = 0;
                        ushort uOutData = 0;
                        EzdKernel.E3_ERR Err = EzdKernel.E3_MarkerReadPort(m_idMarker, ref uData);
                        EzdKernel.E3_ERR Err2 = EzdKernel.E3_MarkerGetWritePort(m_idMarker, ref uOutData);

                        string _readData = Convert.ToString(uData, 2).PadLeft(16, '0');
                        string _readOutData = Convert.ToString(uOutData, 2).PadLeft(16, '0');
                        string temp = new String(_readData.ToCharArray().Reverse().ToArray());
                        string strOutData = new String(_readOutData.ToCharArray().Reverse().ToArray());

                        if (Err == 0)
                        {
                            if (globalValue.ReadyPortNumber != string.Empty)
                            {
                                if (globalValue.bReadyPortNumber == true)
                                {
                                    if (temp.Substring(Convert.ToInt16(globalValue.ReadyPortNumber), 1) == "0")
                                    {
                                        ChangeStatus("READY");
                                    }
                                    else
                                    {
                                        ChangeStatus("LASEROFF");
                                    }
                                }
                                else
                                {
                                    if (temp.Substring(Convert.ToInt16(globalValue.ReadyPortNumber), 1) == "1")
                                    {
                                        ChangeStatus("READY");
                                    }
                                    else
                                    {
                                        ChangeStatus("LASEROFF");
                                    }
                                }
                            }
                        }
                        else
                        {
                            ChangeStatus("Unable to check status!");
                        }
                        OldData = uData;
                        OldOutData = uOutData;
                    }
                    catch (Exception ex)
                    {
                        // Progress_Display(Color.Red, ex.ToString());
                        MessageBox.Show("MessageBox", "I/O Read error" + ex.ToString());
                    }
                }

            }
            Thread.Sleep(10);
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            bool bReturnOK = false;
            EzdKernel.E3_ERR Err = EzdKernel.E3_MarkerDlgSetCfg(m_idMarker, ref bReturnOK);
            if (Err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("Error: " + Err.ToString());
            }
        }
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            bool bReturnOK = false;
            EzdKernel.E3_ERR Err = EzdKernel.E3_MarkerDlgSetCfg(m_idMarker, ref bReturnOK);
            if (Err != EzdKernel.E3_ERR.SUCCESS)
            {
                MessageBox.Show("Error: " + Err.ToString());
            }
        }
        private void ChangeStatus(string message)
        {
            LazerStatus = message;
        }


        #endregion

        // Open Ezcad File
        private void OpenEzcadFileCommandExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Ezcad3 files (*.ez3)|*.ez3",
                Title = "Open Ezcad3 File"
            };

            DialogResult? result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result.HasValue == true)
            {

                string fileName = openFileDialog.FileName;
                // Process the file
                EzdKernel.E3_ERR err = EzdKernel.E3_OpenFileToEntMgr(fileName, m_idEM, false, false);

                int nLayerCount = 0;

                EzdKernel.E3_ERR err1 = EzdKernel.E3_GetLayerCount(m_idEM, ref nLayerCount, ref m_nCurrentLayerIndex);
                m_idLayerList = new EzdKernel.E3_ID[nLayerCount];
                for (int i = 0; i < nLayerCount; i++)
                {
                    m_idLayerList[i] = EzdKernel.E3_ID.INVALID;

                    EzdKernel.E3_ERR err2 = EzdKernel.E3_GetLayerId(m_idEM, i, ref m_idLayerList[i]);
                }
                if (nLayerCount > 0)
                {
                    m_idCurLayer = m_idLayerList[m_nCurrentLayerIndex];
                    EzdKernel.E3_ERR Err = EzdKernel.E3_FindEntInLayerByIndex(m_idCurLayer, 0, ref m_idEntbyIndex);
                    EzdKernel.E3_GetEntCountInLayer(m_idCurLayer, ref m_nEntCount);
                    m_IdEntList = new EzdKernel.E3_ID[m_nEntCount];
                }
                else
                {
                    m_idCurLayer = 0;
                    EzdKernel.E3_ERR Err = EzdKernel.E3_FindEntInLayerByIndex(m_idCurLayer, 0, ref m_idEntbyIndex);
                    EzdKernel.E3_GetEntCountInLayer(m_idCurLayer, ref m_nEntCount);
                    m_IdEntList = new EzdKernel.E3_ID[m_nEntCount];
                }

            }
        }

        // Check for supported figure types in canvas
        private bool IsSupportedFigureType(Infini.Draw2D.Core.Figure figure)
        {
            return figure.GetType() == typeof(Rectangle) || figure.GetType() == typeof(Ellipse) || figure.GetType() == typeof(Line) || figure.GetType() == typeof(Barcode) || figure.GetType() == typeof(BoxedText);
        }
    }
}
