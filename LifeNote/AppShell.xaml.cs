namespace LifeNote;

/// <summary>
/// 该类用于定义应用的可视层次结构
/// </summary>
public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        //Every page that can be navigated to from another page, needs to be registered with the navigation system.
        Routing.RegisterRoute(nameof(Views.NotePage), typeof(Views.NotePage));
    }
}
