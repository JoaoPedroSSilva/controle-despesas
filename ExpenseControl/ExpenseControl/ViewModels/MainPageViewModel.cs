using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseControl.Models;
using ExpenseControl.Services;
using System.Collections.ObjectModel;

namespace ExpenseControl.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly PersonRepository _repo;

        public MainPageViewModel()
        {
            _repo = App.PersonRepo;
            LoadData();
        }

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Today;

        [ObservableProperty]
        private string selectedCategory;

        [ObservableProperty]
        private string entryValue;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private ObservableCollection<string> categories = new();

        [ObservableProperty]
        private ObservableCollection<ExpenseEntry> expenses = new();

        [RelayCommand]
        private async Task AddCategory()
        {

        }

        [RelayCommand]
        private async Task SaveExpense()
        {
            if (string.IsNullOrWhiteSpace(selectedCategory) ||
                string.IsNullOrWhiteSpace(entryValue) ||
                string.IsNullOrWhiteSpace(description))
            {
                StatusMessage = "Todos os campos devem ser preenchidos.";
                return;
            }

            if (!double.TryParse(entryValue, out double value))
            {
                StatusMessage = "Valor inválido.";
                return;
            }

            ExpenseEntry expense = new(selectedDate, selectedCategory, value, description);
            await _repo.AddNewExpense(expense);
            StatusMessage = _repo.StatusMessage;

            EntryValue = string.Empty;
            Description = string.Empty;

            await LoadLastExpenses();
        }

        [RelayCommand]
        private async Task RemoveExpense(ExpenseEntry expense)
        {
            if (expense == null)
                return;

            await _repo.DeleteExpense(expense);
            Expenses.Remove(expense);
        }

        private async void LoadData()
        {
            var catList = await _repo.GetExpensesCategories();
            Categories = new ObservableCollection<string>(catList);
            await LoadLastExpenses();
        }

        private async Task LoadLastExpenses()
        {
            int numberOfExpenses = 4;
            var list = await _repo.GetLastsExpenses(numberOfExpenses);
            Expenses = new ObservableCollection<ExpenseEntry>(list.OrderByDescending(e => e.Date));
        }
    }
}
