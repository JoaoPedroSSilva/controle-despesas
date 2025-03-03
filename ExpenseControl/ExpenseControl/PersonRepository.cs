using ExpenseControl.Models;
using SQLite;

namespace ExpenseControl
{
    public class PersonRepository
    {
        string _dbPath;

        public string StatusMessage { get; set; }

        private SQLiteConnection conn;

        private void Init()
        {
            if (conn != null)
            {
                return;
            }
            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<ExpenseEntry>();

        }

        public PersonRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void AddNewExpense(ExpenseEntry expense)
        {
            int result = 0;
            try
            {
                Init();

                if (expense == null)
                    throw new Exception("Valid expense required");

                result = conn.Insert(expense);

                StatusMessage = string.Format("{0} lançamento gravado (Despesa: {1})",
                    result, expense.Description);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Falha ao adicidionar {0}. Erro: {1}",
                    expense.Description, ex.Message);
            }
        }

        public List<ExpenseEntry> GetAllExpenses()
        {
            try
            {
                Init();
                return conn.Table<ExpenseEntry>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<ExpenseEntry>();
        }
    }
}
