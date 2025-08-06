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
        private string selectedPaymentType;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private ObservableCollection<string> categories = new();

        [ObservableProperty]
        private ObservableCollection<ExpenseEntry> expenses = new();

        [ObservableProperty]
        private ObservableCollection<string> paymentTypes = new();

        [RelayCommand]
        private async Task AddCategory()
        {
            string newCategory = await Shell.Current.DisplayPromptAsync("Adicionar categoria", "Digite a nova categoria.", "Adicionar", "Cancelar");
            if (string.IsNullOrWhiteSpace(newCategory))
                return;
            newCategory = newCategory.Trim();

            foreach (string category in Categories)
            {
                if (category == newCategory)
                {
                    await Shell.Current.DisplayAlert("Categoria já cadastrada!", "Nome de categoria já cadastrada.", "OK");
                    return;
                }
            }
            Categories.Add(newCategory);
            SelectedCategory = newCategory;

            await Shell.Current.DisplayAlert("Categoria adicionada!", "Nova categoria cadastrada.", "OK");
        }

        [RelayCommand]
        private async Task AddPaymentType()
        {
            string newPaymentType = await Shell.Current.DisplayPromptAsync("Adicionar forma de pagamento",
                "Digite a nova forma de pagamento.", "Adicionar", "Cancelar");
            if (string.IsNullOrWhiteSpace(newPaymentType))
                return;
            newPaymentType = newPaymentType.Trim();

            foreach (string type in PaymentTypes)
            {
                if (type == newPaymentType)
                {
                    await Shell.Current.DisplayAlert("Forma de pagamento já cadastrada!", 
                        "Nome de forma de pagamento já cadastrada.", "OK");
                    return;
                }
            }
            PaymentTypes.Add(newPaymentType);
            SelectedPaymentType = newPaymentType;

            await Shell.Current.DisplayAlert("Forma de pagamento adicionada!", "Nova forma de pagamento cadastrada.", "OK");
        }

        [RelayCommand]
        private async Task SaveExpense()
        {
            if (string.IsNullOrWhiteSpace(SelectedCategory) ||
                string.IsNullOrWhiteSpace(EntryValue) ||
                string.IsNullOrWhiteSpace(Description))
            {
                StatusMessage = "Todos os campos devem ser preenchidos.";
                return;
            }

            if (!double.TryParse(EntryValue, out double value))
            {
                StatusMessage = "Valor inválido.";
                return;
            }

            Description = Description.Trim();
            ExpenseEntry expense = new(SelectedDate, SelectedCategory, value, Description, SelectedPaymentType);
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
            List<string> categList = await _repo.GetExpensesCategories();
            Categories = new ObservableCollection<string>(categList.OrderBy(c => c));

            List<string> paymentTypeList = await _repo.GetExpensesPaymentsTypes();
            PaymentTypes = new ObservableCollection<string>(paymentTypeList.OrderBy(p => p));

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
