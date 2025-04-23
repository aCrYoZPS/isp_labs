using System.Globalization;
using Lab1.Services;
using Lab1.Entities;

namespace Lab1;

public partial class CurrencyPage : ContentPage
{
    private bool toByn = false;
    private object? senderEntry = null;
    private readonly List<string> allowedCurrencies = new List<string>
    {
        "RUB", "USD", "EUR", "GBP", "PLN", "CHF", "CNY"
    };
    private readonly IRateService rateService;

    public CurrencyPage(IRateService rateService)
    {
        this.rateService = rateService;
        InitializeComponent();
    }

    private void CurrencyPageOnAppearing(object? sender, EventArgs e)
    {
        Refresh();
    }

    private void OnRefreshButtonClicked(object sender, EventArgs e)
    {
        Refresh();
    }

    private void Refresh()
    {
        var rates = rateService.GetRates(DateTime.Now)
            .Where(r => allowedCurrencies.Contains(r.CurAbbreviation))
            .ToList();
        CurrencyPicker1.ItemsSource = rates;
        CurrencyPicker2.ItemsSource = rates;
        CurrencyPicker3.ItemsSource = rates;
    }

    private void OnBynEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(BynEntry.Text))
        {
            return;
        }

        if (!double.TryParse(BynEntry.Text, NumberStyles.Currency, CultureInfo.InvariantCulture, out var bynAmount))
        {
            return;
        }

        if (CurrencyPicker1.SelectedItem is Rate rate1 && senderEntry != CurrencyEntry1)
        {
            CurrencyEntry1.Text =
                (bynAmount * rate1.CurScale / (double)rate1.CurOfficialRate!)
                .ToString("F2", CultureInfo.InvariantCulture);
        }

        if (CurrencyPicker2.SelectedItem is Rate rate2 && senderEntry != CurrencyEntry2)
        {
            CurrencyEntry2.Text =
                (bynAmount * rate2.CurScale / (double)rate2.CurOfficialRate!)
                .ToString("F2", CultureInfo.InvariantCulture);
        }

        if (CurrencyPicker3.SelectedItem is Rate rate3 && senderEntry != CurrencyEntry3)
        {
            CurrencyEntry3.Text =
                (bynAmount * rate3.CurScale / (double)rate3.CurOfficialRate!)
                .ToString("F2", CultureInfo.InvariantCulture);
        }
    }

    private void OnCurrencyEntry1TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!CurrencyEntry1.IsFocused)
        {
            return;
        }

        if (CurrencyPicker1.SelectedItem is not Rate rate)
        {
            return;
        }

        if (!double.TryParse(CurrencyEntry1.Text, NumberStyles.Currency, CultureInfo.InvariantCulture,
                out var currencyAmount))
        {
            return;
        }

        senderEntry = CurrencyEntry1;
        BynEntry.Text =
            (currencyAmount * (double)rate.CurOfficialRate! / rate.CurScale)
            .ToString("F2", CultureInfo.InvariantCulture);
    }

    private void OnCurrencyEntry2TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!CurrencyEntry2.IsFocused)
        {
            return;
        }

        if (CurrencyPicker2.SelectedItem is not Rate rate)
        {
            return;
        }

        if (!double.TryParse(CurrencyEntry2.Text, NumberStyles.Currency, CultureInfo.InvariantCulture,
                out var currencyAmount))
        {
            return;
        }

        senderEntry = CurrencyEntry2;
        BynEntry.Text =
            (currencyAmount * (double)rate.CurOfficialRate! / rate.CurScale)
            .ToString("F2", CultureInfo.InvariantCulture);
    }

    private void OnCurrencyEntry3TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!CurrencyEntry3.IsFocused)
        {
            return;
        }

        if (CurrencyPicker3.SelectedItem is not Rate rate)
        {
            return;
        }

        if (!double.TryParse(CurrencyEntry3.Text, NumberStyles.Currency, CultureInfo.InvariantCulture,
                out var currencyAmount))
        {
            return;
        }

        senderEntry = CurrencyEntry3;
        BynEntry.Text =
            (currencyAmount * (double)rate.CurOfficialRate! / rate.CurScale)
            .ToString("F2", CultureInfo.InvariantCulture);
    }
}