using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public ObservableCollection<BO.Engineer> engineerList = null; // List of engineers
    string status = "none";
    public BO.Engineer curruntEngineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set
        {
            SetValue(EngineerProperty, value);
        }
    }
    public static readonly DependencyProperty EngineerProperty =
       DependencyProperty.Register("ContentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

    // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty EngineerProperty= DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(BO.Engineer), new PropertyMetadata(null));
    //public static readonly DependencyProperty EngineerProperty = DependencyProperty.Register("ContentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata
    public EngineerWindow(ObservableCollection<BO.Engineer> listEngineer, int Id = 0)
    {
      
        InitializeComponent();
        engineerList = listEngineer;
        if (Id == 0)
        {
            status = "Add";
            curruntEngineer = new BO.Engineer(default,default,default,default, default, default);

            //EngineerProperty = DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(new BO.Engineer(0,"","",BO.EngineerExperience.None,0,null)));
        }
        else
        {
            try
            {
                status = "Update";
                curruntEngineer = s_bl.Engineer.Read(Id);

            }
            catch (Exception e)
            {
                Console.WriteLine( e);
            }
        }
    }

    

    private void updateOrAddEngineer(object sender, RoutedEventArgs e)
    {
        if (status == "Add")
        {
            s_bl.Engineer.Create(curruntEngineer.Id,curruntEngineer.Name,curruntEngineer.Email,curruntEngineer.Level,curruntEngineer.Cost,curruntEngineer.Task);
            engineerList.Add(curruntEngineer);
            MessageBox.Show($"Engineer {curruntEngineer.Id} Added successfuly");
            this.Close();
        }
        else
        {
            s_bl.Engineer.Update(curruntEngineer);
           BO.Engineer engineerToUpdate=engineerList.FirstOrDefault(item => item.Id == curruntEngineer.Id)!;
            engineerList.Remove(engineerToUpdate);
            engineerList.Add(curruntEngineer);
            MessageBox.Show($"Engineer {curruntEngineer.Id} Updated successfuly");
            this.Close();
        }
    }

    private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }
}






















//// <summary>
///// Window class for creating/updating engineers in a WPF application.
///// </summary>
//public partial class EngineerWindow : Window
//{
//  //  static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Business Logic API
//    static int idState = 0;
//    //public ObservableCollection<BO.Engineer> engineerList = null; // List of engineers

//    /// <summary>
//    /// Dependency property representing the content engineer being displayed or edited.
//    /// </summary>
//    public BO.Engineer ContentEngineer
//    {
//        get { return (BO.Engineer)GetValue(EngineerProperty); }
//        set { SetValue(EngineerProperty, value); }
//    }

//    public static readonly DependencyProperty EngineerPropert =
//        DependencyProperty.Register("ContentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

//    /// <summary>
//    /// Constructor for EngineerWindow.
//    /// </summary>
//    /// <param name="listEngineer">ObservableCollection of Engineer objects.</param>
//    /// <param name="Id">Optional parameter for engineer ID.</param>
//    public EngineerWindow(ObservableCollection<BO.Engineer> listEngineer, int Id = 0)
//    {
//        InitializeComponent();
//        engineerList = listEngineer;
//        if (Id == 0)
//        {
//            ContentEngineer = new BO.Engineer();
//        }
//        else
//        {
//            ContentEngineer = s_bl.Engineer.Read(Id);
//            idState = Id;
//        }
//    }

//    /// <summary>
//    /// Event handler for the save button click event.
//    /// </summary>
//    private void save_click(object sender, RoutedEventArgs e)
//    {
//        try
//        {
//            if (idState == 0)
//            {
//                // Creating a new engineer
//                s_bl.Engineer.Create(ContentEngineer);
//                engineerList.Add(ContentEngineer);
//                MessageBox.Show("Engineer created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
//            }
//            else
//            {
//                // Updating an existing engineer
//                s_bl.Engineer.Update(ContentEngineer);
//                BO.Engineer engineerToUpdate = engineerList.First(e => e.Id == idState);
//                engineerList.Remove(engineerToUpdate);
//                engineerList.Add(ContentEngineer);
//                MessageBox.Show("Engineer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
//            }
//            this.Close();
//        }
//        catch (BlNullPropertyException ex)
//        {
//            MessageBox.Show($"You must fill in name, email, and cost", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//        }
//        catch (InvalidInputException ex)
//        {
//            MessageBox.Show($"Invalid input: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//        }
//        catch (BlAlreadyExistsException ex)
//        {
//            MessageBox.Show($"An engineer with such ID already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//        }
//        catch (Exception ex)
//        {
//            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//        }
//    }

//}