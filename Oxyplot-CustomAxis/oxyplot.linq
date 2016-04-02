<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <NuGetReference Prerelease="true">OxyPlot.Wpf</NuGetReference>
  <Namespace>OxyPlot</Namespace>
  <Namespace>OxyPlot.Series</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>OxyPlot.Wpf</Namespace>
  <Namespace>OxyPlot.Axes</Namespace>
</Query>

void Main()
{
	var pm = new PlotModel();

	//Use any parsing/transformation/formatting of the input double you'd like
	pm.Axes.Add(new FuncLinearAxis(AxisPosition.Left, input => $"Custom!: {input}"));
	pm.Axes.Add(new FuncLinearAxis(AxisPosition.Bottom,	input => Math.Floor(input).ToString()));
								
	var lineSeries = new OxyPlot.Series.LineSeries();
	lineSeries.Points.Add( new DataPoint (1,1));
	lineSeries.Points.Add( new DataPoint (2,2));
	lineSeries.Points.Add( new DataPoint (3,1.5));

	pm.Series.Add(lineSeries);
	
	Show(pm);
}

public void Show(PlotModel model, double width = 800, double height = 500)
{
	var w = new Window() { Title = "OxyPlot.Wpf.Plot : " + model.Title, Width = width, Height = height };
	var plot = new PlotView();
	plot.Model = model;
	w.Content = plot;
	w.Show();
}

public class FuncLinearAxis : OxyPlot.Axes.LinearAxis
{
	private Func<double, string> formatter = null;

	public FuncLinearAxis(AxisPosition position, Func<double, string> formatter)
	{
		this.Position = position;
		this.formatter = formatter;
	}

	protected override string FormatValueOverride(double x)
	{
		return formatter.Invoke(x);
	}
}