using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Passwords
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Objects
        public event PropertyChangedEventHandler PropertyChanged;
        public static DispatcherTimer timerOverall = new DispatcherTimer();
        public MainWindow MainWindow { get; set; }
        public Write write = new Write();
        public Random r = new Random();
        public Task T;
        public BackgroundWorker worker;
        #endregion
        #region Properties
        private int n { get; set; } = 5;
        public int N
        {
            get
            {
                return n;
            }
            set
            {
                if (n != value)
                {
                    n = value;
                }
                OnPropertyChange(nameof(N));
            }
        }
        private int length { get; set; } = 20;
        public int Length
        {
            get
            {
                return length;
            }
            set
            {
                if (length != value)
                {
                    length = value;
                    if (!String.IsNullOrEmpty(TargetDirectory))
                    {
                        Target = TargetDirectory + @"\" + "Passwords" + Length + ".seq";
                    }
                    else
                    {
                        Target = "Passwords" + Length + ".seq";
                    }
                }
                OnPropertyChange(nameof(Length));
            }
        }
        private int complexity { get; set; } = 3;
        public int Complexity
        {
            get
            {
                return complexity;
            }
            set
            {
                if (complexity != value)
                {
                    complexity = value;
                }
                OnPropertyChange(nameof(Complexity));
            }
        }
        private string target { get; set; }
        public string Target
        {
            get
            {
                return target;
            }
            set
            {
                if (target != value)
                {
                    target = value;
                }
                if (!String.IsNullOrEmpty(Target))
                {
                    DelVis = Visibility.Visible;
                }
                else
                {
                    DelVis = Visibility.Hidden;
                }
                OnPropertyChange(nameof(Target));
            }
        }
        private bool latinLetters { get; set; } = true;
        public bool LatinLetters
        {
            get
            {
                return latinLetters;
            }
            set
            {
                if (latinLetters != value)
                {
                    latinLetters = value;
                    OnPropertyChange(nameof(LatinLetters));
                }
            }
        }
        private bool numbers { get; set; } = true;
        public bool Numbers
        {
            get
            {
                return numbers;
            }
            set
            {
                if (numbers != value)
                {
                    numbers = value;
                    OnPropertyChange(nameof(Numbers));
                }
            }
        }
        private bool extraSymbols { get; set; } = false;
        public bool ExtraSymbols
        {
            get
            {
                return extraSymbols;
            }
            set
            {
                if (extraSymbols != value)
                {
                    extraSymbols = value;
                    //AddList(ExtraSymbolsList);
                    OnPropertyChange(nameof(ExtraSymbols));
                }
            }
        }
        private bool germanLetters { get; set; } = false;
        public bool GermanLetters
        {
            get
            {
                return germanLetters;
            }
            set
            {
                if (germanLetters != value)
                {
                    germanLetters = value;
                    //AddList(GermanLettersList);
                    OnPropertyChange(nameof(GermanLetters));
                }
            }
        }
        private bool currencies { get; set; } = false;
        public bool Currencies
        {
            get
            {
                return currencies;
            }
            set
            {
                if (currencies != value)
                {
                    currencies = value;
                    OnPropertyChange(nameof(Currencies));
                }
            }
        }
        private bool sequences { get; set; } = true;
        public bool Sequences
        {
            get
            {
                return sequences;
            }
            set
            {
                if (sequences != value)
                {
                    sequences = value;
                    OnPropertyChange(nameof(Sequences));
                }
            }
        }
        private bool dictionary { get; set; } = false;
        public bool Dictionary
        {
            get
            {
                return dictionary;
            }
            set
            {
                if (dictionary != value)
                {
                    dictionary = value;
                    OnPropertyChange(nameof(Dictionary));
                }
            }
        }
        private string separatorSymbol { get; set; }
        public string SeparatorSymbol
        {
            get
            {
                return separatorSymbol;
            }
            set
            {
                if (separatorSymbol != value)
                {
                    separatorSymbol = value;
                    OnPropertyChange(nameof(SeparatorSymbol));
                }
            }
        }
        private string targetDirectory { get; set; }
        public string TargetDirectory
        {
            get
            {
                return targetDirectory;
            }
            set
            {
                if (targetDirectory != value)
                {
                    targetDirectory = value;
                    OnPropertyChange(nameof(TargetDirectory));
                }
            }
        }
        private double nValue1 { get; set; }
        public double Value1
        {
            get
            {
                return nValue1;
            }
            set
            {
                if (nValue1 != value)
                {
                    nValue1 = value;
                }
                OnPropertyChange(nameof(Value1));
            }
        }
        public List<string> TotalListofSymbols { get; set; }
        public List<string> NormSymbolsList = new List<string>(){ "a", "A", "b", "B", "c", "C", "d", "D", "e", "E", "f", "F",
            "g", "G", "h", "H", "i", "I", "j", "J", "k", "K", "l", "L","m","M","n","N","o","O","p","P","q","Q","r","R",
            "s","S","t","T","u","U", "v","V","w","W","x","X","y","Y","z","Z" };
        public List<string> NumbersList = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public List<string> ExtraSymbolsList = new List<string>(){
            "<",">",".","-","_","§","%","&","?","!","{","[","]","}","=","*","#","@","~", "^","(",")",":",";","/","’","+","|"};
        public List<string> GermanLettersList = new List<string>() { "ä", "Ä", "ö", "Ö", "ü", "Ü" };
        public List<string> CurrenciesList = new List<string>() { "€", "$", "₼", "¥", "₡", "₱", "¢", "₹", "₪", "¥", "₩", "₭", "₮", "₦", "₽", "฿", "₺", "₴", "₫" };
        public string[] ListOfDirectories { get; set; }
        int minWindow { get; set; } = 5;
        public int MinWindow
        {
            get
            {
                return minWindow;
            }
            set
            {
                if (minWindow != value)
                {
                    minWindow = value;
                    OnPropertyChange(nameof(MinWindow));
                }
            }
        }
        private string warning { get; set; }
        public string Warning
        {
            get
            {
                return warning;
            }
            set
            {
                if (warning != value)
                {
                    warning = value;
                }
                OnPropertyChange(nameof(Warning));
            }
        }
        public string VersionsNumber
        {
            get
            {
                return "Passwords version: V1.0";
            }
            set
            {
                OnPropertyChange(nameof(VersionsNumber));
            }
        }
        #region Visibilities
        private Visibility versionsVis { get; set; } = Visibility.Collapsed;
        public Visibility VersionsVis
        {
            get { return versionsVis; }
            set
            {
                if (versionsVis != value)
                {
                    versionsVis = value;
                }
                OnPropertyChange(nameof(VersionsVis));
            }
        }
        private Visibility delVis { get; set; }
        public Visibility DelVis
        {
            get { return delVis; }
            set
            {
                if (delVis != value)
                {
                    delVis = value;
                }
                OnPropertyChange(nameof(DelVis));
            }
        }
        private Visibility settingsVis { get; set; } = Visibility.Hidden;
        public Visibility SettingsVis
        {
            get { return settingsVis; }
            set
            {
                if (settingsVis != value)
                {
                    settingsVis = value;
                }
                OnPropertyChange(nameof(SettingsVis));
            }
        }
        private Visibility progressBarVis { get; set; } = Visibility.Hidden;
        public Visibility ProgressBarVis
        {
            get { return progressBarVis; }
            set
            {
                if (progressBarVis != value)
                {
                    progressBarVis = value;
                }
                OnPropertyChange(nameof(ProgressBarVis));
            }
        }
        #endregion
        #endregion
        #region Commands
        public ICommand Run { get; set; }
        public ICommand Versions { get; set; }
        public ICommand ClearBox { get; set; }
        public ICommand Setup { get; set; }
        public ICommand OpenSet { get; set; }
        public ICommand SaveResult { get; set; }
        public ICommand MinimizeIt { get; set; }
        public ICommand CloseIt { get; set; }
        public ICommand Exit { get; set; }
        #endregion
        public ViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            Run = new Command((param) => Start(), () => YouCan());
            ClearBox = new Command((param) => Del(), () => YouCan());
            OpenSet = new Command((param) => OpenSettings(), () => YouCan());
            Versions = new Command((param) => ShowVersion(), () => YouCan());
            Exit = new Command((param) => ExitApp(), () => YouCan());
        }
        public void Start()
        {
            if(File.Exists(Target))File.Delete(Target);
            ProgressBarVis = Visibility.Visible;
            TotalListofSymbols = new List<string>();
            if (Sequences)
            {
                if (!latinLetters && !Numbers && !ExtraSymbols && !GermanLetters && !Currencies)
                {
                    Warning = "Please, select at least one list of symbols!";
                    return;
                }
                if (latinLetters)
                {
                    AddList(NormSymbolsList);
                }
                if (Numbers)
                {
                    AddList(NumbersList);
                }
                if (ExtraSymbols)
                {
                    AddList(ExtraSymbolsList);
                }
                if (GermanLetters)
                {
                    AddList(GermanLettersList);
                }
                if (Currencies)
                {
                    AddList(CurrenciesList);
                }
                worker = new BackgroundWorker();
                worker.DoWork += (obj, e) => CompileSeqs(1, "");
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.WorkerReportsProgress = true;
                worker.RunWorkerAsync();
            }
            else if (Dictionary)
            {
                if (File.Exists("Dictionary.txt"))
                {
                    AddList(new List<string>(File.ReadAllLines("Dictionary.txt")));
                }
                else
                {
                    Warning = "Dictionary file is missing!"; return;
                }
                worker = new BackgroundWorker();
                worker.DoWork += (obj, e) => CompileDic(1, "");
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.WorkerReportsProgress = true;
                worker.RunWorkerAsync();
            }
        }
        public void CompileSeqs(object sender, string text)
        {
            int count = TotalListofSymbols.Count;
            int lengthComplexity = Length * Complexity;

            using (var writer = new StreamWriter(Target, true, Encoding.Default, 131072))
            {
                for (int i = 0; i < N; i++)
                {
                    var ListTmp = new List<string>(); //List of length*Complexity elements
                    string LineFinal = string.Empty;
                    for (int j = 0; j < lengthComplexity; j++)
                    {
                        ListTmp.Add(TotalListofSymbols[r.Next(0, count)]);//List - elements instead of chars
                    }
                    for (int j = 0; j < Length; j++)
                    {
                        LineFinal += ListTmp[r.Next(0, ListTmp.Count)];
                    }
                    writer.WriteLine(LineFinal);
                    worker.ReportProgress(i * 100 / N);
                }
            }
            ProgressBarVis = Visibility.Hidden;
        }
        private void CompileDic(object sender, string text)
        {
            int count = TotalListofSymbols.Count;
            int lengthComplexity = Length * Complexity;
            using (var writer = new StreamWriter(Target, true, Encoding.Default, 131072))
            {
                for (int i = 0; i < N; i++)
                {
                    var ListTmp = new List<string>(); //List of length*Complexity elements
                    string LineFinal = string.Empty;
                    for (int j = 0; j < lengthComplexity; j++)
                    {
                        ListTmp.Add(TotalListofSymbols[r.Next(0, count)]);//Elements instead of chars
                    }
                    for (int j = 0; j < Length; j++)
                    {
                        if (j < Length - 1)
                        {
                            LineFinal += ListTmp[r.Next(0, ListTmp.Count)] + SeparatorSymbol;
                        }
                        else
                        {
                            LineFinal += ListTmp[r.Next(0, ListTmp.Count)];
                        }
                    }
                    writer.WriteLine(LineFinal);
                    worker.ReportProgress((i+1) * 100 / N);
                }
            }
            ProgressBarVis = Visibility.Hidden;
        }
        public void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MainWindow.pb.Value = e.ProgressPercentage;
        }
        private void AddList(List<string> list)
        {
            if (list.Count != 0)
            {
                var combined = TotalListofSymbols.Concat(list);
                TotalListofSymbols = new List<string>(combined);
            }
        }
        public string RollIt(string TBtext, char sign)
        {

            int Value;
            if (sign == '1')
            {
                if (String.IsNullOrEmpty(TBtext))
                {
                    Value = -1;
                }
                else
                {
                    Value = Int32.Parse(TBtext);
                }
                TBtext = (Value + 1).ToString();
            }
            else
            {
                if (String.IsNullOrEmpty(TBtext))
                {
                    Value = 1;
                }
                else
                {
                    Value = Int32.Parse(TBtext);
                }

                if (Value - 1 >= 0)
                {
                    TBtext = (Value - 1).ToString();
                }
                else
                {
                    Value = 11;
                    TBtext = (Value - 1).ToString();
                }
            }
            return TBtext;
        }
        public string RollSeparator(string TBtext, char sign)
        {
            int Value;
            if (!String.IsNullOrEmpty(TBtext) && ExtraSymbolsList.Contains(SeparatorSymbol))
            {
                Value = ExtraSymbolsList.IndexOf(SeparatorSymbol);
            }
            else
            {
                Value = 0;
            }
            if (sign == '1')
            {
                if (Value + 1 < ExtraSymbolsList.Count)
                {
                    Value += 1;
                }
                else
                {
                    Value = 0;
                }
            }
            else
            {
                if (Value - 1 >= 0)
                {
                    Value -= 1;
                }
                else
                {
                    Value = ExtraSymbolsList.Count - 1;
                }
            }
            TBtext = ExtraSymbolsList[Value];
            return TBtext;
        }
        public string RollDirs(string TBtext, char sign)
        {
            int Value;
            if (!String.IsNullOrEmpty(TBtext) && ListOfDirectories.Contains(TBtext))
            {
                Value = Array.FindIndex(ListOfDirectories, x => x.Contains(TBtext));
            }
            else
            {
                Value = 0;
            }
            if (sign == '1')
            {
                if (Value + 1 < ListOfDirectories.Length)
                {
                    Value += 1;
                }
                else
                {
                    Value = 0;
                }
            }
            else
            {
                if (Value - 1 >= 0)
                {
                    Value -= 1;
                }
                else
                {
                    Value = ListOfDirectories.Length - 1;
                }
            }
            TBtext = ListOfDirectories[Value];
            TargetDirectory = ListOfDirectories[Value];
            Target = TargetDirectory + @"\" + Target.Split('\\')[Target.Split('\\').Length - 1];
            return TBtext;
        }
        private void OpenSettings()
        {
            if (SettingsVis == Visibility.Hidden)
            {
                SettingsVis = Visibility.Visible;
            }
            else
            {
                SettingsVis = Visibility.Hidden;
            }
        }
        private void Del()
        {
            Target = string.Empty;
            DelVis = Visibility.Hidden;
        }
        private void ShowVersion()
        {
            if (VersionsVis == Visibility.Collapsed)
            {
                VersionsVis = Visibility.Visible;
            }
            else
            {
                VersionsVis = Visibility.Collapsed;
            }
        }
        private void ExitApp()
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void OnPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public bool YouCan()
        {
            return true;
        }
    }
}
