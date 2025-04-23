using Lab1.Entities;
using Lab1.Services;

namespace Lab1;

public partial class SQLitePage : ContentPage
{
    private readonly IDbService dbService;

    public SQLitePage(IDbService dbService)
    {
        this.dbService = dbService;
        InitializeComponent();
    }

    private void OnSQLitePageLoaded(object sender, EventArgs e)
    {
        WardPicker.ItemsSource = dbService.GetAllWards().ToList();
    }

    private void WardPickerOnSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedWard = WardPicker.SelectedItem as Ward;

        if (selectedWard != null)
        {
            PatientCollectionView.ItemsSource = dbService.GetWardPatients(selectedWard.Id).ToList();
        }
    }
}