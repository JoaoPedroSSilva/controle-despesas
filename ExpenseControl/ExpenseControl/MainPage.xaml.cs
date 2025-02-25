﻿using System.Text;

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

        public MainPage()
        {
            InitializeComponent();
            LoadPickerCategory();
        }

        private void LoadPickerCategory()
        {
            pickerCategory.ItemsSource = categories;
            pickerCategory.ItemDisplayBinding = new Binding("Name");
        }


        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            string newCategory = await DisplayPromptAsync("Adicionar categoria", "Digite a nova categoria.", "Adicionar", "Cancelar");
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
            string date = datePicker.Date.ToShortDateString();
            int selectedCategory = pickerCategory.SelectedIndex;
            string? category = selectedCategory == -1 ? null : categories[selectedCategory].Name;
            string? value = entryValue.Text;
            string description = entryDescription.Text;

            if (string.IsNullOrEmpty(category))
            {
                await DisplayAlert("Categoria inválida!", "Favor selecione uma categoria válida.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                await DisplayAlert("Valor inválido!", "Favor digite um valor válido.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Descrição inválida!", "Favor digite uma descrição.", "OK");
                return;
            }

            labelResume.Text = "Data: " + date + "; Categoria: "
                    + category + "; Valor: R$" + value + "; (" + description + ")";
        }
    }
}
