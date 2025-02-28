using System.Text;
using System.Text.Json;

namespace ExpenseControl
{
    public partial class MainPage : ContentPage
    {

        List<ExpenseCategory> categories =
            [
                new ExpenseCategory("Mercado"),
                new ExpenseCategory("Gasolina"),
                new ExpenseCategory("Lanches")
            ];

        
        List<ExpenseEntry> expensesList = LoadExpensesFromJson("C:\\Users\\jp_sa\\Downloads\\User Expenses.json");

        public MainPage()
        {
            InitializeComponent();
            LoadPickerCategory();
        }

        private void LoadPickerCategory()
        {
            categories.Sort(delegate (ExpenseCategory x, ExpenseCategory y)
            {
                if (x.Name == null && y.Name == null) return 0;
                else if (x.Name == null) return -1;
                else if (y.Name == null) return 1;
                else return x.Name.CompareTo(y.Name);
            });
            pickerCategory.ItemsSource = categories;
            pickerCategory.ItemDisplayBinding = new Binding("Name");
        }


        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            string newCategory = await DisplayPromptAsync("Adicionar categoria", "Digite a nova categoria.", "Adicionar", "Cancelar");
            if (string.IsNullOrWhiteSpace(newCategory))
                return;
            foreach (ExpenseCategory category in categories)
            {
                if (category.Name == newCategory)
                {
                    await DisplayAlert("Categoria já cadastrada!", "Nome de categoria já cadastrada.", "OK");
                    return;
                }
            }
            categories.Add(new ExpenseCategory(newCategory));
            LoadPickerCategory();
            await DisplayAlert("Categoria adicionada!", "Nova categoria cadastrada.", "OK");
        }

        private void OnEntryValueTextChanged(object sender, EventArgs e)
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
            string dateString = entryDate.ToShortDateString();
            int selectedCategory = pickerCategory.SelectedIndex;
            ExpenseCategory entryCategory = categories[selectedCategory];
            string? stringCategory = selectedCategory == -1 ? null : entryCategory.Name;
            string? stringValue = entryValue.Text;
            string? description = entryDescription.Text;

            if (string.IsNullOrEmpty(stringCategory))
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
            expensesList.Add(expense);
            string fileName = "C:\\Users\\jp_sa\\Downloads\\User Expenses.json";

            SaveExpensesToJson(expensesList, fileName);

            entryValue.Text = "";
            entryDescription.Text = "";

            expensesListView.ItemsSource = expensesList;
        }

        private static void SaveExpensesToJson(List<ExpenseEntry> expenses, string fileName)
        {
            var serializedData = JsonSerializer.Serialize(expenses);
            File.WriteAllText(fileName, serializedData);
        }

        private static List<ExpenseEntry> LoadExpensesFromJson(string fileName)
        {
            try
            {
                var rawData = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<List<ExpenseEntry>>(rawData);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex);
            }
        }
    }
}
