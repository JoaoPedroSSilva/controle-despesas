using ExpenseControl.ViewModels;

namespace ExpenseControl.Views;

public partial class ConsolidatedExpenses : ContentPage
{
	public ConsolidatedExpenses()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is ConsolidatedExpensesViewModel vm)
		{
			await vm.InitializeAsync();
		}
	}
}