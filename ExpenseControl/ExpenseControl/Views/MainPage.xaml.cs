using System.Text;
using ExpenseControl.Models;

namespace ExpenseControl
{
    public partial class MainPage : ContentPage
    {
        List<string> categories;


        public MainPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            categories = await App.PersonRepo.GetExpensesCategories();
            LoadPickerCategory();
        }

        private void LoadPickerCategory()
        {
            categories.Sort(delegate (string x, string y)
            {
                if (x == null && y == null) return 0;
                else if (x == null) return -1;
                else if (y == null) return 1;
                else return x.CompareTo(y);
            });
            pickerCategory.ItemsSource = categories;
        }


        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            string newCategory = await DisplayPromptAsync("Adicionar categoria", "Digite a nova categoria.", "Adicionar", "Cancelar");
            if (string.IsNullOrWhiteSpace(newCategory))
                return;
            newCategory = newCategory.Trim();

            foreach (string category in categories)
            {
                if (category == newCategory)
                {
                    await DisplayAlert("Categoria já cadastrada!", "Nome de categoria já cadastrada.", "OK");
                    return;
                }
            }
            categories.Add(newCategory);

            pickerCategory.ItemsSource = null;
            pickerCategory.ItemsSource = categories;

            await DisplayAlert("Categoria adicionada!", "Nova categoria cadastrada.", "OK");
        }

        /*private void OnEntryValueTextChanged(object sender, EventArgs e)
        {
            var value = new StringBuilder();
            foreach (var c in entryValue.Text)
            {
                if (".,0123456789".Contains(c))
                {
                    value.Append(c);
                }
            }
            entryValue.Text = value.ToString();
        }

        private async void OnRecordDataClicked(object sender, EventArgs e)
        {

            DateTime entryDate = datePicker.Date;
            int selectedCategory = pickerCategory.SelectedIndex;
            string entryCategory = categories[selectedCategory];
            string? stringValue = entryValue.Text;
            string? description = entryDescription.Text;

            if (string.IsNullOrEmpty(entryCategory))
            {
                await DisplayAlert("Categoria inválida!", "Favor selecione uma categoria válida.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                await DisplayAlert("Valor inválido!", "Favor digite um valor válido.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Descrição inválida!", "Favor digite uma descrição.", "OK");
                return;
            }

            double value;
            try
            {
                value = double.Parse(stringValue);
            }
            catch (Exception)
            {
                await DisplayAlert("Valor inválido!", "Favor digite um valor válido.", "OK");
                return;
            }

            ExpenseEntry expense = new ExpenseEntry(entryDate, entryCategory, value, description);
            await App.PersonRepo.AddNewExpense(expense);

            entryValue.Text = "";
            entryDescription.Text = "";

            await RefreshLastsExpenses();
        }

        private async Task RefreshLastsExpenses()
        {
            int numberOfLastsExpenses = 4;
            List<ExpenseEntry> expensesList = await App.PersonRepo.GetLastsExpenses(numberOfLastsExpenses);
            expensesListView.ItemsSource = expensesList;
        }

        private async void OnRemoveExpenseClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var expense = button.BindingContext as ExpenseEntry;
            if (expense == null)
                return;

            bool confirm = await DisplayAlert("Confirmação", "Deseja excluir esse lançamento?", "Sim", "Cancelar");
            if (!confirm)
                return;

            await App.PersonRepo.DeleteExpense(expense.Id);

            await RefreshLastsExpenses();

            await DisplayAlert("Lançamento Excluído", "Lançamento excluído com sucesso.", "OK");
        }*/
    }
}
