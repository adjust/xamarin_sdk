using System.Diagnostics;
using adjustSdk;

namespace example;
//import sdk;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		var adjustConfig = new AdjustConfig("abcdef123456", AdjustEnvironment.Sandbox);
		adjustConfig.LogLevel = AdjustLogLevel.VERBOSE;
		Adjust.InitSdk(adjustConfig);
		//Adjust.ping();
		//sdk.Adjust.sdk
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

