namespace ExpenseControl
{
    internal class ExpenseEntry
    {
        public static int Id { get; private set; }
        public DateTime DateEntry { get; private set; }
        public DateTime Date { get; set; }
        public ExpenseCategory Category { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }

        public ExpenseEntry(DateTime date, ExpenseCategory category, double value, string description)
        {
            Id++;
            DateEntry = DateTime.Now;
            Date = date;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Value = value;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
