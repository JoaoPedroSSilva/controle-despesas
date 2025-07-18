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
            LoadAvaibleYears();
            LoadAvaibleCategories();
            LoadExpenses();
        }

        [ObservableProperty]
        int selectedYear = DateTime.Now.Year;

        [ObservableProperty]
        int selectedMonth = DateTime.Now.Month;

        [ObservableProperty]
        string searchDescription = string.Empty;

        [ObservableProperty]
        string? selectedCategory;

        [ObservableProperty]
        double? minValue;

        [ObservableProperty]
        double? maxValue;

        [ObservableProperty]
        ObservableCollection<int> availableYears = new();

        public ObservableCollection<int> availableMonths { get; } = 
            new ObservableCollection<int>(Enumerable.Range(1, 12));

        [ObservableProperty]
        ObservableCollection<string> availableCategories = new();

        [ObservableProperty]
        ObservableCollection<ExpenseEntry> expenses = new();

        [ObservableProperty]
        double totalSpent;

        [RelayCommand]
        private async Task LoadExpenses()
        {
            try
            {
                List<ExpenseEntry> allExpenses = await _repo.GetAllExpenses();

                List<ExpenseEntry> filtered = allExpenses.Where(e =>
                    (e.Date.Year == SelectedYear) &&
                    (e.Date.Month == SelectedMonth) &&
                    (string.IsNullOrEmpty(SearchDescription) ||
                    e.Description.Contains(SearchDescription, StringComparison.OrdinalIgnoreCase)) &&
                    (SelectedCategory == "Todas" || string.IsNullOrEmpty(SelectedCategory) || e.Category == SelectedCategory) &&
                    (!MinValue.HasValue || e.Value >= MinValue) &&
                    (!MaxValue.HasValue || e.Value <= MaxValue))
                    .OrderByDescending(e => e.Date).ToList();

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

        private async void LoadAvaibleYears()
        {
            List<ExpenseEntry> allExpenses = await _repo.GetAllExpenses();
            var years = allExpenses.Select(e => e.Date.Year).Distinct().OrderByDescending(y => y);
            AvailableYears = new ObservableCollection<int>(years);
        }

        private async void LoadAvaibleCategories()
        {
            List<ExpenseEntry> allExpenses = await _repo.GetAllExpenses();
            var categories = allExpenses.Select(e => e.Category).Distinct().OrderBy(c => c).ToList();

            categories.Insert(0, "Todas");

            AvailableCategories = new ObservableCollection<string>(categories);
            SelectedCategory = "Todas";
        }
    }
}
