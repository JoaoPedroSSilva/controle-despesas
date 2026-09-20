using System.Globalization; 

namespace ExpenseControl.Converters
{
    public class CurrencyValidationBehavior : Behavior<Entry>
    {
        private static readonly CultureInfo PtBrCulture = new CultureInfo("pt-BR");

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnTextChanged;
        }

        protected void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is not Entry entry) return;

            string value = e.NewTextValue;

            if (string.IsNullOrEmpty(value)) return;

            string digitsOnly = new string(value.Where(char.IsDigit).ToArray());

            if (ulong.TryParse(digitsOnly, out ulong number))
            {
                decimal amount = number / 100m;
                string formatted = amount.ToString("N2", PtBrCulture);

                if (entry.Text != formatted)
                {
                    entry.Text = formatted;
                }
            }
            else if (value != "0,00")
            {
                entry.Text = "0,00";
            }
        }
    }
}
