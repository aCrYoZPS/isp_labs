namespace Lab1;

public partial class IntegralPage : ContentPage
{
    private const int delay = 1;
    private const double step = 0.001;
    private const int totalSteps = (int)(1 / step);
    private CancellationTokenSource? cancellationTokenSource;

    public IntegralPage()
    {
        InitializeComponent();
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        InfoLabel.Text = "Calculating...";
        PercentageLabel.Text = "0.0%";
        WorkProgressBar.Progress = 0;
        cancellationTokenSource = new CancellationTokenSource();
        try
        {
            var result = await Task.Run(() => CalculateIntegral(cancellationTokenSource.Token));
            InfoLabel.Text = $"Integral value: {result}";
        }
        catch (OperationCanceledException)
        {
            InfoLabel.Text = "Canceled";
        }
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        cancellationTokenSource?.CancelAsync();
    }

    private async Task<double> CalculateIntegral(CancellationToken cancelationToken)
    {
        var a = 0.0;
        var result = 0.0;

        for (var i = 0; i < totalSteps; i++)
        {
            var progress = (double)i / totalSteps;

            await Task.Run(() => Dispatcher.Dispatch(() =>
            {
                WorkProgressBar.Progress = progress;
                PercentageLabel.Text = $"{progress:0.0%}";
            }), cancelationToken);

            cancelationToken.ThrowIfCancellationRequested();
            result += Math.Sin(a) * step;
            a += step;

            await Task.Delay(delay, cancelationToken);
        }

        await Task.Run(() => Dispatcher.Dispatch(() =>
        {
            WorkProgressBar.Progress = 1;
            PercentageLabel.Text = "100,0%";
        }), cancelationToken);

        return result;
    }
}