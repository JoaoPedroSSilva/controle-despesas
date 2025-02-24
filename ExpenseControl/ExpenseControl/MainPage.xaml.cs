namespace ExpenseControl {
    public partial class MainPage : ContentPage {
        int count = 0;



        public MainPage() {
            InitializeComponent();
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e) {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            DisplayAlert("Alert", "Index selected: " + selectedIndex, "OK");

            if (selectedIndex != -1) {
                CategoryNameLabel.Text = picker.Items[selectedIndex];
            }
        }

        private void OnCounterClicked(object sender, EventArgs e) {
            count++;
            
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
