using System.Linq;

namespace Whim.Bar;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class BarWindow : Microsoft.UI.Xaml.Window
{
	private readonly IConfigContext _configContext;
	private readonly BarConfig _barConfig;
	private readonly IMonitor _monitor;

	/// <summary>
	/// The current window state.
	/// </summary>
	public IWindowState WindowState { get; }

	/// <summary>
	/// Creates a new bar window.
	/// </summary>
	/// <param name="configContext"></param>
	/// <param name="barConfig"></param>
	/// <param name="monitor"></param>
	/// <exception cref="BarException"></exception>
	public BarWindow(IConfigContext configContext, BarConfig barConfig, IMonitor monitor)
	{
		_configContext = configContext;
		_barConfig = barConfig;
		_monitor = monitor;

		UIElementExtensions.InitializeComponent(this, "Whim.Bar", "BarWindow");

		IWindow? window = IWindow.CreateWindow(_configContext, this.GetHandle());
		if (window == null)
		{
			throw new BarException("Window was unexpectedly null");
		}
		;

		double scaleFactor = _monitor.ScaleFactor;
		double scale = scaleFactor / 100.0;

		WindowState = new WindowState()
		{
			Window = window,
			Location = new Location<int>()
			{
				X = _monitor.X,
				Y = _monitor.Y,
				Width = (int)(_monitor.Width / scale),
				Height = _barConfig.Height
			},
			WindowSize = WindowSize.Normal
		};

		// Workaround for https://github.com/microsoft/microsoft-ui-xaml/issues/3689
		Title = "Whim Bar";
		Win32Helper.HideCaptionButtons(WindowState.Window.Handle);
		this.SetIsShownInSwitchers(false);

		// Set up the bar.
		LeftPanel.Children.AddRange(_barConfig.LeftComponents.Select(c => c(_configContext, _monitor, this)));
		CenterPanel.Children.AddRange(_barConfig.CenterComponents.Select(c => c(_configContext, _monitor, this)));
		RightPanel.Children.AddRange(_barConfig.RightComponents.Select(c => c(_configContext, _monitor, this)));
	}
}
