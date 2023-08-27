using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Passwords
{
    public partial class MainWindow : Window
    {
        #region Objects
        public ViewModel ViewModel { get; set; }
        #endregion
        #region Properties
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new ViewModel(this);
            this.DataContext = ViewModel;
            this.MouseDown += delegate { DragMove(); };
            this.LocationChanged += DragMoveCoordinates;
            this.Top = 50;
            this.Left = 600;
            ViewModel.MinimizeIt = new Command((param) => ReshapeWindow("Minimize"), () => ViewModel.YouCan());
            ViewModel.CloseIt = new Command((param) => ReshapeWindow("CloseWin"), () => ViewModel.YouCan());
        }
        private void DragMoveCoordinates(object sender, EventArgs e)
        {
            //ViewModel.LeftValue = Left;
            //ViewModel.TopValue = Top;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            ViewModel.Target = @"Passwords" + ViewModel.Length + ".seq";
            ViewModel.ListOfDirectories = (from dir in Directory.EnumerateDirectories(ViewModel.write.dir) select dir).ToArray();
        }
        private void MouseLeftDown(object sender, MouseEventArgs e)
        {
            ViewModel.SettingsVis = Visibility.Hidden;
            ViewModel.VersionsVis = Visibility.Collapsed;
        }
        private void DragThatOver(object sender, DragEventArgs e)
        {
            e.Effects = System.Windows.DragDropEffects.All;
            e.Handled = true;
        }
        private void TextBoxDoubleClick(object obj, EventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                var tb = obj as System.Windows.Controls.TextBox;
                if (openFileDialog.ShowDialog() == true)
                {
                    string filename = openFileDialog.FileName;
                    tb.Text = filename;
                    string name = tb.Name.ToString();
                    ViewModel.Target = filename;
                }
            }
            catch { }
        }
        private void TextBoxDoubleClickDir(object obj, EventArgs e)
        {
            try
            {
                var tb = obj as TextBox;
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    string path = dialog.SelectedPath;
                    ViewModel.Target = path + @"\" + ViewModel.Target.Split('\\')[ViewModel.Target.Split('\\').Length - 1];
                    tb.Text = path;
                }
            }
            catch { }
        }
        public void ReshapeWindow(string mode)
        {
            switch (mode)
            {
                case "Minimize":
                    this.WindowState = WindowState.Minimized;
                    break;
                case "CloseWin":
                    System.Windows.Application.Current.Shutdown();
                    break;
                default:
                    throw new NotImplementedException(string.Format($"{mode.ToString()} not implemented"));
            }
        }
        private void RollMouseWheel(object sender, MouseWheelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Name == "Separator")
            {
                tb.Text = ViewModel.RollSeparator(tb.Text, e.Delta.ToString()[0]);
            }
            else if(tb.Name == "TargetDir")
            {
                tb.Text = ViewModel.RollDirs(tb.Text, e.Delta.ToString()[0]);
            }
            else
            {
                tb.Text = ViewModel.RollIt(tb.Text, e.Delta.ToString()[0]);
            }
        }
    }
}
