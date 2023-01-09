namespace LifeNote;

/// <summary>
/// App.xaml 文件包含应用范围的 XAML 资源，例如颜色、样式或模板。 
/// App.xaml.cs 文件通常包含实例化 Shell 应用程序的代码。
/// </summary>
public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
