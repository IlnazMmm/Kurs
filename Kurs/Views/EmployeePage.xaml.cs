using Kurs.Models;

namespace Kurs.Views;

public partial class EmployeePage : ContentPage
{
    public EmployeePage()
    {
        InitializeComponent();
    }
    private void OnSalaryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            // Разрешаем только цифры и запятую
            string newText = new string(e.NewTextValue.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());

            if (newText != e.NewTextValue)
                entry.Text = newText;
        }
    }
}
