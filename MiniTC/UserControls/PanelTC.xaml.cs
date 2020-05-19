using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniTC.UserControls
{
    /// <summary>
    /// Interaction logic for PanelTC.xaml
    /// </summary>
    public partial class PanelTC : UserControl
    {
        string selectedDrive;
        public string SelectedDrive
        {
            get { return selectedDrive; }
            set
            {
                selectedDrive = value;
                onDriveSelection();
            }
        }
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set
            {
                SetValue(PathProperty, value);
            }
        }

        public static readonly DependencyProperty PathProperty =
           DependencyProperty.Register("Path", typeof(string),
             typeof(PanelTC), new PropertyMetadata(default(string)));

        string[] Drives
        {
            get { return (string[])GetValue(DrivesProperty); }
            set { SetValue(DrivesProperty, value); }
        }

        public static readonly DependencyProperty DrivesProperty =
            DependencyProperty.Register("Drives", typeof(string[]),
              typeof(PanelTC), new PropertyMetadata(default(string[])));

        public string[] FilesAndDirectories
        {
            get { return (string[])GetValue(FilesAndDirectoriesProperty); }
            set { SetValue(FilesAndDirectoriesProperty, value); }
        }

        public static readonly DependencyProperty FilesAndDirectoriesProperty =
            DependencyProperty.Register("FilesAndDirectories", typeof(string[]),
              typeof(PanelTC), new PropertyMetadata(default(string[])));

        public static readonly RoutedEvent ItemSelectedEventRegistered =
       EventManager.RegisterRoutedEvent(nameof(ItemSelectedEvent),
                    RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                    typeof(PanelTC));

        //definicja zdarzenia MojeZdarzenie
        public event RoutedEventHandler ItemSelectedEvent
        {
            add { AddHandler(ItemSelectedEventRegistered, value); }
            remove { RemoveHandler(ItemSelectedEventRegistered, value); }
        }
        void ItemSelected(object sender, SelectionChangedEventArgs args)
        {

            if (args.AddedItems.Count > 0)
            {
                if (args.AddedItems[0].ToString() == "..")
                {
                    var arr = Path.Split("\\");
                    var drive = arr[0];
                    var path = arr.Skip(1).ToArray();
                    path = path.SkipLast(1).ToArray();

                    Path = drive + "\\" + string.Join("\\", path);
                    refreshFilesAndDirs();
                }
                else
                {
                    FileAttributes attr = File.GetAttributes(args.AddedItems[0].ToString());
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        Path = args.AddedItems[0].ToString();
                        refreshFilesAndDirs();
                    }
                    else
                    {
                        Path = args.AddedItems[0].ToString();
                        RoutedEventArgs newEventArgs = new RoutedEventArgs(PanelTC.ItemSelectedEventRegistered);
                        RaiseEvent(newEventArgs);
                    }
                }
            }
        }
        public PanelTC()
        {
            InitializeComponent();
        }

        private void onDriveSelection()
        {
            Path = selectedDrive;
            var dirs = Directory.GetDirectories(Path).ToList();
            var files = Directory.GetFiles(Path).ToList();
            FilesAndDirectories = new List<string>(dirs.Concat(files)).ToArray();
        }

        private void refreshFilesAndDirs()
        {
            System.Threading.Thread.Sleep(300);
            var dirs = Directory.GetDirectories(Path).ToList();
            var files = Directory.GetFiles(Path).ToList();
            var filesAndDirs = new List<string>(dirs.Concat(files));
            if (Path.Split("\\")[1] != "")
            {
                filesAndDirs.Insert(0, "..");
            }
            filesAndDirsListBox.UnselectAll();
            FilesAndDirectories = filesAndDirs.ToArray();
        }

        private void OnDropDownOpened(object sender, EventArgs e)
        {
            Drives = Directory.GetLogicalDrives(); //refreshes the list of logical drives every time a user click on the combobox, tested by putting a pendive into a USB port
        }


    }
}
