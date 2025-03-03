namespace ExpenseControl.Models
{
    public class ExpenseCategory
    {
        public string Name { get; set; }

        public ExpenseCategory(string name)
        {
            Name = name;
        }
    }
}
