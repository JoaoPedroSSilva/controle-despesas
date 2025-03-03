using SQLite;

namespace ExpenseControl.Models;

[Table("expenses")]
public class ExpenseEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime DateEntry { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
    public double Value { get; set; }
    public string Description { get; set; }
    public string Resume { get; set; }


    public ExpenseEntry(DateTime date, string category, double value, string description)
    {
        DateEntry = DateTime.Now;
        Date = date;
        Category = category;
        Value = value;
        Description = description;
        Resume = ToString();
    }

    public ExpenseEntry() { }

    public override string ToString()
    {
        return "Data: " + Date.ToShortDateString() + "; Categoria: "
                + Category + "; Valor: R$"
                + Value.ToString("F2")
                + "; (" + Description + ").";
    }
}

