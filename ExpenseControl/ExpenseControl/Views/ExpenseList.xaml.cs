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
        currentMonthExpenses = currentMonthExpenses.OrderByDescending(exp => exp.Date).ToList();
        
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
        var button = sender as Button;
        if (button == null)
            return;

        var expense = button.BindingContext as ExpenseEntry;
        if (expense == null)
            return;

        bool confirm = await DisplayAlert("Confirma��o", "Deseja excluir esse lan�amento?", "Sim", "Cancelar");
        if (!confirm)
            return;

        await App.PersonRepo.DeleteExpense(expense.Id);

        LoadExpenseList();

        await DisplayAlert("Lan�amento Exclu�do", "Lan�amento exclu�do com sucesso.", "OK"); ;
    }



    // Listar lan�amentos de acordo com o m�s corrente



    // Selecionar filtros de lan�amentos para data, valor, categoria e descri��o

}