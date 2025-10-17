using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseControl.Models;
using ExpenseControl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseControl.ViewModels
{
    public partial class ExpenseListViewModel : ObservableObject
    {
        private readonly PersonRepository _repo;

        public ExpenseListViewModel()
        {
            _repo = App.PersonRepo;
            _ = LoadFiltersAsync();
        }

        [ObservableProperty]
        string statusMessage;

        [ObservableProperty]
        int selectedYear = DateTime.Now.Year;

        [ObservableProperty]
        int selectedMonth = DateTime.Now.Month;

        [ObservableProperty]
        string searchDescription = string.Empty;

        [ObservableProperty]
        string? selectedCategory;

        [ObservableProperty]
        string? selectedPaymentType;

        [ObservableProperty]
        double? minValue;

        [ObservableProperty]
        double? maxValue;

        public ObservableCollection<int> availableYears { get; } =
            new ObservableCollection<int>(Enumerable.Range(DateTime.Now.Year -6, 8));

        public ObservableCollection<int> availableMonths { get; } = 
            new ObservableCollection<int>(Enumerable.Range(1, 12));

        [ObservableProperty]
        ObservableCollection<string> availableCategories = new();

        [ObservableProperty]
        ObservableCollection<string> availablePaymentTypes = new();

        [ObservableProperty]
        ObservableCollection<ExpenseEntry> expenses = new();

        [ObservableProperty]
        double totalSpent;

        [RelayCommand]
        private async Task LoadExpenses()
        {
            try
            {
                List<ExpenseEntry> expenses = await _repo.GetMonthExpenses(SelectedMonth, SelectedYear);

                List<ExpenseEntry> filtered = expenses.Where(e =>
                    (e.Date.Year == SelectedYear) &&
                    (e.Date.Month == SelectedMonth) &&
                    (string.IsNullOrEmpty(SearchDescription) ||
                    e.Description.Contains(SearchDescription, StringComparison.OrdinalIgnoreCase)) &&
                    (SelectedCategory == "Todas" || string.IsNullOrEmpty(SelectedCategory) || e.Category == SelectedCategory) &&
                    (SelectedPaymentType == "Todas" || string.IsNullOrEmpty(SelectedPaymentType) || e.PaymentType == SelectedPaymentType) &&
                    (!MinValue.HasValue || e.Value >= MinValue) &&
                    (!MaxValue.HasValue || e.Value <= MaxValue))
                    .OrderByDescending(e => e.Date).ToList();

                if (!filtered.Any())
                {
                    Expenses.Clear();
                    TotalSpent = 0.0;
                    StatusMessage = "Nenhuma despesa encontrada para os filtros selecionados.";
                    return;
                }

                Expenses = new ObservableCollection<ExpenseEntry>(filtered);
                TotalSpent = filtered.Sum(e => e.Value);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar despesas: {ex.Message}", "OK");
            }
        }


        [RelayCommand]
        private async Task DeleteExpense(ExpenseEntry expense)
        {
            if (expense == null) return;

            bool confirm = await Shell.Current
                .DisplayAlert("Confirmação", "Deseja excluir esse lançamento?", "Sim", "Cancelar");

            if (!confirm) return;

            await _repo.DeleteExpense(expense);
            await LoadExpenses();
            await Shell.Current.DisplayAlert("Sucesso", "Lançamento excluído com sucesso.", "Ok");
        }

        partial void OnSelectedYearChanged(int value) => _ = LoadFiltersAsync();
        partial void OnSelectedMonthChanged(int value) => _ = LoadFiltersAsync();


        private async Task LoadFiltersAsync()
        {
            Task<List<string>> categoriesTask = _repo.GetCategoriesByPeriod(SelectedMonth, SelectedYear);
            Task<List<string>> paymentTypesTask = _repo.GetPaymentTypesByPeriod(SelectedMonth, SelectedYear);

            await Task.WhenAll(categoriesTask, paymentTypesTask);

            List<string> categories = await categoriesTask;
            List<string> paymentTypes = await paymentTypesTask;

            categories.Insert(0, "Todas");
            paymentTypes.Insert(0, "Todas");

            AvailableCategories = new ObservableCollection<string>(categories);
            AvailablePaymentTypes = new ObservableCollection<string>(paymentTypes);

            SelectedCategory = "Todas";
            SelectedPaymentType = "Todas";
        }
    }
}