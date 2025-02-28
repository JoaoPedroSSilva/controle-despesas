namespace ExpenseControl
{
    internal class ExpenseEntry
    {
        public int Id { get; private set; }
        public DateTime DateEntry { get; private set; }
        public DateTime Date { get; set; }
        public ExpenseCategory Category { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }

        private static int LastId = 0; 



        public ExpenseEntry(DateTime date, ExpenseCategory category, double value, string description)
        {
            Id = ++LastId;
            DateEntry = DateTime.Now;
            Date = date;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Value = value;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public static int GetLastId()
        {
            return LastId;
        }

        public override string ToString()
        {
            return "Data: " + Date.ToShortDateString() + "; Categoria: "
                    + Category.Name + "; Valor: R$" 
                    + Value.ToString("F2") 
                    + "; (" + Description + ").";
        }
    }
}
