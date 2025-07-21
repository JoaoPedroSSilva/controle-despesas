using SQLite;

namespace ExpenseControl.Models;

[Table("expenses")]
public class ExpenseEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime DateEntry { get; set; }
    public DateTime Date { get; set; }
    public string DateString { get; set; }
    public string Category { get; set; }
    public double Value { get; set; }
    public string Description { get; set; }
    public string Resume { get; set; }

    public string PaymentType { get; set; } = "Cartão";


    public ExpenseEntry(DateTime date, string category, double value, 
        string description, string paymentType = "Cartão")
    {
        DateEntry = DateTime.Now;
        Date = date;
        DateString = date.ToString("yyyy-MM-dd");
        Category = category;
        Value = value;
        Description = description;
        PaymentType = paymentType;
        Resume = ToString();
    }

    public ExpenseEntry() { }

    public override string ToString()
    {
        return Date.ToShortDateString() + "; R$" + Value.ToString("F2")
            + "; Categoria: "
            + Category 
            + "; Tipo: " + PaymentType + "; (" + Description + ").";
    }
}

