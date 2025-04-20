using MyResto.Pages;

namespace MyResto;

public partial class App : Application
{
     public App()
     {
          InitializeComponent();
          MainPage = new NavigationPage(new HomePage());
     }
}
