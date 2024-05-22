using BlApi;
using PL.Engineer;
using System.Windows;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Engineer_List(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void Init_DB(object sender, RoutedEventArgs e)
        {
            MessageBoxResult initDBResult = MessageBox.Show("Are you sure you want to initialization the data base?", "Initialization DB",
                MessageBoxButton.OKCancel, MessageBoxImage.Error);
            switch (initDBResult)
            {
                case MessageBoxResult.OK:
                    s_bl.InitializeDB();
                    break;
                case MessageBoxResult.Cancel:

                    break;
            }
        }

        private void Reset_DB(object sender, RoutedEventArgs e)
        {
            MessageBoxResult initDBResult = MessageBox.Show("Are you sure you want to reset all the data base?", "Reset DB",
                MessageBoxButton.OKCancel, MessageBoxImage.Error);
            switch (initDBResult)
            {
                case MessageBoxResult.OK:
                    s_bl.ResetDB();
                    break;
                case MessageBoxResult.Cancel:

                    break;
            }
        }
    }
}
