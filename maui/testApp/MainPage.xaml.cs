namespace testApp;

using System.Diagnostics;
using adjustSdk;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		Trace.WriteLine("OnCounterClicked");

		Adjust.ping();
		Adjust.commonPing();

		var testLibrary = new TestLibraryBridge();
		testLibrary.start();

		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

