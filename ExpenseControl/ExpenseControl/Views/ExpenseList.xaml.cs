using ExpenseControl.Models;

namespace ExpenseControl.Views;

public partial class ExpenseList : ContentPage
{
	public ExpenseList()
	{
		InitializeComponent();
		LoadExpenseList();
	}

    private async void LoadExpenseList()
    {
        List<ExpenseEntry> expensesList = await App.PersonRepo.GetAllExpenses();
        expensesListView.ItemsSource = expensesList;
    }

    private async void OnRemoveExpenseClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Lançamento Excluído", "Lançamento excluído com sucesso.", "OK");
    }
}