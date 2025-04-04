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
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;
        List<ExpenseEntry> currentMonthExpenses = await App.PersonRepo.GetMonthExpenses(month, year);

        labelTotalSpent.Text = $"Total de gastos: {SumExpenses(currentMonthExpenses):C}";
        expensesListView.ItemsSource = currentMonthExpenses;
    }

    private double SumExpenses(List<ExpenseEntry> expenses)
    {
        double sum = 0;
        foreach(ExpenseEntry expense in expenses)
        {
            sum += expense.Value;
        }
        return sum;
    }

    private async void OnRemoveExpenseClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Lan�amento Exclu�do", "Lan�amento exclu�do com sucesso.", "OK");
    }



    // Listar lan�amentos de acordo com o m�s corrente



    // Selecionar filtros de lan�amentos para data, valor, categoria e descri��o

}