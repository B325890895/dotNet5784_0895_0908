using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace PL.Engineer;
/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>


public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None;
    public EngineerListWindow()
    {
        InitializeComponent();
        // EngineerList = s_bl?.Engineer.ReadAll()!;
        EngineerList = new ObservableCollection<BO.Engineer>(s_bl?.Engineer.ReadAll())!;

    }
    public ObservableCollection<BO.Engineer> EngineerList
    {
        get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

   
    private void EngineerExperienceSelection(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (Experience == BO.EngineerExperience.None)
        {
            EngineerList = new ObservableCollection<BO.Engineer>(s_bl?.Engineer.ReadAll())!;
        }
        else
        {
            EngineerList=new ObservableCollection<BO.Engineer>(s_bl?.Engineer.ReadAll(item => item.Level == Experience)!)!;
        }
          
    }


    private void addEngineer(object sender, RoutedEventArgs e)
  {
    new EngineerWindow(EngineerList, 0).ShowDialog();

  }
  private void Engineer_dubbleClick(object sender, RoutedEventArgs e)
  {
        BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
    if (EngineerInList != null)
    {
        EngineerWindow ew = new EngineerWindow(EngineerList, EngineerInList.Id);
        ew.ShowDialog();
    }
  }
}