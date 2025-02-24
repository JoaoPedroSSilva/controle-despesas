namespace ExpenseControl {
    public partial class MainPage : ContentPage {

        List<ExpenseCategory> categories =
            [
                new ExpenseCategory("Mercado"),
                new ExpenseCategory("Gasolina"),
                new ExpenseCategory("Lanches")
            ];

        public MainPage() {
            InitializeComponent();
            LoadPickerCategory();

        }

        private void LoadPickerCategory()
        {
            pickerCategory.ItemsSource = categories;
            pickerCategory.ItemDisplayBinding = new Binding("Name");
        }

        /*
        private void OnPickerSelectedIndexChanged(object sender, EventArgs e) {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            DisplayAlert("Alert", "Index selected: " + selectedIndex, "OK");

            if (selectedIndex != -1) {
                CategoryNameLabel.Text = picker.Items[selectedIndex];
            }
        }*/

        private async void OnAddCategoryClicked(object sender, EventArgs e) {
            string newCategory = await DisplayPromptAsync("Adicionar categoria", "Digite a nova categoria.", "Adicionar", "Cancelar");
            categories.Add(new ExpenseCategory(newCategory));
            LoadPickerCategory();
        }
    }
}
