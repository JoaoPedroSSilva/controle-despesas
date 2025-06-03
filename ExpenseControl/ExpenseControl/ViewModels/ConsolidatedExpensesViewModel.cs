using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseControl.Models;
using ExpenseControl.Services;
using System.Collections.ObjectModel;

namespace ExpenseControl.ViewModels
{
    public partial class ConsolidatedExpensesViewModel : ObservableObject
    {
        private readonly PersonRepository _repo;

        public ConsolidatedExpensesViewModel()
        {
            _repo = App.PersonRepo;
            LoadAvaibleYears();
            LoadConsolidatedExpenses();
        }

        [ObservableProperty]
        int selectedYear = DateTime.Now.Year;

        [ObservableProperty]
        int selectedMonth = DateTime.Now.Month;

        [ObservableProperty]
        ObservableCollection<int> availableYears = new ObservableCollection<int>();

        [ObservableProperty]
        ObservableCollection<int> availableMonths = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        [ObservableProperty]
        ObservableCollection<ConsolidatedExpense> consolidatedExpenses = new();

        [ObservableProperty]
        ObservableCollection<ConsolidatedExpense> chartData = new();

        partial void OnSelectedYearChanged(int value) => LoadConsolidatedExpensesCommand.Execute(null);
        partial void OnSelectedMonthChanged(int value) => LoadConsolidatedExpensesCommand.Execute(null);

        [RelayCommand]
        public async Task InitializeAsync()
        {
            await LoadAvaibleYears();
            await LoadConsolidatedExpenses();
        }


        [RelayCommand]
        private async Task LoadConsolidatedExpenses()
        {
            try
            {
                List<ExpenseEntry> expenses = await _repo.GetAllExpenses();

                List<ExpenseEntry> filtered = expenses
                    .Where(e =>
                (SelectedYear == 0 || e.Date.Year == SelectedYear) &&
                (SelectedMonth == 0 || e.Date.Month == SelectedMonth)).ToList();

                List<ConsolidatedExpense> grouped = filtered
                    .GroupBy(e => new { e.Category, MonthYear = e.Date.ToString("MM/yyyy") })
                    .Select(g => new ConsolidatedExpense
                    {
                        Category = g.Key.Category,
                        MonthYear = g.Key.MonthYear,
                        TotalValue = g.Sum(e => e.Value)
                    })
                    .OrderByDescending(c => c.TotalValue)
                    .ToList();

                ConsolidatedExpenses = new ObservableCollection<ConsolidatedExpense>(grouped);


                List<ConsolidatedExpense> chartGrouped = filtered
                    .GroupBy(e => e.Category)
                    .Select(g => new ConsolidatedExpense
                    {
                        Category = g.Key,
                        TotalValue = g.Sum(e => e.Value)
                    })
                    .OrderByDescending(c => c.TotalValue)
                    .ToList();

                ChartData = new ObservableCollection<ConsolidatedExpense>(chartGrouped);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar dados: {ex.Message}", "OK");
            }
        }

        private async Task LoadAvaibleYears()
        {
            try
            {
                List<ExpenseEntry> expenses = await _repo.GetAllExpenses();
                var years = expenses.Select(e => e.Date.Year).Distinct().OrderByDescending(y => y);
                AvailableYears = new ObservableCollection<int>(years);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar anos: {ex.Message}", "OK");
            }
        }
    }
}
