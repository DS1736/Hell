using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InIWorkspace.UserControls
{
    /// <summary>
    /// Interaction logic for AdminLoginControl.xaml
    /// </summary>
    public partial class AdminLoginControl : UserControl
    {
        public AdminLoginControl()
        {
            InitializeComponent();
            StartClock();
            SetCopyRightText();
        }
        #region Variable

        private DispatcherTimer dispatcherTimer;

        #endregion Variable


        private void SetCopyRightText()
        {
            txtCopyRightText.Text = $"© {DateTime.Now.Year} Infini app, LLC. All rights reserved.";
        }
        private void StartClock()
        {
            dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += Timer_Tick;
            dispatcherTimer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            btnDisplayTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        }
    }
}
