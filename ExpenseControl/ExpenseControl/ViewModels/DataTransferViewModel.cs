using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseControl.Services;
using ExpenseControl.Models;



namespace ExpenseControl.ViewModels
{
    public partial class DataTransferViewModel : ObservableObject
    {
        private readonly PersonRepository _repo;
        public DataTransferViewModel()
        {
            _repo = App.PersonRepo;
        }

        [ObservableProperty]
        string statusMessage;

        [RelayCommand]
        public async Task Export()
        {
            try
            {
                string fileName = $"despesas_exportadas_{DateTime.Now:yyyyMMdd+HHmmss}.json";
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

                await _repo.ExportExpensesToJsonAsync(filePath);
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
    }
}
