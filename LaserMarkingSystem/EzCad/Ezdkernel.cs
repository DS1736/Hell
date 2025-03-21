using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ezcad
{
    using System.Drawing;
    using System.Runtime.InteropServices;
    using E3_POINTER = Int64;


    public struct Box2d
    {
        Pt2d a;
        Pt2d b;
        public Pt2d A
        {
            get { return a; }
            set { a = value; }
        }
        public Pt2d B
        {
            get { return b; }
            set { b = value; }
        }

    }

    public struct Pt2d
    {
        double x;
        double y;
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
    }
    
    public class EzdKernel : Object
    {
        public class E3_ID
        {
            private Int64 value = 0;
            public const Int64 INVALID = 0;

            public E3_ID(Int64 p)
            {
                value = p;
            }

            public static implicit operator E3_ID(Int64 v)
            {
                return new E3_ID(v);
            }

            public static implicit operator Int64(E3_ID id)
            {
                if (id == null)
                {
                    return 0;
                }
                return id.value;
            }
        }

        ////错误ID返回错误
        public enum E3_ERR
        {
            SUCCESS = 0,
            FAIL = 1,
            ERRORPARAM = 2,//错误输入参数
            OPENFILE = 3,//打开文件失败 
            NOENTITY = 4,//没有加工对象
            EZCADRUN = 5,//ezcad已经运行
            NOLICENSE = 6,//无license
            NOCARD = 7,//无连接板卡
        }


        #region Base
        /// <summary>
        /// 初始化函数库
        /// PathName 是Ezcad3kernel.dll.dll所在的目录
        /// </summary>     
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_Initial", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern E3_ERR E3_Initial(string PathName, int nFlag);


        ///// </summary>     
        //[DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_Initial", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //public static extern E3_ERR E3_Initial(string PathName, int nFlag);
        /// <summary>
        /// 设置是否提示控制卡丢失信息
        /// </summary>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_DisableInitialPrompt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void E3_DisableInitialPrompt();


        /// <summary>
        /// 设置语言文件
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetLanguageFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern E3_ERR E3_SetLanguageFile(string strFile);


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetEntTextInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetEntTextInfo(E3_POINTER idEM, E3_POINTER idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, string[] strText, string[] strExtFile, bool bUndo, string strUndo);
        public static E3_ERR E3_SetEntTextInfo(E3_ID idEM, E3_ID idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, string[] strText, string[] strExtFile, bool bUndo, string strUndo)
        {
            return e3_SetEntTextInfo(idEM, idEnt, nMaxParamN, nParam, nMaxParamD, dParam, strText, strExtFile, bUndo, strUndo);
        }

        /// <summary>
        /// 关闭函数库 
        /// </summary>     
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_Close", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern E3_ERR E3_Close();

        /// <summary>
        /// 创建对象管理器 
        /// </summary>     
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateEntMgr", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_POINTER e3_CreateEntMgr(int nUnitType);
        public static E3_ID E3_CreateEntMgr(int nUnitType)
        {
            return (E3_ID)e3_CreateEntMgr(nUnitType);
        }


        /// <summary>
        /// 得到对象基础信息
        /// </summary>
        /// <param name="idEnt"></param>
        /// <param name="nType"></param>
        /// <param name="nPen"></param>
        /// <param name="sbName"></param>
        /// <param name="box"></param>
        /// <param name="dz"></param>
        /// <param name="da"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetEntBaseInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetEntBaseInfo(E3_POINTER idEnt, ref int nType, ref int nPen, StringBuilder sbName, ref Box2d box, ref double dz, ref double da);
        public static E3_ERR E3_GetEntBaseInfo(E3_ID idEnt, ref int nType, ref int nPen, StringBuilder sbName, ref Box2d box, ref double dz, ref double da)
        {
            return e3_GetEntBaseInfo(idEnt, ref nType, ref nPen, sbName, ref box, ref dz, ref da);
        }


        /// <summary>
        /// 释放对象管理器 
        /// </summary>     
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_FreeEntMgr", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_FreeEntMgr(E3_POINTER idEM);
        public static E3_ERR E3_FreeEntMgr(E3_ID idEM)
        {
            return e3_FreeEntMgr(idEM);
        }


        //E3_API int E3_EnableShowHatchProcess(BOOL b);
        /// <summary>
        /// 显示填充进度条
        /// </summary>
        /// <param name="EnShow"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_EnableShowHatchProcess", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_EnableShowHatchProcess(bool EnShow);
        public static E3_ERR E3_EnableShowHatchProcess(bool EnShow)
        {
            return e3_EnableShowHatchProcess(EnShow);
        }


        //句柄
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();


        ////初始化监视USB
        ////pWndMonitor=接受监视消息的窗口
        ////#define WM_USER_USBLMC_REMOVE  (WM_USER+17)       表示有一个设备掉了
        ////#define WM_USER_USBLMC_ARRIVE  (WM_USER+18)       表示有找到一个新设备
        //E3_API int E3_MarkerInitUsbMonitor(CWnd* pWndMonitor);
        //E3_API int E3_MarkerInitUsbMonitor2(HWND hWndMonitor);

        /// <summary>
        /// 启动设备检测
        /// </summary>
        /// <param name="hWndMonitor">窗口句柄</param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerInitUsbMonitor2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern E3_ERR E3_MarkerInitUsbMonitor2(IntPtr hWndMonitor);



        /// <summary>
        /// 关闭设备检测
        /// </summary>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerCloseUsbMonitor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern E3_ERR E3_MarkerCloseUsbMonitor();


        /// <summary>
        /// 获取新连接控制卡ID
        /// </summary>
        /// <param name="idMarker"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerUsbMonitorGetNewDevice", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerUsbMonitorGetNewDevice(ref E3_POINTER idMarker);
        public static E3_ERR E3_MarkerUsbMonitorGetNewDevice(ref E3_ID idMarker)
        {
            E3_POINTER _MarkID = 0;
            E3_ERR Err = e3_MarkerUsbMonitorGetNewDevice(ref _MarkID);
            idMarker = _MarkID;
            return Err;
        }



        #endregion



        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        #region File

        //打开文件到对象管理器
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_OpenFileToEntMgr", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_OpenFileToEntMgr(string pStrFileName, E3_POINTER idEM, bool bAddMode, bool bUndo);
        public static E3_ERR E3_OpenFileToEntMgr(string pStrFileName, E3_ID idEM, bool bAddMode, bool bUndo)
        {
            return e3_OpenFileToEntMgr(pStrFileName, idEM, bAddMode, bUndo);
        }



        //  E3_API int E3_GetPenParamWobble(E3_ID idEM, int nPenNo, BOOL& bWobbleMode, int& nWobbleType, double& dWobbleDiameter, double& dWobbleDiameterB, double& dWobbleDist);
        //  E3_API int E3_SetPenParamWobble(E3_ID idEM, int nPenNo, BOOL bWobbleMode, int nWobbleType, double dWobbleDiameter, double dWobbleDiameterB, double dWobbleDist);
        //获取笔参数抖动
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetPenParamWobble", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         static extern E3_ERR e3_GetPenParamWobble(E3_POINTER idEM, int nPenNo, ref bool bWobbleMode, ref int nWobbleType,ref  double dWobbleDiameter,ref  double dWobbleDiameterB, ref  double dWobbleDist);
        public static E3_ERR E3_GetPenParamWobble( E3_ID idEM, int nPenNo, ref bool bWobbleMode, ref int nWobbleType, ref double dWobbleDiameter, ref double dWobbleDiameterB, ref double dWobbleDist)
        {
            return e3_GetPenParamWobble(idEM,  nPenNo, ref bWobbleMode, ref nWobbleType, ref dWobbleDiameter, ref dWobbleDiameterB, ref dWobbleDist);
        }
        //设置笔参数抖动
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetPenParamWobble", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
         static extern E3_ERR e3_SetPenParamWobble(E3_POINTER idEM, int nPenNo, bool bWobbleMode, int nWobbleType, double dWobbleDiameter, double dWobbleDiameterB, double dWobbleDist);
        public static E3_ERR E3_SetPenParamWobble(E3_ID idEM, int nPenNo,  bool bWobbleMode,  int nWobbleType,  double dWobbleDiameter,  double dWobbleDiameterB,  double dWobbleDist)
        {
            return e3_SetPenParamWobble(idEM, nPenNo,  bWobbleMode,  nWobbleType,  dWobbleDiameter,  dWobbleDiameterB,  dWobbleDist);
        }
        //添加矢量文件到对象管理器
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_ImportFileToEntMgr", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_ImportFileToEntMgr(string pStrFileName, E3_POINTER idEM, bool bUndo, string strUndoName);
        public static E3_ERR E3_ImportFileToEntMgr(string pStrFileName, E3_ID idEM, bool bUndo, string strUndoName)
        {
            return e3_ImportFileToEntMgr(pStrFileName, idEM, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SaveEntMgrToFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SaveEntMgrToFile(string pStrFileName, E3_POINTER idEM, bool bSelect, bool bPreImage, string strAuthor, string strTime, string strNotes);
        public static E3_ERR E3_SaveEntMgrToFile(string pStrFileName, E3_ID idEM, bool bSelect, bool bPreImage, string strAuthor, string strTime, string strNotes)
        {
            return e3_SaveEntMgrToFile(pStrFileName, idEM, bSelect, bPreImage, strAuthor, strTime, strNotes);
        }

        #endregion


        #region EntContrle
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CopyEntity", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_POINTER e3_CopyEntity(E3_POINTER idEnt);
        public static E3_ID E3_CopyEntity(E3_ID idEnt)
        {
            return e3_CopyEntity(idEnt);

        }
        /// <summary>
        /// 添加多段线，可以设置创建曲线,但是不更新对象列表信息,最后更新,避免大量创建时由于更新对象列表导致的卡顿
        /// </summary>
        /// <param name="idEM"></param>
        /// <param name="nPen"></param>
        /// <param name="ptLines"></param>
        /// <param name="nPointCount"></param>
        /// <param name="bUpdateParentInfo"></param>
        /// <param name="idEnt"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateLines_2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateLines_2(E3_POINTER idEM, int nPen, Pt2d[] ptLines, int nPointCount, bool bUpdateParentInfo, ref E3_POINTER idEnt);
        public static E3_ERR E3_CreateLines_2(E3_ID idEM, int nPen, Pt2d[] ptLines, int nPointCount, bool bUpdateParentInfo, ref E3_ID idEnt)
        {
            E3_POINTER id = idEM;
            E3_ERR err = e3_CreateLines_2(idEM, nPen, ptLines, nPointCount, bUpdateParentInfo, ref id);
            idEnt = id;
            return err;
        }


        //得到指定图层指定索引的对象id
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_FindEntInLayerByIndex", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_FindEntInLayerByIndex(E3_POINTER idLayer, int nEntIndex, ref E3_POINTER idEnt);
        public static E3_ERR E3_FindEntInLayerByIndex(E3_ID idLayer, int nEntIndex, ref E3_ID idEnt)
        {
            E3_POINTER id = idLayer;
            E3_ERR err = e3_FindEntInLayerByIndex(idLayer, nEntIndex, ref id);
            idEnt = id;
            return err;
        }


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateLines", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateLines(E3_POINTER idEM, int nPen, Pt2d[] ptLines, int nPointCount, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreateLines(E3_ID idEM, int nPen, Pt2d[] ptLines, int nPointCount, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreateLines(idEM, nPen, ptLines, nPointCount, bPick, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateRect", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateRect(E3_POINTER idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreateRect(E3_ID idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreateRect(idEM, nPen, pt1, pt2, bPick, bUndo, strUndoName);
        }
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateCircle", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateCircle(E3_POINTER idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreateCircle(E3_ID idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreateCircle(idEM, nPen, pt1, pt2, bPick, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateSpiral", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateSpiral(E3_POINTER idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreateSpiral(E3_ID idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreateSpiral(idEM, nPen, pt1, pt2, bPick, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateEllipse", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateEllipse(E3_POINTER idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreateEllipse(E3_ID idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreateEllipse(idEM, nPen, pt1, pt2, bPick, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreatePolygon", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreatePolygon(E3_POINTER idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreatePolygon(E3_ID idEM, int nPen, Pt2d pt1, Pt2d pt2, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreatePolygon(idEM, nPen, pt1, pt2, bPick, bUndo, strUndoName);
        }
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetTextInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetEntTextInfo(E3_POINTER idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam);
        public static E3_ERR E3_GetEntTextInfo(E3_ID idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam)
        {
            return e3_GetEntTextInfo(idEnt, nMaxParamN, nParam, nMaxParamD, dParam);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetEntTextInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetEntTextInfo(E3_POINTER idEM, E3_POINTER idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, StringBuilder strText, StringBuilder strExtFile, bool bUndo, string strUndo);
        public static E3_ERR E3_SetEntTextInfo(E3_ID idEM, E3_ID idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, StringBuilder strText, StringBuilder strExtFile, bool bUndo, string strUndo)
        {
            return e3_SetEntTextInfo(idEM, idEnt, nMaxParamN, nParam, nMaxParamD, dParam, strText, strExtFile, bUndo, strUndo);
        }
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetTextStringInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetTextStringInfo(E3_POINTER idEnt, int index, int nMaxCharLen, StringBuilder strData);
        public static E3_ERR E3_GetTextStringInfo(E3_ID idEnt, int index, int nMaxCharLen, StringBuilder strData)
        {
            return e3_GetTextStringInfo(idEnt, index, nMaxCharLen, strData);
        }
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetTextBarcodeInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetTextBarcodeInfo(E3_POINTER idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, StringBuilder strBarTypeName, StringBuilder strTextTypeName, ref HatchParam param1, ref HatchParam param2, ref HatchParam param3);
        public static E3_ERR E3_GetTextBarcodeInfo(E3_ID idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, StringBuilder strBarTypeName, StringBuilder strTextTypeName, ref HatchParam param1, ref HatchParam param2, ref HatchParam param3)
        {
            return e3_GetTextBarcodeInfo(idEnt, nMaxParamN, nParam, nMaxParamD, dParam, strBarTypeName, strTextTypeName, ref param1, ref param2, ref param3);
        }
      
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_IsBarcodeTextValid", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public extern static E3_ERR E3_IsBarcodeTextValid(int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, string strBarcodeFontName, string strText, ref bool bValid);


        public const int BMPSCAN_INVERT = 0x0001;//图像反转
        public const int BMPSCAN_GRAY = 0x0002;//图像灰度
        public const int BMPSCAN_LIGHT = 0x0004;//图像亮度
        public const int BMPSCAN_DITHER = 0x0010;//网点处理

        public const int BMPSCAN_BIDIR = 0x1000;//双向扫描
        public const int BMPSCAN_YDIR = 0x2000;//Y向扫描
        public const int BMPSCAN_DRILL = 0x4000;//打点模式
        public const int BMPSCAN_POWER = 0x8000;//调整功率


        public const int BMPATTRIB_DYNFILE = 0x1000;//动态文件
        public const int BMPATTRIB_IMPORTFIXED_WIDTH = 0x2000;//固定文件输入宽 
        public const int BMPATTRIB_IMPORTFIXED_HEIGHT = 0x4000;//固定文件输入高
        public const int BMPATTRIB_IMPORTFIXED_DPI = 0x8000;//固定DPI
        public const int BMPSCAN_OFFSETPT = 0x0100;//隔行错位
         public const int BMPSCAN_OPTIMIZE = 0x0200;//优化模式
         public   const int MAX_DPI = 4000;//最高DPI
        public const int MIN_DPI = 10;//最低DPI
        public const int MAX_GRAYCURVEPT_NUM = 20;//功率曲线节点数量    

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateBitmap", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateBitmap(E3_POINTER idEM, int nPen, string strBmpFile, int nBmpAttrib, int nScanAttrib, Pt2d ptBase, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreateBitmap(E3_ID idEM, int nPen, string strBmpFile, int nBmpAttrib, int nScanAttrib, Pt2d ptBase, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreateBitmap(idEM, nPen, strBmpFile, nBmpAttrib, nScanAttrib, ptBase, bPick, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateControl", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateControl(E3_POINTER idEM, int nPen, int nControlType, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreateControl(E3_ID idEM, int nPen, int nControlType, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreateControl(idEM, nPen, nControlType, bPick, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateText", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateText(E3_POINTER idEM, int nPen, Pt2d ptBase, string str, int nFontId, double dTextHeight, double dTextWidthRatio, int nTextSpaceType, double dTextSpace, double dTextAngle,
         int nAlignType, double dNullCharWidthRatio, double dLineSpace, bool bVerText, bool bBold, bool bItalic, bool bPick, bool bUndo, string strUndo);
        public static E3_ERR E3_CreateText(E3_ID idEM, int nPen, Pt2d ptBase, string str, int nFontId, double dTextHeight, double dTextWidthRatio, int nTextSpaceType, double dTextSpace, double dTextAngle,
      int nAlignType, double dNullCharWidthRatio, double dLineSpace, bool bVerText, bool bBold, bool bItalic, bool bPick, bool bUndo, string strUndo)
        {
            return e3_CreateText(idEM, nPen, ptBase, str, nFontId, dTextHeight, dTextWidthRatio, nTextSpaceType, dTextSpace, dTextAngle,
          nAlignType, dNullCharWidthRatio, dLineSpace, bVerText, bBold, bItalic, bPick, bUndo, strUndo);
        }

        /// <summary>
        /// 添加STL文件模型（3D或者2.5D切片使用）
        /// </summary>
        /// <param name="idEM"></param>
        /// <param name="strFile"></param>
        /// <param name="bAddMode"></param>
        /// <param name="bPick"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreateAddStlFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreateAddStlFile(E3_POINTER idEM, string strFile, bool bAddMode, bool bPick);
        public static E3_ERR E3_CreateAddStlFile(E3_ID idEM, string strFile, bool bAddMode, bool bPick)
        {
            return e3_CreateAddStlFile(idEM, strFile, bAddMode, bPick);
        }

        // <summary>
        /// <summary>
        /// 在DC里绘制对象
        /// </summary>  
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_DrawEnt2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_DrawEnt2(IntPtr hdc, E3_POINTER idEnt, int nParam, int bmpwidth, int bmpheight, double dLogOriginX, double dLogOriginY, double dLogScale);
        public static E3_ERR E3_DrawEnt(IntPtr hdc, E3_ID idEnt, int nParam, int bmpwidth, int bmpheight, double dLogOriginX, double dLogOriginY, double dLogScale)
        {
            return e3_DrawEnt2(hdc, idEnt, nParam, bmpwidth, bmpheight, dLogOriginX, dLogOriginY, dLogScale);
        }

        //得到指定图层对象数
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetEntCountInLayer", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetEntCountInLayer(E3_POINTER idLayer, ref int nCount);
        public static E3_ERR E3_GetEntCountInLayer(E3_ID idLayer, ref int nCount)
        {
            return e3_GetEntCountInLayer(idLayer, ref nCount);
        }

        //得到指定图层指定名字的对象id
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_FindEntInLayerByName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_FindEntInLayerByName(E3_POINTER idLayer, string strEntName, ref E3_POINTER idEnt);
        public static E3_ERR E3_FindEntInLayerByName(E3_ID idLayer, string strEntName, ref E3_ID idEnt)
        {
            E3_POINTER id = idLayer;
            E3_ERR err = e3_FindEntInLayerByName(idLayer, strEntName, ref id);
            idEnt = id;
            return err;
        }

        //得到对象的尺寸
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetEntityRange", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetEntityRange(E3_POINTER idEnt, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref bool bIsEmpty);
        public static E3_ERR E3_GetEntityRange(E3_ID idEnt, ref double dMinx, ref double dMiny, ref double dMaxx, ref double dMaxy, ref bool bIsEmpty)
        {
            return e3_GetEntityRange(idEnt, ref dMinx, ref dMiny, ref dMaxx, ref dMaxy, ref bIsEmpty);
        }

        //移动指定对象
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerSetRotateMoveParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MoveRotateEnt(E3_POINTER idEnt, double dMoveX, double dMoveY, double dCenterX, double dCenterY, double dRotateAng);
        public static E3_ERR E3_MoveRotateEnt(E3_ID idEnt, double dMoveX, double dMoveY, double dCenterX, double dCenterY, double dRotateAng)
        {
            return e3_MoveRotateEnt(idEnt, dMoveX, dMoveY, dCenterX, dCenterY, dRotateAng);
        }

        //拉伸指定对象
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_ScaleEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_ScaleEnt(E3_POINTER idEnt, double dCenx, double dCeny, double dScaleX, double dScaleY);
        public static E3_ERR E3_ScaleEnt(E3_ID idEnt, double dCenx, double dCeny, double dScaleX, double dScaleY)
        {
            return e3_ScaleEnt(idEnt, dCenx, dCeny, dScaleX, dScaleY);
        }


        //得到指定数据管理器中的图层数和当前层ID
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_AddLayer", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_AddLayer(E3_POINTER idEM, bool bUndo, string strUndoName);
        public static E3_ERR E3_AddLayer(E3_ID idEM, bool bUndo, string strUndoName)
        {
            return e3_AddLayer(idEM, bUndo, strUndoName);
        }

        //得到指定数据管理器中的图层数和当前层ID
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetLayerCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetLayerCount(E3_POINTER idEM, ref int nLayerCount, ref int nCurLayerIndex);
        public static E3_ERR E3_GetLayerCount(E3_ID idEM, ref int nLayerCount, ref int nCurLayerIndex)
        {
            return e3_GetLayerCount(idEM, ref nLayerCount, ref nCurLayerIndex);
        }
        //得到对象管理器中指定序号的图层ID
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetLayerId", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetLayerId(E3_POINTER idEM, int nLayerIndex, ref E3_POINTER idLayer);
        public static E3_ERR E3_GetLayerId(E3_ID idEM, int nLayerIndex, ref E3_ID idLayer)
        {
            E3_POINTER id = idLayer;
            E3_ERR err = e3_GetLayerId(idEM, nLayerIndex, ref id);
            idLayer = id;
            return err;
        }


        //更改当前数据库里的指定文本对象的文本
        //输入参数: strTextName     要更改内容的文本对象的名称
        //          strTextNew      新的文本内容 
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_ChangeTextByName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_ChangeTextByName(E3_POINTER idLayer, string strTextName, string strTextNew);
        public static E3_ERR E3_ChangeTextByName(E3_ID idLayer, string strTextName, string strTextNew)
        {
            return e3_ChangeTextByName(idLayer, strTextName, strTextNew);
        }
        public static bool E3_ChangeAllEntTextByName(E3_ID idLayer, string strTextName, string strTextNew)
        {
            int ncount = 0;
            E3_ERR err = E3_GetEntCountInLayer(idLayer, ref ncount);
            if (err != E3_ERR.SUCCESS)
            {
                return false;
            }
            for (int i = 0; i < ncount; i++)
            {
                E3_ID temID = 0;
                E3_FindEntInLayerByIndex(idLayer, i, ref temID);
                StringBuilder st = new StringBuilder(256);
                string strTemName = "";
                if (E3_GetEntityName(temID,   st) != E3_ERR.SUCCESS)
                {
                    return false;
                }
                strTemName = st.ToString();
                if (strTemName == strTextName)
                {
                    if (E3_ChangeTextById(temID, strTextNew) != E3_ERR.SUCCESS)
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        //更改当前数据库里的指定文本对象的文本
        //输入参数:  strTextNew      新的文本内容 
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_ChangeTextById", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_ChangeTextById(E3_POINTER idEnt, string strTextNew);
        public static E3_ERR E3_ChangeTextById(E3_ID idEnt, string strTextNew)
        {
            return e3_ChangeTextById(idEnt, strTextNew);
        }
        
        //获取对象名称
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetEntityName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetEntityName(E3_POINTER idEnt,  StringBuilder strEntName);
        public static E3_ERR E3_GetEntityName(E3_ID idEnt, StringBuilder strEntName)
        {
            return e3_GetEntityName(idEnt,  strEntName);
        }
        //投影指定对象
        //E3_API int E3_ProjectEnt(E3_ID idEM, E3_ID idEnt, BOOL bDownProject, double dLimetZ, double dDefaultZ, BOOL bRemoveNoInter, BOOL bUndo, TCHAR* strUndo);
        /// <summary>
        ///对象进行投影
        /// </summary>
        /// <param name="idEM"></param>
        /// <param name="idEnt"></param>
        /// <param name="bDownProject">从下往上投影</param>
        /// <param name="dLimetZ">Z极限值</param>
        /// <param name="dDefaultZ"></param>
        /// <param name="bRemoveNoInter"></param>
        /// <param name="bUndo"></param>
        /// <param name="strUndoName"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_ProjectEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern bool e3_ProjectEnt(E3_POINTER idEM, E3_POINTER idEnt, bool bDownProject, double dLimetZ, double dDefaultZ, bool bRemoveNoInter, bool bUndo, string strUndoName);
        public static bool E3_ProjectEnt(E3_ID idEM, E3_ID idEnt, bool bDownProject, double dLimetZ, double dDefaultZ, bool bRemoveNoInter, bool bUndo, string strUndoName)
        {
            return e3_ProjectEnt(idEM, idEnt, bDownProject, dLimetZ, dDefaultZ, bRemoveNoInter, bUndo, strUndoName);
        }
        //得到指定对象ID的文本内容
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetTextByID", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetTextByID(E3_POINTER idEnt, ref StringBuilder strEntName);
        public static E3_ERR E3_GetTextByID(E3_ID idEnt, ref StringBuilder strEntName)
        {
            return e3_GetTextByID(idEnt, ref strEntName);
        }
        //设置指定对象ID的对象名称
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetEntityName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetEntityName(E3_POINTER idEnt, string szEntName);
        public static E3_ERR E3_SetEntityName(E3_ID idEnt, string szEntName)
        {
            return e3_SetEntityName(idEnt, szEntName);
        }
        /// <summary>
        /// 获取指定文本对象的字体类型，以及是否是条码
        /// </summary>
        /// <param name="idEnt"></param>
        /// <param name="strFontName"></param>
        /// <param name="bIsbarcode"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetTextFontName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetTextFontName(E3_POINTER idEnt, StringBuilder strFontName, ref bool bIsbarcode);
        public static E3_ERR E3_GetTextFontName(E3_ID idEnt, StringBuilder strFontName, ref bool bIsbarcode)
        {
            return e3_GetTextFontName(idEnt, strFontName, ref bIsbarcode);
        }

        /// <summary>
        /// 查询二维码行列数
        /// </summary>
        /// <param name="idEnt"></param>
        /// <param name="str">条码数据内容</param>
        /// <param name="nSizeMode">条码版本号</param>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetTextBarcodeInfo2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetTextBarcodeInfo2(E3_POINTER idEnt, string str, ref int nSizeMode, ref int nRow, ref int nCol);
        public static E3_ERR E3_GetTextBarcodeInfo2(E3_ID idEnt, string str, ref int nSizeMode, ref int nRow, ref int nCol)
        {
            return e3_GetTextBarcodeInfo2(idEnt, str, ref nSizeMode, ref nRow, ref nCol);
        }
        //把新对象idEntNew添加到idEntOther前后
        //idEntMgr可以为空，如果不为空会自动刷新对象信息表
        //bToHead=TRUE表示dEntNew添加到idEntNewParent子对象尾
        //bToHead=FALSE表示dEntNew添加到idEntNewParent子对象头
        //int E3_AddEntityToChild(E3_ID idEntMgr, E3_ID idEntNew, E3_ID idEntNewParent, BOOL bToHead)

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_AddEntityToChild", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_AddEntityToChild(E3_POINTER idEntMgr, E3_POINTER idEntNew, E3_POINTER idEntNewParent, bool bToHead);
        public static E3_ERR E3_AddEntityToChild(E3_ID idEntMgr, E3_ID idEntNew, E3_ID idEntNewParent, bool bToHead)
        {
            return e3_AddEntityToChild(idEntMgr, idEntNew, idEntNewParent, bToHead);
        }

        //把新对象idEntNew添加到idEntOther前后
        //idEntMgr可以为空，如果不为空会自动刷新对象信息表
        //bBeforeOther=TRUE表示dEntNew添加到idEntOther前
        //bBeforeOther=FALSE表示dEntNew添加到idEntOther后
        //E3_API int E3_AddEntityToOther(E3_ID idEntMgr, E3_ID idEntNew, E3_ID idEntOther, BOOL bBeforeOther);
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_AddEntityToOther", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_AddEntityToOther(E3_POINTER idEntMgr, E3_POINTER idEntNew, E3_POINTER idEntOther, bool bBeforeOther);
        public static E3_ERR E3_AddEntityToOther(E3_ID idEntMgr, E3_ID idEntNew, E3_ID idEntOther, bool bBeforeOther)
        {
            return e3_AddEntityToOther(idEntMgr, idEntNew, idEntOther, bBeforeOther);
        }




        /// <summary>
        /// 组合一组对象
        /// </summary>
        //E3_API int E3_GroupEnt(E3_ID* idEnts, int nEntCount, E3_ID&  idEntGroup);
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GroupEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GroupEnt(E3_POINTER[] idEnts, int nEntCount, ref E3_POINTER idEntGroup);
        public static E3_ERR E3_GroupEnt(E3_ID[] idEnts, int nEntCount, ref E3_ID idEntGroup)
        {
            E3_POINTER id = 0;
            E3_POINTER[] idList = new E3_POINTER[idEnts.Length];
            for (int i = 0; i < idEnts.Length; i++)
            {
                idList[i] = idEnts[i];
            }
            E3_ERR err = e3_GroupEnt(idList, nEntCount, ref id);
            idEntGroup = id;
            return err;
        }

        /// <summary>
        /// 解散群组（解散到曲线后再调用此接口时返回值为2）
        /// </summary>
        //E3_API int E3_GroupEnt(E3_ID* idEnts, int nEntCount, E3_ID&  idEntGroup);
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_UnGroupEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_UnGroupEnt(E3_POINTER idEnt);
        public static E3_ERR E3_UnGroupEnt(E3_ID idEnt)
        {
            return e3_UnGroupEnt(idEnt);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="idEnt"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_DeleteEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_DeleteEnt(E3_POINTER idEnt);
        public static E3_ERR E3_DeleteEnt(E3_ID idEnt)
        {
            return e3_DeleteEnt(idEnt);
        }

        //得到飞行编码器脉冲数  index=1或2
        //   int E3_MarkerGetFlyEncoder(E3_ID idMarker, int index, unsigned int& nCount)
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetFlyEncoder", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetFlyEncoder(E3_POINTER idMarker, int index, ref uint nCount);
        public static E3_ERR E3_MarkerGetFlyEncoder(E3_ID idMarker, int index, ref uint nCount)
        {
            return e3_MarkerGetFlyEncoder(idMarker, index, ref nCount);
        }

        // int E3_MarkerGetSpeedOfFly(E3_ID idMarker, int index, double& dSpeed)
        //获取流水线速度
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetSpeedOfFly", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetSpeedOfFly(E3_POINTER idMarker, int index, ref double dSpeed);
        public static E3_ERR E3_MarkerGetSpeedOfFly(E3_ID idMarker, int index, ref double dSpeed)
        {
            return e3_MarkerGetSpeedOfFly(idMarker, index, ref dSpeed);
        }

        /// <summary>
        /// 删除一个集合ID下的所有对象，例如一个图层内的所有子对象
        /// </summary>
        /// <param name="idEnt"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_DeleteAllChildOfEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_DeleteAllChildOfEnt(E3_POINTER idEnt);
        public static E3_ERR E3_DeleteAllChildOfEnt(E3_ID idEnt)
        {
            return e3_DeleteAllChildOfEnt(idEnt);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_DeleteLayer", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_DeleteLayer(E3_POINTER idEM, int nLayerIndex, bool bUndo, string strUndoName);
        public static E3_ERR E3_DeleteLayer(E3_ID idEM, int nLayerIndex, bool bUndo, string strUndoName)
        {
            return e3_DeleteLayer(idEM, nLayerIndex, bUndo, strUndoName);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetCurLayer", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetCurLayer(E3_POINTER idEM, int nLayerIndex);
        public static E3_ERR E3_SetCurLayer(E3_ID idEM, int nLayerIndex)
        {
            return e3_SetCurLayer(idEM, nLayerIndex);
        }

        #endregion

        #region EntInfo
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetEntInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR E3_GetEntInfo(E3_POINTER idEnt, int nFlag, ref int np1, ref int np2, ref int np3, ref int np4, ref int np5, ref int np6,
ref double dp1, ref double dp2, ref double dp3, ref double dp4, ref double dp5, ref double dp6, StringBuilder str1, byte[] pParam1);


        public static E3_ERR E3_GetEntSpiralInfo(E3_ID idEnt, ref int nSpiralMode, ref int bInsideToOutside, ref int nOutsideLoop, ref int nInsideLoop, ref double dSpiralPitchDistMin, ref double dSpiralPitchDistMax, ref double dSpiralPitchDistInc, ref double dMinRadius, ref double dTolError)
        {
            int np1 = 0;
            int np2 = 0;
            int np3 = 0;
            int np4 = 0;
            int np5 = 0;
            int np6 = 0;
            double dp1 = 0;
            double dp2 = 0;
            double dp3 = 0;
            double dp4 = 0;
            double dp5 = 0;
            double dp6 = 0;


            E3_ERR err = E3_GetEntInfo(idEnt, 0, ref np1, ref np2, ref np3, ref np4, ref np5, ref np6, ref dp1, ref dp2, ref dp3, ref dp4, ref dp5, ref dp6, null, null);
            nSpiralMode = np1;//填充属性 0=等间距  1=外疏内密  2=外密内疏  
            bInsideToOutside = np2;
            nOutsideLoop = np3;//外环数
            nInsideLoop = np4;//内环数 

            dSpiralPitchDistMin = dp1;//最小螺旋线间距
            dSpiralPitchDistMax = dp2;//最大螺旋线间距
            dSpiralPitchDistInc = dp3;//螺旋线间距增量 
            dMinRadius = dp4;//最小半径
            dTolError = dp5;//离散精度
            return err;
        }
        #endregion

        #region PenParam
        /// </summary> 
        /// <summary>
        /// 得到标记笔参数
        /// </summary>   
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetPenParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetPenParam(E3_POINTER idEM,
                                            int nPenNo,
                                            StringBuilder pStrName,
                                            ref int clr,
                                            ref bool bDisableMark,
                                            ref bool bUseDefParam,
                                            ref int nMarkLoop,
                                            ref double dMarkSpeed,
                                            ref double dPowerRatio,
                                            ref double dCurrent,
                                            ref double dFreq,
                                            ref double dQPulseWidth,
                                            ref int nStartTC,
                                            ref int nLaserOffTC,
                                            ref int nEndTC,
                                            ref int nPolyTC,
                                            ref double dJumpSpeed,
                                            ref int nMinJumpDelayTCUs,
                                            ref int nMaxJumpDelayTCUs,
                                            ref double dJumpLengthLimit,
                                            ref double dPointTimeMs,
                                            ref bool nSpiSpiContinueMode,
                                            ref int nSpiWave,
                                            ref int nYagMarkMode,
                                            ref bool bPulsePointMode,
                                            ref int nPulseNum,
                                            ref bool bEnableACCMode,
                                            ref double dEndComp,
                                            ref double dAccDist,
                                            ref double dBreakAngle,
                                            ref bool bWobbleMode,
                                            ref double bWobbleDiameter,
                                            ref double bWobbleDist);

        public static E3_ERR E3_GetPenParam(E3_ID idEM,
                                             int nPenNo,
                                             StringBuilder pStrName,
                                             ref int clr,
                                             ref bool bDisableMark,
                                             ref bool bUseDefParam,
                                             ref int nMarkLoop,
                                             ref double dMarkSpeed,
                                             ref double dPowerRatio,
                                             ref double dCurrent,
                                             ref double dFreq,
                                             ref double dQPulseWidth,
                                             ref int nStartTC,
                                             ref int nLaserOffTC,
                                             ref int nEndTC,
                                             ref int nPolyTC,
                                             ref double dJumpSpeed,
                                             ref int nMinJumpDelayTCUs,
                                             ref int nMaxJumpDelayTCUs,
                                             ref double dJumpLengthLimit,
                                             ref double dPointTimeMs,
                                             ref bool nSpiSpiContinueMode,
                                             ref int nSpiWave,
                                             ref int nYagMarkMode,
                                             ref bool bPulsePointMode,
                                             ref int nPulseNum,
                                             ref bool bEnableACCMode,
                                             ref double dEndComp,
                                             ref double dAccDist,
                                             ref double dBreakAngle,
                                             ref bool bWobbleMode,
                                             ref double bWobbleDiameter,
                                             ref double bWobbleDist)
        {
            return e3_GetPenParam(idEM,
            nPenNo,
            pStrName,
            ref clr,
            ref bDisableMark,
            ref bUseDefParam,
            ref nMarkLoop,
            ref dMarkSpeed,
            ref dPowerRatio,
            ref dCurrent,
            ref dFreq,
            ref dQPulseWidth,
            ref nStartTC,
            ref nLaserOffTC,
            ref nEndTC,
            ref nPolyTC,
            ref dJumpSpeed,
            ref nMinJumpDelayTCUs,
            ref nMaxJumpDelayTCUs,
            ref dJumpLengthLimit,
            ref dPointTimeMs,
            ref nSpiSpiContinueMode,
            ref nSpiWave,
            ref nYagMarkMode,
            ref bPulsePointMode,
            ref nPulseNum,
            ref bEnableACCMode,
            ref dEndComp,
            ref dAccDist,
            ref dBreakAngle,
            ref bWobbleMode,
            ref bWobbleDiameter,
            ref bWobbleDist);
        }


        /// </summary> 
        /// <summary>
        /// 设置标记笔参数
        /// </summary>   
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetPenParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetPenParam(E3_POINTER idEM, int nPenNo,
                                                            string pStrName,
                                                            int clr,
                                                            bool bDisableMark,
                                                            bool bUseDefParam,
                                                            int nMarkLoop,
                                                            double dMarkSpeed,
                                                            double dPowerRatio,
                                                            double dCurrent,
                                                            double dFreq,
                                                            double dQPulseWidth,
                                                            int nStartTC,
                                                            int nLaserOffTC,
                                                            int nEndTC,
                                                            int nPolyTC,
                                                            double dJumpSpeed,
                                                            int nMinJumpDelayTCUs,
                                                            int nMaxJumpDelayTCUs,
                                                            double dJumpLengthLimit,
                                                            double dPointTimeMs,
                                                            bool nSpiSpiContinueMode,
                                                            int nSpiWave,
                                                            int nYagMarkMode,
                                                            bool bPulsePointMode,
                                                            int nPulseNum,
                                                            bool bEnableACCMode,
                                                            double dEndComp,
                                                            double dAccDist,
                                                            double dBreakAngle,
                                                            bool bWobbleMode,
                                                            double bWobbleDiameter,
                                                            double bWobbleDist);

        public static E3_ERR E3_SetPenParam(E3_ID idEM, int nPenNo,
                                                        string pStrName,
                                                        int clr,
                                                        bool bDisableMark,
                                                        bool bUseDefParam,
                                                        int nMarkLoop,
                                                        double dMarkSpeed,
                                                        double dPowerRatio,
                                                        double dCurrent,
                                                        double dFreq,
                                                        double dQPulseWidth,
                                                        int nStartTC,
                                                        int nLaserOffTC,
                                                        int nEndTC,
                                                        int nPolyTC,
                                                        double dJumpSpeed,
                                                        int nMinJumpDelayTCUs,
                                                        int nMaxJumpDelayTCUs,
                                                        double dJumpLengthLimit,
                                                        double dPointTimeMs,
                                                        bool nSpiSpiContinueMode,
                                                        int nSpiWave,
                                                        int nYagMarkMode,
                                                        bool bPulsePointMode,
                                                        int nPulseNum,
                                                        bool bEnableACCMode,
                                                        double dEndComp,
                                                        double dAccDist,
                                                        double dBreakAngle,
                                                        bool bWobbleMode,
                                                        double bWobbleDiameter,
                                                        double bWobbleDist)
        {
            return e3_SetPenParam(idEM, nPenNo,
            pStrName,
            clr,
            bDisableMark,
            bUseDefParam,
            nMarkLoop,
            dMarkSpeed,
            dPowerRatio,
            dCurrent,
            dFreq,
            dQPulseWidth,
            nStartTC,
            nLaserOffTC,
            nEndTC,
            nPolyTC,
            dJumpSpeed,
            nMinJumpDelayTCUs,
            nMaxJumpDelayTCUs,
            dJumpLengthLimit,
            dPointTimeMs,
            nSpiSpiContinueMode,
            nSpiWave,
            nYagMarkMode,
            bPulsePointMode,
            nPulseNum,
            bEnableACCMode,
            dEndComp,
            dAccDist,
            dBreakAngle,
            bWobbleMode,
            bWobbleDiameter,
            bWobbleDist);
        }


        #endregion


        #region Hatch

        public struct HatchParam
        {
          
                int m_nHatchParamSize;//HatchParam的尺寸（默认是120即可）
                int m_bEnableHatch;//填充使能
                int m_bAllCalc;  //整体计算
                int m_bFollowEdge;//绕边
                int m_bAverageLine;//平均分布填充线
                int m_bLoopReverse;//反转
                int m_bCross;//交叉填充  

                int m_nHatchType; //填充类型（0-6依次对应填充类型中从单向填充开始依次对应）
                int m_nPenNo;//填充笔号  
                double m_dHatchLineDist;//填充线间距
                double m_dHatchAngle;//填充线角度
                double m_dHatchEdgeDist;//填充线边距 
                double m_dHatchStartOffset;//填充线起始偏移距离
                double m_dHatchEndOffset;//填充线结束偏移距离

                int m_bEnableAutoRotate;//使能自动旋转
                double m_dHatchRotateAngle;//旋转角度

                double m_dHatchLineReduction;//直线缩进
                double m_dHatchLoopDist;//环间距
                int m_nEdgeLoop;//边界环数
         

            public HatchParam(int BaseParam)
            {
                m_nHatchParamSize = 120;
                m_bEnableHatch = 1;//填充使能
                m_bAllCalc = 1;  //整体计算
                m_bFollowEdge = 1;//绕边
                m_bAverageLine = 1;//平均分布填充线
                m_bLoopReverse = 0;//反转
                m_bCross = 0;
                m_nHatchType = 1; //填充类型
                m_nPenNo = 0;//填充笔	 
                m_dHatchLineDist = 0.01;//填充线间距
                m_dHatchAngle = 0;//填充线角度
                m_dHatchEdgeDist = 0;//填充线边距 
                m_dHatchStartOffset = 0;//填充线起始偏移距离
                m_dHatchEndOffset = 0;//填充线结束偏移距离

                m_bEnableAutoRotate = 0;//使能自动旋转
                m_dHatchRotateAngle = 0;//旋转角度

                m_dHatchLineReduction = 0;//直线缩进
                m_dHatchLoopDist = 0;//环间距
                m_nEdgeLoop = 0;//边界环数
            }

        };
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetHatchParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetHatchParam(E3_POINTER idEnt, ref bool bEnableContour, ref bool bContourFirst, ref HatchParam param1, ref HatchParam param2, ref HatchParam param3);
        public static E3_ERR E3_GetHatchParam(E3_ID idEnt, ref bool bEnableContour, ref bool bContourFirst, ref HatchParam param1, ref HatchParam param2, ref HatchParam param3)
        {
            return e3_GetHatchParam(idEnt, ref bEnableContour, ref bContourFirst, ref param1, ref param2, ref param3);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetHatchParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetHatchParam(E3_POINTER idEM, E3_POINTER idEnt, bool bEnableContour, bool bContourFirst, HatchParam param1, HatchParam param2, HatchParam param3);
        public static E3_ERR E3_SetHatchParam(E3_ID idEM, E3_ID idEnt, bool bEnableContour, bool bContourFirst, HatchParam param1, HatchParam param2, HatchParam param3)
        {
            return e3_SetHatchParam(idEM, idEnt, bEnableContour, bContourFirst, param1, param2, param3);
        }
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CreatePoints", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CreatePoints(E3_POINTER idEM, int nPen, Pt2d[] ptLines, int nPointCount, bool bPick, bool bUndo, string strUndoName);
        public static E3_ERR E3_CreatePoints(E3_ID idEM, int nPen, Pt2d[] ptLines, int nPointCount, bool bPick, bool bUndo, string strUndoName)
        {
            return e3_CreatePoints(idEM, nPen, ptLines, nPointCount, bPick, bUndo, strUndoName);
        }
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetEntHatchParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetEntHatchParam(E3_POINTER idEnt, ref bool bEnableContour, ref bool bContourFirst, ref HatchParam param1, ref HatchParam param2, ref HatchParam param3);
        public static E3_ERR E3_GetEntHatchParam(E3_ID idEnt, ref bool bEnableContour, ref bool bContourFirst, ref HatchParam param1, ref HatchParam param2, ref HatchParam param3)
        {
            return e3_GetEntHatchParam(idEnt, ref bEnableContour, ref bContourFirst, ref param1, ref param2, ref param3);
        }



        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetEntHatchParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetEntHatchParam(E3_POINTER idEM, E3_POINTER idEnt, bool bEnableContour, bool bContourFirst, HatchParam param1, HatchParam param2, HatchParam param3, bool bUndo, string strUndo);
        public static E3_ERR E3_SetEntHatchParam(E3_ID idEM, E3_ID idEnt, bool bEnableContour, bool bContourFirst, HatchParam param1, HatchParam param2, HatchParam param3, bool bUndo, string strUndo)
        {
            return e3_SetEntHatchParam(idEM, idEnt, bEnableContour, bContourFirst, param1, param2, param3, bUndo, strUndo);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_DelEntHatchParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_DelEntHatchParam(E3_POINTER idEM, E3_POINTER idEnt, bool bUndo, string strUndo);
        public static E3_ERR E3_DelEntHatchParam(E3_ID idEM, E3_ID idEnt, bool bUndo, string strUndo)
        {
            return e3_DelEntHatchParam(idEM, idEnt, bUndo, strUndo);
        }


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_HatchEnt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_HatchEnt(E3_POINTER idEnt, bool bEnableContour, bool bContourFirst, HatchParam param1, HatchParam param2, HatchParam param3);
        public static E3_ERR E3_HatchEnt(E3_ID idEnt, bool bEnableContour, bool bContourFirst, HatchParam param1, HatchParam param2, HatchParam param3)
        {
            return e3_HatchEnt(idEnt, bEnableContour, bContourFirst, param1, param2, param3);
        }

        //int E3_HatchEnt2(E3_ID idEnt, BOOL bEnableContour, BOOL bContourFirst, int nHatchParamCount, HatchParam* pParam, E3_ID& idEntHatch)
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_HatchEnt2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_HatchEnt2(E3_POINTER idEnt, bool bEnableContour, bool bContourFirst, int nHatchParamCount, HatchParam[] paramList, ref E3_POINTER idHatchEnt);
        public static E3_ERR E3_HatchEnt2(E3_ID idEnt, bool bEnableContour, bool bContourFirst, int nHatchParamCount, HatchParam[] paramList, ref E3_ID idHatchEnt)
        {
            E3_POINTER id = 0;
            E3_ERR err = e3_HatchEnt2(idEnt, bEnableContour, bContourFirst, nHatchParamCount, paramList, ref id);
            idHatchEnt = id;
            return err;
        }


        /// <summary>
        /// 背景填充功能（需要开通背景填充功能）
        /// </summary>
        /// <param name="idEnt"></param>
        /// <param name="idEntBack"></param>
        /// <param name="bEnableContour"></param>
        /// <param name="bContourFist"></param>
        /// <param name="bDeleteOldEnt"></param>
        /// <param name="idNewEnt"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_HatchEntByBack", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_HatchEntByBack(E3_POINTER idEnt, E3_POINTER idEntBack, bool bEnableContour, bool bContourFist, bool bDeleteOldEnt, ref E3_POINTER idNewEnt);
        public static E3_ERR E3_HatchEntByBack(E3_ID idEnt, E3_ID idEntBack, bool bEnableContour, bool bContourFist, bool bDeleteOldEnt, ref E3_ID idNewEnt)
        {
            E3_POINTER id = idEntBack;
            E3_ERR err = e3_HatchEntByBack(idEnt, idEntBack, bEnableContour, bContourFist, bDeleteOldEnt, ref id);
            idNewEnt = id;
            return err;
        }




        #endregion


        #region Marker
        //得到第一个有效的Marker的ID
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetFirstValidId", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_POINTER e3_MarkerGetFirstValidId();
        public static E3_ID E3_MarkerGetFirstValidId()
        {
            return e3_MarkerGetFirstValidId();
        }

        //标刻一条曲线
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerOneCurveToList2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerOneCurveToList2(E3_POINTER idMarker, int type, int pen, int ptNum, int nFlag, [MarshalAs(UnmanagedType.LPArray)]double[,] ptBuf);
        public static E3_ERR E3_MarkerOneCurveToList2(E3_ID idMarker, int type, int pen, int ptNum, int nFlag, [MarshalAs(UnmanagedType.LPArray)]double[,] ptBuf)
        {
            return e3_MarkerOneCurveToList2(idMarker, type, pen, ptNum, nFlag, ptBuf);
        }


        //设置所有对象的先绕旋转中心旋转指定角度，然后平移指定距离
        //输入参数: dMoveX   为x平移的距离E3_MarkerSetRotateMoveParam
        //          dMoveY   为y平移的距离
        //          dCenterX 旋转中心x坐标 
        //          dCenterY 旋转中心y坐标 
        //          dRotateAng 旋转角度(弧度值), 
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerSetRotateMoveParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerSetRotateMoveParam(E3_POINTER idMarker, double dMoveX, double dMoveY, double dCenterX, double dCenterY, double dRotateAng);
        public static E3_ERR E3_MarkerSetRotateMoveParam(E3_ID idMarker, double dMoveX, double dMoveY, double dCenterX, double dCenterY, double dRotateAng)
        {
            return e3_MarkerSetRotateMoveParam(idMarker, dMoveX, dMoveY, dCenterX, dCenterY, dRotateAng);
        }



        /// <summary>
        /// 获取控制卡SN序号
        /// </summary>     
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetMarkerSN", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetMarkerSN(E3_POINTER idMarker, ref int Sn);
        public static E3_ERR E3_GetMarkerSN(E3_ID idMarker, ref int Sn)
        {
            return e3_GetMarkerSN(idMarker, ref Sn);

        }


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerDlgSetCfg", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerDlgSetCfg(E3_POINTER idMarker, ref bool bReturnOk);
        public static E3_ERR E3_MarkerDlgSetCfg(E3_ID idMarker, ref bool bReturnOk)
        {
            return e3_MarkerDlgSetCfg(idMarker, ref bReturnOk);
        }

        /// <summary>
        /// 关闭控制卡
        /// </summary>
        /// <param name="idMarker"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_CloseMarker", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_CloseMarker(E3_POINTER idMarker);
        public static E3_ERR E3_CloseMarker(E3_ID idMarker)
        {
            return e3_CloseMarker(idMarker);
        }

        // E3_API int E3_MarkerSetPause(E3_ID idMarker);
        //E3_API int E3_MarkerRestartList(E3_ID idMarker);
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="idMarker"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerSetPause", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerSetPause(E3_POINTER idMarker);
        public static E3_ERR E3_MarkerSetPause(E3_ID idMarker)
        {
            return e3_MarkerSetPause(idMarker);
        }

        /// <summary>
        /// 暂停继续
        /// </summary>
        /// <param name="idMarker"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerRestartList", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerRestartList(E3_POINTER idMarker);
        public static E3_ERR E3_MarkerRestartList(E3_ID idMarker)
        {
            return e3_MarkerRestartList(idMarker);
        }


        /// <summary>
        /// 停止加工
        /// </summary>
        /// <param name="idMarker"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerStop", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerStop(E3_POINTER idMarker);
        public static E3_ERR E3_MarkerStop(E3_ID idMarker)
        {
            return e3_MarkerStop(idMarker);
        }

        /// <summary>
        /// 标刻指定对象，添加到列表
        /// </summary>
        /// <param name="idMarker"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerMarkEntToList2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkEntToList2(E3_POINTER idMarker, E3_POINTER idEnt);
        public static E3_ERR E3_MarkEntToList2(E3_ID idMarker, E3_ID idEnt)
        {
            return e3_MarkEntToList2(idMarker, idEnt);
        }


        //单独控制激光开关
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerLaserOn", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerLaserOn(E3_POINTER idMarker, int nPenNo, bool bOpen);
        public static E3_ERR E3_MarkerLaserOn(E3_ID idMarker, int nPenNo, bool bOpen)
        {
            return e3_MarkerLaserOn(idMarker, nPenNo, bOpen);
        }



        /// <summary>
        /// 关联控制卡和对象管理器
        /// </summary>
        /// <param name="idMarker"></param>
        /// <param name="idEntMgr"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerSetEntMgr", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerSetEntMgr(E3_POINTER idMarker, E3_POINTER idEntMgr);
        public static E3_ERR E3_MarkerSetEntMgr(E3_ID idMarker, E3_ID idEntMgr)
        {
            return e3_MarkerSetEntMgr(idMarker, idEntMgr);
        }


        //E3_API int E3_MarkerGetRange(E3_ID idMarker, double& x, double& y);

        /// <summary>
        /// 获取校正范围
        /// </summary>
        /// <param name="idMarker"></param>
        /// <param name="RangeX"></param>
        /// <param name="RangeY"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetRange", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetRange(E3_POINTER idMarker, ref double RangeX, ref double RangeY);
        public static E3_ERR E3_MarkerGetRange(E3_ID idMarker, ref double RangeX, ref double RangeY)
        {
            return e3_MarkerGetRange(idMarker, ref RangeX, ref RangeY);
        }
        /// <summary>
        /// 查看控制卡的关联对象管理器
        /// </summary>
        /// <param name="idMarker"></param>
        /// <param name="idEntMgr"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetEntMgr", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetEntMgr(E3_POINTER idMarker, ref E3_POINTER idEntMgr);
        public static E3_ERR E3_MarkerGetEntMgr(E3_ID idMarker, ref E3_ID idEntMgr)
        {
            E3_POINTER id = 0;
            E3_ERR err = e3_MarkerGetEntMgr(idMarker, ref id);
            idEntMgr = id;
            return err;
        }
        //


        //跳转到指定坐标
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerJumpTo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerJumpTo(E3_POINTER idMarker, double x, double y, double z, double spd);
        public static E3_ERR E3_MarkerJumpTo(E3_ID idMarker, double x, double y, double z, double spd)
        {
            return e3_MarkerJumpTo(idMarker, x, y, z, spd);
        }




        public const int MAKRFLAG_REDLIGHT = 0x000000002;  //Red light indication 

        public const int MAKRFLAG_FLYMODE = 0x000000400;  //Flight marking      

        public const int MAKRFLAG_CALCTIMEMODE = 0x01000000;  //Calculation Mode

        /// <summary>
        /// 获取上次计算的加工时间
        /// </summary>
        /// <param name="idMarker"></param>
        /// <param name="TimeMs"></param>
        /// <returns></returns>
        // E3_API int E3_MarkerGetCalcMarkTime(E3_ID idMarker, int& nTimeMs);
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetCalcMarkTime", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetCalcMarkTime(E3_POINTER idMarker, ref int TimeMs);
        public static E3_ERR E3_MarkerGetCalcMarkTime(E3_ID idMarker, ref int TimeMs)
        {
            return e3_MarkerGetCalcMarkTime(idMarker, ref TimeMs);
        }


        /// <summary>
        /// 获取上次实际加工时间
        /// </summary>
        /// <param name="idMarker"></param>
        /// <param name="nTimeMs"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetMakingTime", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetMakingTime(E3_POINTER idMarker, ref int nTimeMs);
        public static E3_ERR E3_MarkerGetMakingTime(E3_ID idMarker, ref int nTimeMs)
        {
            return e3_MarkerGetMakingTime(idMarker, ref nTimeMs);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerListReady", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerListReady(E3_POINTER idMarker, E3_POINTER idEM, int nFlag);
        public static E3_ERR E3_MarkerListReady(E3_ID idMarker, E3_ID idEm, int nFlag)
        {
            return e3_MarkerListReady(idMarker, idEm, nFlag);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerListEnd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerListEnd(E3_POINTER idMarker);
        public static E3_ERR E3_MarkerListEnd(E3_ID idMarker)
        {
            return e3_MarkerListEnd(idMarker);
        }

        //等待标刻结束
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerWaitForFinish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerWaitForFinish(E3_POINTER idMarker);
        public static E3_ERR E3_MarkerWaitForFinish(E3_ID idMarker)
        {
            return e3_MarkerWaitForFinish(idMarker);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerOneCurveToList", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerCurveToList(E3_POINTER idMarker, int type, int pen, int ptNum, int nFlag, double[,] ptBuf);
        public static E3_ERR E3_MarkerCurveToList(E3_ID idMarker, int type, int pen, int ptNum, int nFlag, double[,] ptBuf)
        {
            return e3_MarkerCurveToList(idMarker, type, pen, ptNum, nFlag, ptBuf);
        }

        // int E3_TransormEnt3dRotate(E3_ID idEnt, double dMoveX, double dMoveY, double dMoveZ, double ptAxisX, double ptAxisY, double ptAxisZ, double fRadAngle, double dTol, E3_ID& idNewEnt)

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_TransormEnt3dRotate", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_TransormEnt3dRotate(E3_POINTER idEnt, double dMoveX, double dMoveY, double dMoveZ, double ptAxisX, double ptAxisY, double ptAxisZ, double fRadAngle, double dTol, ref E3_POINTER idNewEnt);
        public static E3_ERR E3_TransormEnt3dRotate(E3_ID idEnt, double dMoveX, double dMoveY, double dMoveZ, double ptAxisX, double ptAxisY, double ptAxisZ, double fRadAngle, double dTol, ref E3_ID idNewEnt)
        {

            E3_POINTER id = idEnt;
            E3_ERR err = e3_TransormEnt3dRotate(idEnt, dMoveX, dMoveY, dMoveZ, ptAxisX, ptAxisY, ptAxisZ, fRadAngle, dTol, ref id);
            idNewEnt = id;
            return err;
        }

        


        //使能列表飞行模式
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerFlyEnableToList", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerFlyEnableToList(E3_POINTER idMarker, bool bEnable);
        public static E3_ERR E3_MarkerFlyEnableToList(E3_ID idMarker, bool bEnable)
        {
            return e3_MarkerFlyEnableToList(idMarker, bEnable);
        }



        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerMarkEnt2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkEnt2(E3_POINTER idMarker, E3_POINTER idEM, E3_POINTER idEnt, int nMarkFlag, int nStartPartNum, int nMaxPartCount, ref int nErr);
        public static E3_ERR E3_MarkEnt2(E3_ID idMarker, E3_ID idEM, E3_ID idEnt, int nMarkFlag, int nStartPartNum, int nMaxPartCount, ref int nErr)
        {
            return e3_MarkEnt2(idMarker, idEM, idEnt, nMarkFlag, nStartPartNum, nMaxPartCount, ref nErr);
        }


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerMarkEntToList2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerMarkEntToList2(E3_POINTER idMarker, E3_POINTER idEnt);
        public static E3_ERR E3_MarkerMarkEntToList2(E3_ID idMarker, E3_ID idEnt)
        {
            return e3_MarkerMarkEntToList2(idMarker, idEnt);
        }



        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerReadPort", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerReadPort(E3_POINTER idMarker, ref ushort data);
        public static E3_ERR E3_MarkerReadPort(E3_ID idMarker, ref ushort data)
        {
            return e3_MarkerReadPort(idMarker, ref data);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetWritePort", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetWritePort(E3_POINTER idMarker, ref ushort data);
        public static E3_ERR E3_MarkerGetWritePort(E3_ID idMarker, ref ushort data)
        {
            return e3_MarkerGetWritePort(idMarker, ref data);
        }

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerWritePort", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerWritePort(E3_POINTER idMarker, ushort data);
        public static E3_ERR E3_MarkerWritePort(E3_ID idMarker, ushort data)
        {
            return e3_MarkerWritePort(idMarker, data);
        }
        #endregion

        /// <summary>
        /// 连续模式加工
        /// </summary>
        /// <param name="idMarker"></param>
        /// <param name="nNum"></param>
        /// <param name="pStr1"></param>
        /// <param name="pStr2"></param>
        /// <param name="pStr3"></param>
        /// <param name="pStr4"></param>
        /// <param name="pStr5"></param>
        /// <param name="pStr6"></param>
        /// <returns></returns>

        //nNum=后面有几个字符串有效
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerContinueBufferSetTextName", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerContinueBufferSetTextName(E3_POINTER idMarker, int nNum, string pStr1, string pStr2, string pStr3, string pStr4, string pStr5, string pStr6);
        public static E3_ERR E3_MarkerContinueBufferSetTextName(E3_ID idMarker, int nNum, string pStr1, string pStr2, string pStr3, string pStr4, string pStr5, string pStr6)
        {
            return e3_MarkerContinueBufferSetTextName(idMarker, nNum, pStr1, pStr2, pStr3, pStr4, pStr5, pStr6);
        }
        //造形
        //nMode=0 焊接对象 求idEnt1和idEnt2焊接区域
        //nMode=1 修剪对象 idEnt1为边界去修剪idEnt2
        //nMode=2 交叉对象 求idEnt1和idEnt2的交叉区域
        //E3_API int E3_DistortionEntity(E3_ID idEM, E3_ID idEnt1, E3_ID idEnt2, int nMode, int nPen, BOOL bUndo, TCHAR* strUndoName);
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_DistortionEntity", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern int e3_DistortionEntity(E3_POINTER idEM, E3_POINTER idEnt1, E3_POINTER idEnt2, int nMode, int nPen, bool bUndo, string strUndoName);
        public static int E3_DistortionEntity(E3_ID idEM, E3_ID idEnt1, E3_ID idEnt2, int nMode, int nPen, bool bUndo, string strUndoName)
        {
            return e3_DistortionEntity(idEM, idEnt1, idEnt2, nMode, nPen, bUndo, strUndoName);
        }
        //nBufferLeftCount 当前缓存区剩余中未开始处理的个数（有可能数据已经处理，当时还未开始加工）,缓存区数据数最大为8
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerContinueBufferGetParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerContinueBufferGetParam(E3_POINTER idMarker, ref int nTotalMarkCount, ref int nBufferLeftCount);
        public static E3_ERR E3_MarkerContinueBufferGetParam(E3_ID idMarker, ref int nTotalMarkCount, ref int nBufferLeftCount)
        {
            return e3_MarkerContinueBufferGetParam(idMarker, ref nTotalMarkCount, ref nBufferLeftCount);
        }
        //增加缓存数据，
        //pStr1=TEXT1对应的字符串
        //pStr2=TEXT2对应的字符串
        //。。。。
        //pStr6=TEXT6对应的字符串
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerContinueBufferAdd", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerContinueBufferAdd(E3_POINTER idMarker, string pStr1, string pStr2, string pStr3, string pStr4, string pStr5, string pStr6);
        public static E3_ERR E3_MarkerContinueBufferAdd(E3_ID idMarker, string pStr1, string pStr2, string pStr3, string pStr4, string pStr5, string pStr6)
        {
            return e3_MarkerContinueBufferAdd(idMarker, pStr1, pStr2, pStr3, pStr4, pStr5, pStr6);
        }
        //设置发送零件数已经结束的标志
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerContinueBufferPartFinish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerContinueBufferPartFinish(E3_POINTER idMarker);
        public static E3_ERR E3_MarkerContinueBufferPartFinish(E3_ID idMarker)
        {
            return e3_MarkerContinueBufferPartFinish(idMarker);
        }
        //开始缓存区加工，如何缓存区没数据会一直等待数据, 
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerContinueBufferStart", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerContinueBufferStart(E3_POINTER idMarker, E3_POINTER idEM, E3_POINTER idEntParent);
        public static E3_ERR E3_MarkerContinueBufferStart(E3_ID idMarker, E3_ID idEM, E3_ID idEntParent)
        {
            return e3_MarkerContinueBufferStart(idMarker, idEM, idEntParent);
        }

        //清除缓存区
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerContinueBufferClear", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerContinueBufferClear(E3_POINTER idMarker);
        public static E3_ERR E3_MarkerContinueBufferClear(E3_ID idMarker)
        {
            return e3_MarkerContinueBufferClear(idMarker);
        }

        #region Axis


        //初始化所有扩展轴
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_InitAllAxis", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_InitAllAxis(E3_POINTER idMarker);
        public static E3_ERR E3_InitAllAxis(E3_ID idMarker)
        {
            return e3_InitAllAxis(idMarker);
        }


        //扩展轴移动到指定坐标位置 axis=0，1，2，3
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_AxisMoveTo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_AxisMoveTo(E3_POINTER idMarker, int axis, double dGoalCoor, bool bWaitForMoveFinish);
        public static E3_ERR E3_AxisMoveTo(E3_ID idMarker, int axis, double dGoalCoor, bool bWaitForMoveFinish)
        {
            return e3_AxisMoveTo(idMarker, axis, dGoalCoor, bWaitForMoveFinish);
        }


        //扩展轴回零
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_AxisHome", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_AxisHome(E3_POINTER idMarker, int axis, bool bWaitForMoveFinish);
        public static E3_ERR E3_AxisHome(E3_ID idMarker, int axis, bool bWaitForMoveFinish)
        {
            return e3_AxisHome(idMarker, axis, bWaitForMoveFinish);
        }

        //得到扩展轴的当前坐标 
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetAxisCoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetAxisCoor(E3_POINTER idMarker, int axis, ref double dCoor);
        public static E3_ERR E3_GetAxisCoor(E3_ID idMarker, int axis, ref double dCoor)
        {
            return e3_GetAxisCoor(idMarker, axis, ref dCoor);
        }
        /// <summary>
        /// 新加
        /// </summary>
        /// <param name="idMarker"></param>
        /// <param name="axis"></param>
        /// <param name="dCoor"></param>
        /// <returns></returns>
        //获取指定轴运动参数
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetAxisParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetAxisParam(E3_POINTER idMarker,
                    int axis,
                    ref bool bEnable,
                   ref bool bInvert,
                    ref bool bOutNeg,//1=共阴极输出	
                    ref bool bRotaryAxis,
                     ref double dDistPerRound,//转一圈的距离
                     ref double dPulsePerRound,//电机一转脉冲数
                     ref double dMinVel,//最小速度
                     ref double dMaxVel,//最大速度 
                     ref double dAcceleration,
                     ref double dDeceleration,
                     ref double dBacklash,//反向间隙
                   ref bool bEnFeedback,//是否使能编码器
                     ref double dPosErrFollow,//跟随误差	
                   ref bool bSAcc,
                     ref double dJerk,
                   ref bool bEnableSoftLimit,
                     ref double dMinSoftLimit,
                    ref double dMaxSoftLimit,
                 ref bool bEnableHome,
                  ref bool bHomeLowValid,
                  ref bool bHomeDirPos,//正向回零
                    ref bool bHomeFindIndex,
                    ref double dHomePos,//零点的坐标
                     ref double dHomingFindVel,//找零点速度
                   ref double dHomingLeaveVel,//离开零点速度
                    ref bool bHomeFinishGotoPos,
                     ref double dHomeFinishGotoPos,
                    ref bool bEnableLimit,
                    ref bool bLimitLowValid,
                    ref bool bMarkFinishGotoStartPoint
);
        public static E3_ERR E3_GetAxisParam(E3_ID idMarker,
                    int axis,
                    ref bool bEnable,
                   ref bool bInvert,
                    ref bool bOutNeg,//1=共阴极输出	
                    ref bool bRotaryAxis,
                     ref double dDistPerRound,//转一圈的距离
                     ref double dPulsePerRound,//电机一转脉冲数
                     ref double dMinVel,//最小速度
                     ref double dMaxVel,//最大速度 
                     ref double dAcceleration,
                     ref double dDeceleration,
                     ref double dBacklash,//反向间隙
                   ref bool bEnFeedback,//是否使能编码器
                     ref double dPosErrFollow,//跟随误差	
                   ref bool bSAcc,
                     ref double dJerk,
                   ref bool bEnableSoftLimit,
                     ref double dMinSoftLimit,
                    ref double dMaxSoftLimit,
                 ref bool bEnableHome,
                  ref bool bHomeLowValid,
                  ref bool bHomeDirPos,//正向回零
                    ref bool bHomeFindIndex,
                    ref double dHomePos,//零点的坐标
                     ref double dHomingFindVel,//找零点速度
                   ref double dHomingLeaveVel,//离开零点速度
                    ref bool bHomeFinishGotoPos,
                     ref double dHomeFinishGotoPos,
                    ref bool bEnableLimit,
                    ref bool bLimitLowValid,
                    ref bool bMarkFinishGotoStartPoint)
        {
            return e3_GetAxisParam(idMarker,
                    axis,
                    ref bEnable,
                   ref bInvert,
                    ref bOutNeg,//1=共阴极输出	
                    ref bRotaryAxis,
                     ref dDistPerRound,//转一圈的距离
                     ref dPulsePerRound,//电机一转脉冲数
                     ref dMinVel,//最小速度
                     ref dMaxVel,//最大速度 
                     ref dAcceleration,
                     ref dDeceleration,
                     ref dBacklash,//反向间隙
                   ref bEnFeedback,//是否使能编码器
                     ref dPosErrFollow,//跟随误差	
                   ref bSAcc,
                     ref dJerk,
                   ref bEnableSoftLimit,
                     ref dMinSoftLimit,
                    ref dMaxSoftLimit,
                 ref bEnableHome,
                  ref bHomeLowValid,
                  ref bHomeDirPos,//正向回零
                    ref bHomeFindIndex,
                    ref dHomePos,//零点的坐标
                     ref dHomingFindVel,//找零点速度
                   ref dHomingLeaveVel,//离开零点速度
                    ref bHomeFinishGotoPos,
                     ref dHomeFinishGotoPos,
                    ref bEnableLimit,
                    ref bLimitLowValid,
                    ref bMarkFinishGotoStartPoint);
        }

        //获取指定轴运动参数
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetAxisParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetAxisParam(E3_POINTER idMarker,
                    int axis,
                     bool bEnable,
                    bool bInvert,
                     bool bOutNeg,//1=共阴极输出	
                     bool bRotaryAxis,
                      double dDistPerRound,//转一圈的距离
                      double dPulsePerRound,//电机一转脉冲数
                      double dMinVel,//最小速度
                      double dMaxVel,//最大速度 
                     double dAcceleration,
                      double dDeceleration,
                      double dBacklash,//反向间隙
                    bool bEnFeedback,//是否使能编码器
                      double dPosErrFollow,//跟随误差	
                    bool bSAcc,
                      double dJerk,
                    bool bEnableSoftLimit,
                    double dMinSoftLimit,
                    double dMaxSoftLimit,
                bool bEnableHome,
                  bool bHomeLowValid,
                  bool bHomeDirPos,//正向回零
                     bool bHomeFindIndex,
                  double dHomePos,//零点的坐标
                      double dHomingFindVel,//找零点速度
                    double dHomingLeaveVel,//离开零点速度
                    bool bHomeFinishGotoPos,
                     double dHomeFinishGotoPos,
                     bool bEnableLimit,
                    bool bLimitLowValid,
                    bool bMarkFinishGotoStartPoint
);
        public static E3_ERR E3_SetAxisParam(E3_ID idMarker,
                    int axis,
                     bool bEnable,
                  bool bInvert,
                     bool bOutNeg,//1=共阴极输出	
                    bool bRotaryAxis,
                      double dDistPerRound,//转一圈的距离
                     double dPulsePerRound,//电机一转脉冲数
                    double dMinVel,//最小速度
                      double dMaxVel,//最大速度 
                     double dAcceleration,
                     double dDeceleration,
                     double dBacklash,//反向间隙
                   bool bEnFeedback,//是否使能编码器
                      double dPosErrFollow,//跟随误差	
                    bool bSAcc,
                      double dJerk,
                  bool bEnableSoftLimit,
                     double dMinSoftLimit,
                    double dMaxSoftLimit,
                  bool bEnableHome,
                   bool bHomeLowValid,
                  bool bHomeDirPos,//正向回零
                    bool bHomeFindIndex,
                    double dHomePos,//零点的坐标
                    double dHomingFindVel,//找零点速度
                   double dHomingLeaveVel,//离开零点速度
                     bool bHomeFinishGotoPos,
                    double dHomeFinishGotoPos,
                     bool bEnableLimit,
                   bool bLimitLowValid,
                    bool bMarkFinishGotoStartPoint)
        {
            return e3_SetAxisParam(idMarker,
                    axis,
                    bEnable,
                    bInvert,
                     bOutNeg,//1=共阴极输出	
                     bRotaryAxis,
                      dDistPerRound,//转一圈的距离
                      dPulsePerRound,//电机一转脉冲数
                      dMinVel,//最小速度
                      dMaxVel,//最大速度 
                      dAcceleration,
                      dDeceleration,
                      dBacklash,//反向间隙
                    bEnFeedback,//是否使能编码器
                      dPosErrFollow,//跟随误差	
                    bSAcc,
                      dJerk,
                    bEnableSoftLimit,
                      dMinSoftLimit,
                     dMaxSoftLimit,
                  bEnableHome,
                   bHomeLowValid,
                   bHomeDirPos,//正向回零
                     bHomeFindIndex,
                     dHomePos,//零点的坐标
                      dHomingFindVel,//找零点速度
                    dHomingLeaveVel,//离开零点速度
                     bHomeFinishGotoPos,
                      dHomeFinishGotoPos,
                     bEnableLimit,
                     bLimitLowValid,
                     bMarkFinishGotoStartPoint);
        }

        //设置轴点位运动参数
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetAxisSpeedParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SetAxisSpeedParam(E3_POINTER idMarker, int axis, double dStartSpeed, double dMaxSpeed, double dAccSpeed);
        public static E3_ERR E3_SetAxisSpeedParam(E3_ID idMarker, int axis, double dStartSpeed, double dMaxSpeed, double dAccSpeed)
        {
            return e3_SetAxisSpeedParam(idMarker, axis, dStartSpeed, dMaxSpeed, dAccSpeed);
        }


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetAxisFBCoor", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetAxisFBCoor(E3_POINTER idMarker, int axis, ref double dCoor);
        public static E3_ERR E3_GetAxisFBCoor(E3_ID idMarker, int axis, ref double dCoor)
        {
            return e3_GetAxisFBCoor(idMarker, axis, ref dCoor);
        }

        //判断扩展轴正在运动
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_IsAxisMoving", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_IsAxisMoving(E3_POINTER idMarker, int axis, ref bool bIsMoving);
        public static E3_ERR E3_IsAxisMoving(E3_ID idMarker, int axis, ref bool bIsMoving)
        {
            return e3_IsAxisMoving(idMarker, axis, ref bIsMoving);
        }


        //得到扩展轴是否有原点信号(bIsHaveHome),是否已经找过原点（bIsAlreadyFindHome）
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_IsAxisHaveHome", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_IsAxisHaveHome(E3_POINTER idMarker, int axis, ref bool bIsHaveHome, ref bool bIsAlreadyFindHome);
        public static E3_ERR E3_IsAxisHaveHome(E3_ID idMarker, int axis, ref bool bIsHaveHome, ref bool bIsAlreadyFindHome)
        {
            return e3_IsAxisHaveHome(idMarker, axis, ref bIsHaveHome, ref bIsAlreadyFindHome);
        }

        //得到扩展轴当前是否有故障
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_IsAxisHaveFault", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_IsAxisHaveFault(E3_POINTER idMarker, int axis, ref bool bIsFault);
        public static E3_ERR E3_IsAxisHaveFault(E3_ID idMarker, int axis, ref bool bIsFault)
        {
            return e3_IsAxisHaveFault(idMarker, axis, ref bIsFault);
        }

        public const UInt32 FONT_ATB_TYPE_JSF = 0x0001;       //JczSingle字型
        public const UInt32 FONT_ATB_TYPE_TTF = 0x0002;       //TrueType字型   
        public const UInt32 FONT_ATB_TYPE_DMF = 0x0004;       //DotMatrix字型   
        public const UInt32 FONT_ATB_TYPE_BCF = 0x0008;       //BarCode字型   
        public const UInt32 FONT_ATB_TYPE_SHX = 0x0010;       //shx字型   

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetAllFontCount", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR E3_GetAllFontCount(ref int nEntCount);

        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetAllFontRecord", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR E3_GetFontRecord(int id, StringBuilder strName, ref UInt32 attrib);

        #endregion

        #region STL 文件分层 显示
        /// <summary>
        /// 分割STL文件
        /// </summary>
        /// <param name="idLayer"></param>
        /// <param name="strStlFile"></param>
        /// <param name="strName"></param>
        /// <param name="bZUpToDown"></param>
        /// <param name="dMinZ"></param>
        /// <param name="dMaxZ"></param>
        /// <param name="dThickness"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SliceStlFile", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SliceStlFile(E3_POINTER idLayer, string strStlFile, string strName, bool bZUpToDown, double dMinZ, double dMaxZ, double dThickness);

        public static E3_ERR E3_SliceStlFile(E3_ID idLayer, string strStlFile, string strName, bool bZUpToDown, double dMinZ, double dMaxZ, double dThickness)
        {
            return e3_SliceStlFile(idLayer, strStlFile, strName, bZUpToDown, dMinZ, dMaxZ, dThickness);
        }


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetStlFileSize", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetStlFileSize(E3_POINTER idEM, string strStlFile, ref double minx, ref double miny, ref double maxx, ref double maxy, ref double minz, ref double maxz);


      
       


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_GetEntInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_GetEntInfo(E3_POINTER idEnt, int nFlag, ref int np1, ref int np2, ref int np3, ref int np4, ref int np5, ref int np6,
    ref double dp1, ref double dp2, ref double dp3, ref double dp4, ref double dp5, ref double dp6, StringBuilder str1, byte[] pParam1);






        /// <summary>
        /// 获取STL文件尺寸数据
        /// </summary>
        /// <param name="idEM"></param>
        /// <param name="strStlFile"></param>
        /// <param name="minx"></param>
        /// <param name="miny"></param>
        /// <param name="maxx"></param>
        /// <param name="maxy"></param>
        /// <param name="minz"></param>
        /// <param name="maxz"></param>
        /// <returns></returns>
        public static E3_ERR E3_GetStlFileSize(E3_ID idEM, string strStlFile, ref double minx, ref double miny, ref double maxx, ref double maxy, ref double minz, ref double maxz)
        {
            return e3_GetStlFileSize(idEM, strStlFile, ref minx, ref miny, ref maxx, ref maxy, ref minz, ref maxz);
        }

        /// <summary>
        /// 获取位图对象属性
        /// </summary>
        /// <param name="idEnt">位图对象ID</param>
        /// <param name="strFileName">位图对象文件路径</param>
        /// <param name="bBidir">双向扫描</param>
        /// <param name="dBiDirOffset">双向扫描错位补偿</param>
        /// <param name="bDrillmode">打点模式true false</param>
        /// <param name="dDrillTime">打点延时</param>
        /// <param name="bScanY">Y向扫描true false</param>
        /// <param name="bPixelPower">调整点功率true false</param>
        /// <param name="bFixedDPI">使能固定DPItrue false</param>
        /// <param name="nDpiX">DPIX</param>
        /// <param name="nDpiY">DPIY</param>
        /// <param name="bDynFile">使能动态文件</param>
        /// <param name="bFixedX">使能固定X尺寸true false</param>
        /// <param name="bFixedY">使能固定Y尺寸true false</param>
        /// <param name="dSizeX">固定X尺寸</param>
        /// <param name="dSizeY">固定Y尺寸</param>
        /// <param name="nFixedPosition">动态文件的参考坐标0~8</param>
        /// <param name="bGray">使能灰度处理true false</param>
        /// <param name="bInvert">使能反转处理true false</param>
        /// <param name="bDither">使能网点处理true false</param>
        /// <param name="bOptimize">使能优化模式（无效）true false</param>
        /// <param name="bLineOffset">行错位（无效）</param>
        /// <param name="bLineIncrement">隔行扫描true false</param>
        /// <param name="nLineIncrement">隔行扫描行数 0~n</param>
        /// <param name="nMinLowGrayPt">灰度阈值(0~255)</param>
        /// <param name="bDisableMarkLowGrayPt">使能低于灰度阈值不加工</param>
        /// <param name="dAccDist">加速距离（无效）</param>
        /// <param name="dDecDist">减速距离（无效）</param>
        /// <param name="nGrayCurvePt">灰度功率曲线节点数量（无效）</param>
        /// <param name="ptGrayCurveBuf">灰度功率曲线列表(无效)</param>
        /// <param name="bGrayScaleBuf">灰度功率表（256长度，POW[i]  i=gray  pow[i]=pow*2.56）</param>
        /// <returns></returns>
        public static E3_ERR E3_GetEntBmpFileInfo(E3_ID idEnt, ref string strFileName, ref bool bBidir, ref double dBiDirOffset, ref bool bDrillmode, ref double dDrillTime, ref bool bScanY, ref bool bPixelPower, ref bool bFixedDPI, ref int nDpiX, ref int nDpiY,
                                                   ref bool bDynFile, ref bool bFixedX, ref bool bFixedY, ref double dSizeX, ref double dSizeY, ref int nFixedPosition, ref bool bGray, ref bool bInvert, ref bool bDither, ref bool bLight, ref bool bOptimize, ref bool bLineOffset, ref bool bLineIncrement, ref int nLineIncrement,
                                                   ref int nMinLowGrayPt, ref bool bDisableMarkLowGrayPt, ref double dAccDist, ref double dDecDist, ref int nGrayCurvePt, Point[] ptGrayCurveBuf, byte[] bGrayScaleBuf)
        {
            int np1 = 0;
            int np2 = 0;
            int np3 = 0;
            int np4 = 0;
            int np5 = 0;
            int np6 = 0;
            double dp1 = 0;
            double dp2 = 0;
            double dp3 = 0;
            double dp4 = 0;
            double dp5 = 0;
            double dp6 = 0;
            StringBuilder strFile = new StringBuilder("", 256);

            E3_ERR err = e3_GetEntInfo(idEnt, 0, ref np1, ref np2, ref np3, ref np4, ref np5, ref np6, ref dp1, ref dp2, ref dp3, ref dp4, ref dp5, ref dp6, strFile, bGrayScaleBuf);
            bDynFile = (np1 & BMPATTRIB_DYNFILE) != 0 ? true : false;
            bFixedX = (np1 & BMPATTRIB_IMPORTFIXED_WIDTH) != 0 ? true : false;
            bFixedY = (np1 & BMPATTRIB_IMPORTFIXED_HEIGHT) != 0 ? true : false;
            bFixedDPI = (np1 & BMPATTRIB_IMPORTFIXED_DPI) != 0 ? true : false;

            bInvert = (np3 & BMPSCAN_INVERT) != 0 ? true : false;
            bGray = (np3 & BMPSCAN_GRAY) != 0 ? true : false;
            bDither = (np3 & BMPSCAN_DITHER) != 0 ? true : false;
            bBidir = (np3 & BMPSCAN_BIDIR) != 0 ? true : false;//
            bDrillmode = (np3 & BMPSCAN_DRILL) != 0 ? true : false;
            bPixelPower = (np3 & BMPSCAN_POWER) != 0 ? true : false;
            bLight = (np3 & BMPSCAN_LIGHT) != 0 ? true : false;
            bOptimize = (np3 & BMPSCAN_OPTIMIZE) != 0 ? true : false;
            bScanY = (np3 & BMPSCAN_YDIR) != 0 ? true : false;
            bLineOffset = (np3 & BMPSCAN_OFFSETPT) != 0 ? true : false;

            nFixedPosition = np2;
            nDpiX = np4;
            nDpiY = np5;

            dSizeX = dp1;
            dSizeY = dp2;
            dDrillTime = dp4;
            strFileName = strFile.ToString();

            if (err != E3_ERR.SUCCESS)
            {
                return err;
            }

            byte[] byteGrayCurveBuf = new byte[8 * MAX_GRAYCURVEPT_NUM];

            err = e3_GetEntInfo(idEnt, 1, ref np1, ref np2, ref np3, ref np4, ref np5, ref np6, ref dp1, ref dp2, ref dp3, ref dp4, ref dp5, ref dp6, strFile, byteGrayCurveBuf);
            bLineIncrement = np1 != 0 ? true : false;
            nLineIncrement = np2;
            bDisableMarkLowGrayPt = np3 != 0 ? true : false;
            nMinLowGrayPt = np4;
            nGrayCurvePt = np5;

            dBiDirOffset = dp1;
            dAccDist = dp2;
            dDecDist = dp3;

            return err;
        }


        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetBmpFileInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR E3_SetBmpFileInfo(E3_POINTER idEM, E3_POINTER idEnt, int[] nParamBuf, double[] dParamBuf, byte[] bGrayScaleBuf, string str1, bool bUndo, string strUndo);


        /// <summary>
        /// 设置位图对象参数
        /// </summary>
        /// <param name="idEM">对象管理器</param>
        /// <param name="idEnt">对象ID</param>
        /// <param name="strFileName">位图文件</param>
        /// <param name="bBidir">双向扫描</param>
        /// <param name="dBiDirOffset">双向扫描补偿</param>
        /// <param name="bDrillmode">打点模式</param>
        /// <param name="dDrillTime">打点模式单点最大时间</param>
        /// <param name="bScanY">Y向扫描</param>
        /// <param name="bPixelPower">调整点功率</param>
        /// <param name="bFixedDPI">固定DPI</param>
        /// <param name="nDpiX">XDPI</param>
        /// <param name="nDpiY">YDPI</param>
        /// <param name="bDynFile">动态文件</param>
        /// <param name="bFixedX">固定X尺寸</param>
        /// <param name="bFixedY">固定Y尺寸</param>
        /// <param name="dSizeX">X尺寸</param>
        /// <param name="dSizeY">Y尺寸</param>
        /// <param name="nFixedPosition">固定参考点序号</param>
        /// <param name="bGray">灰度处理</param>
        /// <param name="bInvert">反转处理</param>
        /// <param name="bDither">网点处理</param>
        /// <param name="bOptimize">优化模式</param>
        /// <param name="bLineOffset">隔行错位</param>
        /// <param name="bLineIncrement">位图扫描增量模式1TRUE 0FALSE</param>
        /// <param name="nLineIncrement">位图扫描增量值</param>
        /// <param name="nMinLowGrayPt">阈值灰度</param>
        /// <param name="bDisableMarkLowGrayPt">低于阈值灰度数据不加工</param>
        /// <param name="dAccDist">加速距离</param>
        /// <param name="dDecDist">减速距离</param>
        /// <param name="nGrayCurvePt">灰度功率曲线节点数</param>
        /// <param name="ptGrayCurveBuf">灰度功率曲线列表</param>
        /// <param name="bGrayScaleBuf">灰度功率表</param>
        /// <param name="bUndo">是否支持复位 false</param>
        /// <param name="strUndo">操作名称 null</param>
        /// <returns></returns>
        public static E3_ERR E3_SetEntBmpFileInfo(E3_ID idEM, EzdKernel.E3_ID idEnt, string strFileName, bool bBidir, double dBiDirOffset, bool bDrillmode, double dDrillTime, bool bScanY, bool bPixelPower, bool bFixedDPI, int nDpiX, int nDpiY,
 bool bDynFile, bool bFixedX, bool bFixedY, double dSizeX, double dSizeY, int nFixedPosition, bool bGray, bool bInvert, bool bDither, bool bOptimize, bool bLineOffset, bool bLineIncrement, int nLineIncrement,
            int nMinLowGrayPt, bool bDisableMarkLowGrayPt, double dAccDist, double dDecDist, int nGrayCurvePt, Point[] ptGrayCurveBuf, byte[] bGrayScaleBuf, bool bUndo, string strUndo)
        {
            int nBmpAttrib = 0;
            if (bDynFile) nBmpAttrib |= BMPATTRIB_DYNFILE;
            if (bFixedX) nBmpAttrib |= BMPATTRIB_IMPORTFIXED_WIDTH;
            if (bFixedY) nBmpAttrib |= BMPATTRIB_IMPORTFIXED_HEIGHT;
            if (bFixedDPI) nBmpAttrib |= BMPATTRIB_IMPORTFIXED_DPI;

            int nScanAttrib = 0;
            if (bInvert) nScanAttrib |= BMPSCAN_INVERT;
            if (bGray) nScanAttrib |= BMPSCAN_GRAY;
            if (bDither) nScanAttrib |= BMPSCAN_DITHER;
            if (bBidir) nScanAttrib |= BMPSCAN_BIDIR;
            if (bDrillmode) nScanAttrib |= BMPSCAN_DRILL;
            if (bPixelPower) nScanAttrib |= BMPSCAN_POWER;
            if (bOptimize) nScanAttrib |= BMPSCAN_OPTIMIZE;
            if (bScanY) nScanAttrib |= BMPSCAN_YDIR;
            if (bLineOffset) nScanAttrib |= BMPSCAN_OFFSETPT;

            int[] nParamBuf = new int[10 + 2 * MAX_GRAYCURVEPT_NUM];
            nParamBuf[0] = nBmpAttrib;
            nParamBuf[1] = nScanAttrib;
            nParamBuf[2] = nFixedPosition;
            nParamBuf[3] = nDpiX;
            nParamBuf[4] = nDpiY;
            nParamBuf[5] = bLineIncrement ? 1 : 0;
            nParamBuf[6] = nLineIncrement;
            nParamBuf[7] = bDisableMarkLowGrayPt ? 1 : 0;
            nParamBuf[8] = nMinLowGrayPt;
            nParamBuf[9] = nGrayCurvePt;
            for (int i = 0; i < MAX_GRAYCURVEPT_NUM; i++)
            {//i=1,x=255,y=255 ,i!=1 ,x=0,y=0
                nParamBuf[10 + i * 2] = ptGrayCurveBuf[i].X;
                nParamBuf[10 + i * 2 + 1] = ptGrayCurveBuf[i].Y;
            }

            double[] dParamBuf = new double[10];
            dParamBuf[0] = dSizeX;
            dParamBuf[1] = dSizeY;
            dParamBuf[2] = dDrillTime;
            dParamBuf[3] = dBiDirOffset;
            dParamBuf[4] = dAccDist;
            dParamBuf[5] = dDecDist;

            E3_ERR err = E3_SetBmpFileInfo(idEM, idEnt, nParamBuf, dParamBuf, bGrayScaleBuf, strFileName, bUndo, strUndo);
            return err;
        }



        //dStlOffset stl文件的偏移
        //E3_API int E3_SliceStlFile2(E3_ID idLayer, TCHAR* strStlFile, TCHAR* strName, BOOL bZUpToDown, double dMinZ, double dMaxZ, double dThickness, double dStlOffsetX, double dStlOffsetY, double dStlOffsetZ, int nFlag);
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SliceStlFile2", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SliceStlFile2(E3_POINTER idLayer, string strStlFile, string strName, bool bZUpToDown, double dMinZ, double dMaxZ, double dThickness, double dStlOffsetX, double dStlOffsetY, double dStlOffsetZ, int nFlag);

        public static E3_ERR E3_SliceStlFile2(E3_ID idLayer, string strStlFile, string strName, bool bZUpToDown, double dMinZ, double dMaxZ, double dThickness, double dStlOffsetX, double dStlOffsetY, double dStlOffsetZ, int nFlag)
        {
            return e3_SliceStlFile2(idLayer, strStlFile, strName, bZUpToDown, dMinZ, dMaxZ, dThickness, dStlOffsetX, dStlOffsetY, dStlOffsetZ, nFlag);
        }


        ///条码参数结构体，包含了条码的所有参数
        public struct BarcodeParam
        {
            public string m_strBarcodeFontName;//文本字体名称

            public Int32 m_bBarcodeType;//条码类型 1 是1维，2是2维条码
            public bool m_bBarcodeCheckNum;
            public bool m_bBarcodeReverse;//条码反转


            public double m_dHeight;//整个条码的高	
            public double m_dNarrowWidth;//最窄模块宽	

            public bool m_bFixedSize;
            public double m_dFixedSizeX;
            public double m_dFixedSizeY;


            //1维码参数
            public double m_dBarWidthScale0;//条宽比例	(与最窄模块宽相比)
            public double m_dBarWidthScale1;
            public double m_dBarWidthScale2;
            public double m_dBarWidthScale3;

            public double m_dSpaceWidthScale0;//空宽比例(与最窄模块宽相比)
            public double m_dSpaceWidthScale1;
            public double m_dSpaceWidthScale2;
            public double m_dSpaceWidthScale3;

            public double m_dMidCharSpaceScale;//字符间隔比例(与最窄模块宽相比)

            public bool m_nEnableInterHatch;//1维条码内部填充 qy
            public double m_dHatchLineSpace;//填充线间距
            public double m_dLaserBeamDiameter;//激光光斑直径


            //二维码参数
            public Int32 m_n2DBarMode;//二维码条码模式
            public bool m_n2DBarReverseMode;//二维码条码反转模式

            public double m_d2DBarSize;//尺寸 

            public Int32 m_nRow;//行数
            public Int32 m_nCol;//列数
            public Int32 m_nCheckLevel;//错误纠正级别	
            public Int32 m_nSizeMode;//DataMatrix尺寸模式
            public Int32 m_nMaskPattern;//QRCode掩模图形参考	

            public bool m_nDmEnableeTilde;
            public bool m_nPdfShortMode;

            public double m_dOptimizCompt;//优化补偿
            public Int32 m_nPointTimesN;//20140324点倍数，用于GM码

            //空白参数
            public double m_dQuietLeftScale;//条码左空白宽度比例(与最窄模块宽相比)
            public double m_dQuietMidScale;//条码中空白宽度比例(与最窄模块宽相比)
            public double m_dQuietRightScale;//条码右空白宽度比例(与最窄模块宽相比)	
            public double m_dQuietTopScale;//条码上空白宽度比例(与最窄模块宽相比)
            public double m_dQuietBottomScale;//条码下空白宽度比例(与最窄模块宽相比)


            //人识别字符 
            public bool m_nShowText;
            public Int32 m_nTextPen;
            public bool m_bShowCheckNum;
            public double m_dTextHeight;
            public double m_dTextWidth;
            public double m_dTextOffsetX;
            public double m_dTextOffsetY;
            public double m_dTextSpace;

            public string m_strTextFontName;//文本字体名称


            public bool m_bEnableHatchText;
            public bool m_bEnableContour;
            public bool m_bContourFirst;
            public HatchParam m_HatchFillParam1;
            public HatchParam m_HatchFillParam2;
            public HatchParam m_HatchFillParam3;

            public bool m_bTextFixedSize;
            public double m_dTextFixedSizeX;
            public double m_dTextFixedSizeY;

            public BarcodeParam(string strFontName)
            {

                m_strBarcodeFontName = strFontName;
                m_bBarcodeType = 1;//条码类型 1 是1维，2是2维条码 

                m_bBarcodeCheckNum = false;
                m_bBarcodeReverse = false;//条码反转


                m_dHeight = 10;//整个条码的高	
                m_dNarrowWidth = 1;//最窄模块宽	

                m_bFixedSize = false;
                m_dFixedSizeX = 10;
                m_dFixedSizeY = 10;


                //1维码参数
                m_dBarWidthScale0 = 1;//条宽比例	(与最窄模块宽相比)
                m_dBarWidthScale1 = 2;
                m_dBarWidthScale2 = 3;
                m_dBarWidthScale3 = 4;

                m_dSpaceWidthScale0 = 1;//空宽比例(与最窄模块宽相比)
                m_dSpaceWidthScale1 = 2;
                m_dSpaceWidthScale2 = 3;
                m_dSpaceWidthScale3 = 4;

                m_dMidCharSpaceScale = 1;//字符间隔比例(与最窄模块宽相比)

                m_nEnableInterHatch = false;//1维条码内部填充 qy
                m_dHatchLineSpace = 0.1;//填充线间距
                m_dLaserBeamDiameter = 0.05;//激光光斑直径


                //二维码参数
                m_n2DBarMode = 0;//二维码条码模式
                m_n2DBarReverseMode = false;//二维码条码反转模式

                m_d2DBarSize = 1;//尺寸 

                m_nRow = 5;//行数
                m_nCol = 5;//列数
                m_nCheckLevel = 0;//错误纠正级别	
                m_nSizeMode = 0;//DataMatrix尺寸模式
                m_nMaskPattern = 0;//QRCode掩模图形参考	

                m_nPdfShortMode = false;
                m_nDmEnableeTilde = false;

                m_dOptimizCompt = 0;//优化补偿
                m_nPointTimesN = 1;//20140324点倍数，用于GM码

                //空白参数
                m_dQuietLeftScale = 10;//条码左空白宽度比例(与最窄模块宽相比)
                m_dQuietMidScale = 10;//条码中空白宽度比例(与最窄模块宽相比)
                m_dQuietRightScale = 10;//条码右空白宽度比例(与最窄模块宽相比)	
                m_dQuietTopScale = 10;//条码上空白宽度比例(与最窄模块宽相比)
                m_dQuietBottomScale = 10;//条码下空白宽度比例(与最窄模块宽相比)


                //人识别字符 
                m_nShowText = false;
                m_nTextPen = 0;
                m_bShowCheckNum = false;
                m_dTextHeight = 2;
                m_dTextWidth = 1;
                m_dTextOffsetX = 0;
                m_dTextOffsetY = 0;
                m_dTextSpace = 0;
                m_strTextFontName = "Arial";

                m_bEnableHatchText = false;
                m_bEnableContour = true;
                m_bContourFirst = false;
                m_HatchFillParam1 = new HatchParam();
                m_HatchFillParam2 = new HatchParam();
                m_HatchFillParam3 = new HatchParam();

                m_bTextFixedSize = false;
                m_dTextFixedSizeX = 10;
                m_dTextFixedSizeY = 10;
            }

            public string FontName()
            {
                return m_strBarcodeFontName;
            }

            public void SetFontName(string str)
            {
                m_strBarcodeFontName = str;
            }

            public string TextFontName()
            {
                return m_strTextFontName;
            }
            public void SetTextFontName(string str)
            {
                m_strTextFontName = str;
            }

            public bool Is1D()
            {
                return m_bBarcodeType == 1 ? true : false;
            }
            public bool Is2D()
            {
                return m_bBarcodeType == 2 ? true : false;
            }

            public int[] GetParamN()
            {
                int[] nParam = new int[22];

                nParam[0] = m_bBarcodeType;//一维码==1，二维码==2
                nParam[1] = m_bBarcodeCheckNum ? 1 : 0;
                nParam[2] = m_bBarcodeReverse ? 1 : 0;//反转
                nParam[3] = m_bFixedSize ? 1 : 0;//条码固定尺寸
                nParam[4] = m_nEnableInterHatch ? 1 : 0;//使能内部填充线
                nParam[5] = m_n2DBarMode;//条码模式 3=点 1=矩形 2=圆 0=标准模式
                nParam[6] = m_n2DBarReverseMode ? 1 : 0;//黑白反转
                nParam[7] = m_nRow;//行数
                nParam[8] = m_nCol;//列数
                nParam[9] = m_nCheckLevel;//错误纠正级别
                nParam[10] = m_nSizeMode;//条码版本号
                nParam[11] = m_nMaskPattern;
                nParam[12] = m_nPdfShortMode ? 1 : 0;//PDF417码缩短模式
                nParam[13] = m_nPointTimesN;//点倍数
                nParam[14] = m_nShowText ? 1 : 0;//显示人可识别字符
                nParam[15] = m_nTextPen;//可识别字符笔号
                nParam[16] = m_bShowCheckNum ? 1 : 0;//显示校验码
                nParam[17] = m_bEnableHatchText ? 1 : 0;//是否填充可识别字符
                nParam[18] = m_bEnableContour ? 1 : 0;//是否使能轮廓
                nParam[19] = m_bContourFirst ? 1 : 0; //轮廓是否优先加工
                nParam[20] = m_bTextFixedSize ? 1 : 0;//可识别字符固定尺寸
                nParam[21] = m_nDmEnableeTilde ? 1 : 0;//使能转义字符

                return nParam;
            }

            public double[] GetParamD()
            {
                double[] dParam = new double[28];

                dParam[0] = m_dHeight;//一维码条码高
                dParam[1] = m_dNarrowWidth;//窄条模块宽
                dParam[2] = m_dFixedSizeX;//固定尺寸X
                dParam[3] = m_dFixedSizeY;//固定尺寸Y
                dParam[4] = m_dBarWidthScale0;
                dParam[5] = m_dBarWidthScale1;
                dParam[6] = m_dBarWidthScale2;
                dParam[7] = m_dBarWidthScale3;
                dParam[8] = m_dSpaceWidthScale0;
                dParam[9] = m_dSpaceWidthScale1;
                dParam[10] = m_dSpaceWidthScale2;
                dParam[11] = m_dSpaceWidthScale3;
                dParam[12] = m_dMidCharSpaceScale;
                dParam[13] = m_dHatchLineSpace;//内部填充线间距
                dParam[14] = m_dLaserBeamDiameter;//激光光斑直径
                dParam[15] = m_d2DBarSize;

                dParam[16] = m_dQuietLeftScale;//左静区比例
                dParam[17] = m_dQuietMidScale;//中间静区比例
                dParam[18] = m_dQuietRightScale;//右静区比例
                dParam[19] = m_dQuietTopScale; //顶部静区比例
                dParam[20] = m_dQuietBottomScale;//底部静区比例

                dParam[21] = m_dTextHeight;//人可识别字符高度
                dParam[22] = m_dTextWidth;//人可识别字符宽度
                dParam[23] = m_dTextOffsetX;//文本X偏移
                dParam[24] = m_dTextOffsetY;//文本Y偏移
                dParam[25] = m_dTextSpace;//文本间距
                dParam[26] = m_dTextFixedSizeX;//人可识别字符固定尺寸X
                dParam[27] = m_dTextFixedSizeY;//人可识别字符固定尺寸Y
                return dParam;
            }

            public void SetParamN(int[] nParam)
            {
                m_bBarcodeType = nParam[0];
                m_bBarcodeCheckNum = nParam[1] != 0 ? true : false;
                m_bBarcodeReverse = nParam[2] != 0 ? true : false;
                m_bFixedSize = nParam[3] != 0 ? true : false;
                m_nEnableInterHatch = nParam[4] != 0 ? true : false;

                m_n2DBarMode = nParam[5];
                m_n2DBarReverseMode = nParam[6] != 0 ? true : false;//黑白反转
                m_nRow = nParam[7];
                m_nCol = nParam[8];
                m_nCheckLevel = nParam[9];
                m_nSizeMode = nParam[10];
                m_nMaskPattern = nParam[11];
                m_nPdfShortMode = nParam[12] != 0 ? true : false;
                m_nPointTimesN = nParam[13];

                m_nShowText = nParam[14] != 0 ? true : false;

                m_nTextPen = nParam[15];
                m_bShowCheckNum = nParam[16] != 0 ? true : false;

                m_bEnableHatchText = nParam[17] != 0 ? true : false;
                m_bEnableContour = nParam[18] != 0 ? true : false;
                m_bContourFirst = nParam[19] != 0 ? true : false;

                m_bTextFixedSize = nParam[20] != 0 ? true : false;
                m_nDmEnableeTilde = nParam[21] != 0 ? true : false;
            }

            public void SetParamD(double[] dParam)
            {
                m_dHeight = dParam[0];
                m_dNarrowWidth = dParam[1];
                m_dFixedSizeX = dParam[2];
                m_dFixedSizeY = dParam[3];
                m_dBarWidthScale0 = dParam[4];
                m_dBarWidthScale1 = dParam[5];
                m_dBarWidthScale2 = dParam[6];
                m_dBarWidthScale3 = dParam[7];
                m_dSpaceWidthScale0 = dParam[8];
                m_dSpaceWidthScale1 = dParam[9];
                m_dSpaceWidthScale2 = dParam[10];
                m_dSpaceWidthScale3 = dParam[11];
                m_dMidCharSpaceScale = dParam[12];
                m_dHatchLineSpace = dParam[13];
                m_dLaserBeamDiameter = dParam[14];
                m_d2DBarSize = dParam[15];

                m_dQuietLeftScale = dParam[16];
                m_dQuietMidScale = dParam[17];
                m_dQuietRightScale = dParam[18];
                m_dQuietTopScale = dParam[19];
                m_dQuietBottomScale = dParam[20];

                m_dTextHeight = dParam[21];
                m_dTextWidth = dParam[22];
                m_dTextOffsetX = dParam[23];
                m_dTextOffsetY = dParam[24];
                m_dTextSpace = dParam[25];
                m_dTextFixedSizeX = dParam[26];
                m_dTextFixedSizeY = dParam[27];
            }
        }


        /// <summary>
        /// 设置条码参数
        /// </summary>
        /// <param name="idEM"></param>
        /// <param name="idEnt"></param>
        /// <param name="nMaxParamN"></param>
        /// <param name="nParam"></param>
        /// <param name="nMaxParamD"></param>
        /// <param name="dParam"></param>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="strText"></param>
        /// <param name="bUndo"></param>
        /// <param name="strUndo"></param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SetTextBarcodeInfo", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        extern static E3_ERR e3_SetTextBarcodeInfo(E3_POINTER idEM, E3_POINTER idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, StringBuilder str1, StringBuilder str2, HatchParam param1, HatchParam param2, HatchParam param3, string strText, bool bUndo, string strUndo);
        public static E3_ERR E3_SetTextBarcodeInfo(E3_ID idEM, E3_ID idEnt, int nMaxParamN, int[] nParam, int nMaxParamD, double[] dParam, StringBuilder str1, StringBuilder str2, HatchParam param1, HatchParam param2, HatchParam param3, string strText, bool bUndo, string strUndo)
        {
            return e3_SetTextBarcodeInfo(idEM, idEnt, nMaxParamN, nParam, nMaxParamD, dParam, str1, str2, param1, param2, param3, strText, bUndo, strUndo);
        }



        public EzdKernel.E3_ID m_idEM;
      
        public bool GetTextBarcodeParam(EzdKernel.E3_ID idEntText, ref BarcodeParam barcodeParam)
        {
            int[] nParamBuf = new int[22];
            double[] dParamBuf = new double[28];
            StringBuilder str1 = new StringBuilder("", 256);
            StringBuilder str2 = new StringBuilder("", 256);

            if (EzdKernel.E3_GetTextBarcodeInfo(idEntText, nParamBuf.Length, nParamBuf, dParamBuf.Length, dParamBuf, str1, str2, ref barcodeParam.m_HatchFillParam1, ref barcodeParam.m_HatchFillParam2, ref barcodeParam.m_HatchFillParam3) != EzdKernel.E3_ERR.SUCCESS)
            {
                return false;
            }
            barcodeParam.SetParamN(nParamBuf);
            barcodeParam.SetParamD(dParamBuf);
            barcodeParam.m_strBarcodeFontName = str1.ToString();
            barcodeParam.m_strTextFontName = str2.ToString();
            return true;
        }



        #endregion

        /// <summary>
        /// 振镜
        /// </summary>
        /// 
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_MarkerGetPos", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_MarkerGetPos(E3_POINTER idMarker, ref double x, ref double y, ref double z);
        public static E3_ERR E3_MarkerGetPos(E3_ID idMarker, ref double x, ref double y, ref double z)
        {
            return e3_MarkerGetPos(idMarker, ref x, ref y, ref z);
        }

        #region 分割
        //nBox表示pBoxs有多少个矩形
        //dSplitErr表示边界计算误差，一般为0.001就行
        //dArcTol表示曲线离散误差，一般为0.01就行
        //E3_API int E3_SplitEntByBox(E3_ID idEnt, SplitBox* pBoxs, int nBoxCount, double dSplitErr, double dArcTol, TCHAR* strNewEntNamePrefix);
        public struct SplitBox
        {
            public double x1;
            public double y1;
            public double x2;
            public double y2;
            public void Build(double dx1, double dy1, double dx2, double dy2)
            {
                x1 = dx1; x2 = dx2; y1 = dy1; y2 = dy2;
            }
        }

        /// <summary>
        /// 分割接口
        /// </summary>
        /// <param name="idEnt">需要分割的对象名称</param>
        /// <param name="pBoxs">分割盒列表</param>
        /// <param name="nBoxCount">分割盒数量</param>
        /// <param name="dSplitErr">边界计算误差</param>
        /// <param name="dArcTol">曲线离散误差</param>
        /// <param name="strNewEntNamePrefix">分割后对象名标识</param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SplitEntByBox", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SplitEntByBox(E3_POINTER idEnt, SplitBox[] pBoxs, int nBoxCount, double dSplitErr, double dArcTol, string strNewEntNamePrefix);
        public static E3_ERR E3_SplitEntByBox(E3_ID idEnt, SplitBox[] pBoxs, int nBoxCount, double dSplitErr, double dArcTol, string strNewEntNamePrefix)
        {
            return e3_SplitEntByBox(idEnt, pBoxs, nBoxCount, dSplitErr, dArcTol, strNewEntNamePrefix);
        }



        /// <summary>
        /// 把文本对象分离成一个个字符对象(分离后的一个个对象以这个对象字符命名)
        /// </summary>
        /// <param name="idEnt">指定对象ID</param>
        /// <param name="idEntParent">文本对象分离后放到的层ID（不可以是对象ID）</param>
        /// <param name="bTextLeftToRight">是否从左到右分离</param>
        /// <param name="dArcTol">分离精确度默认0.01即可</param>
        /// <returns></returns>
        [DllImport(@"EzCad3DLL\Ezcad3kernel.dll", EntryPoint = "E3_SplitTextToChars", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        static extern E3_ERR e3_SplitTextToChars(E3_POINTER idEnt, E3_POINTER idEntParent, bool bTextLeftToRight, double dArcTol);
        public static E3_ERR E3_SplitTextToChars(E3_ID idEnt, E3_ID idEntParent, bool bTextLeftToRight, double dArcTol)
        {
            return e3_SplitTextToChars(idEnt, idEntParent, bTextLeftToRight, dArcTol);
        }

        #endregion


    }
}
