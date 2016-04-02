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
	
	//Start point of the data
	var startOfDate = DateTime.Now.Subtract(TimeSpan.FromDays(1));
	
	//https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
	var formatString = "m.ss.fffff";
	
	Func<double, string> labelFormatter = 
		milliseconds => startOfDate.Add(TimeSpan.FromMilliseconds(milliseconds)).ToString(formatString);

	//Use any parsing/transformation/formatting of the input double you'd like
	pm.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left });
	pm.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Bottom, LabelFormatter = labelFormatter });
								
	Random random = new Random(Guid.NewGuid().GetHashCode());
	
	var datapoints = 
		Enumerable.Range(1, 100)
			.Select(milliseconds => new DataPoint(milliseconds, random.Next(0, 10)));
			
	var lineSeries = new OxyPlot.Series.LineSeries();
	lineSeries.Points.AddRange(datapoints);

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