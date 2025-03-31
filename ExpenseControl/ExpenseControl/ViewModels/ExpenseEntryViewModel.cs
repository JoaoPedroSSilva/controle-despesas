using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpenseControl.ViewModels
{
    public class ExpenseEntryViewModel : ObservableObject
    {
        // Dados Necessários

        // Data
        private DateTime _selectedDateTime;

        public DateTime SelectedDateTime
        {
            get => _selectedDateTime;
            set => SetProperty(ref _selectedDateTime, value);
        }

        // Categoria




        // Valor

        // Descrição

        // Últimos lançamentos
    }
}
