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
        
    }
}
