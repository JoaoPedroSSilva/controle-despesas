using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseControl.Models;
using ExpenseControl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ExpenseControl.ViewModels
{
    public partial class DataTransferViewModel : ObservableObject
    {
        private readonly PersonRepository _repo;
        public DataTransferViewModel()
        {
            _repo = App.PersonRepo;
            LoadAvaibleYears();
        }

        [ObservableProperty]
        int selectedYear;

        [ObservableProperty]
        int selectedMonth = DateTime.Now.Month;

        [ObservableProperty]
        ObservableCollection<int> availableYears = new();

        public ObservableCollection<int> availableMonths { get; } =
            new ObservableCollection<int>(Enumerable.Range(1, 12));

        [ObservableProperty]
        string statusMessage;

        [RelayCommand]
        public async Task Export()
        {
            try
            {
                string fileName = $"despesas_{SelectedYear}_{SelectedMonth}_{DateTime.Now:yyyyMMdd+HHmmss}.json";
                string filePath;

#if ANDROID
                string downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                filePath = Path.Combine(downloadsPath, fileName);
#elif WINDOWS
                string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                filePath = Path.Combine(downloadsPath, fileName);
#else
                filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
#endif

                await _repo.ExportExpensesToJsonAsync(filePath, SelectedMonth, SelectedYear);
                StatusMessage = $"Exportado para: {filePath}";

#if ANDROID
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Compartilhar despesas exportadas",
                    File = new ShareFile(filePath)
                });
#endif
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro na exportação: {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task Import()
        {
#if WINDOWS
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecione o arquivo JSON de despesas",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".json" } },
                    })
                });

                if (result != null)
                {
                    await _repo.ImportExpensesFromJsonAsync(result.FullPath);
                    StatusMessage = $"Importado com sucesso de: {result.FileName}";
                }
                else
                {
                    StatusMessage = "Importação cancelada.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao importar: {ex.Message}";
            }
#else
            StatusMessage = "Importação só está disponível no Windows por enquanto.";
#endif
        }

        private async void LoadAvaibleYears()
        {
            List<ExpenseEntry> allExpenses = await _repo.GetAllExpenses();
            var years = allExpenses.Select(e => e.Date.Year).Distinct().OrderByDescending(y => y);
            AvailableYears = new ObservableCollection<int>(years);
            SelectedYear = AvailableYears.FirstOrDefault();
        }
    }
}
