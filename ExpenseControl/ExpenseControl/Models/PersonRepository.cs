using SQLite;

namespace ExpenseControl.Models
{
    public class PersonRepository
    {
        string _dbPath;

        public string StatusMessage { get; set; }

        private SQLiteAsyncConnection conn;

        private async Task Init()
        {
            if (conn != null)
            {
                return;
            }
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<ExpenseEntry>();
        }

        public PersonRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task AddNewExpense(ExpenseEntry expense)
        {
            int result = 0;
            try
            {
                await Init();

                if (expense == null)
                    throw new Exception("Valid expense required");

                result = await conn.InsertAsync(expense);

                StatusMessage = string.Format("Lançamento gravado (Despesa: {0})",
                    expense.Description);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Falha ao adicidionar {0}. Erro: {1}",
                    expense.Description, ex.Message);
            }
        }

        public async Task<List<ExpenseEntry>> GetAllExpenses()
        {
            try
            {
                await Init();
                return await conn.Table<ExpenseEntry>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Falha ao recuperar dados. {0}", ex.Message);
            }

            return new List<ExpenseEntry>();
        } 

        public async Task<List<ExpenseEntry>> GetLastsExpenses(int limit) 
        {
            try
            {
                await Init();
                List<ExpenseEntry> lastsExpenses = await conn.QueryAsync<ExpenseEntry>(
                    $"SELECT * FROM expenses ORDER BY id DESC LIMIT {limit}");
                return lastsExpenses;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Falha ao recuperar dados. {0}", ex.Message);
            }

            return new List<ExpenseEntry>();
        }

        public async Task<List<string>> GetExpensesCategories()
        {
            try
            {
                await Init();
                var categories = await conn.QueryAsync<ExpenseEntry>("SELECT DISTINCT Category FROM expenses");
                return categories.Select(x => x.Category).ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Falha ao recuperar dados. {0}", ex.Message);
            }

            return new List<string>() 
            { 
                "Mercado",
                "Gasolina",
                "Condomínio",
                "Lanches" 
            };
        }

        /* public void RemoveExpense(ExpenseEntry expense)
        {
            int result = 0;
            try
            {
                Init();

                if (expense == null)
                    throw new Exception("Valid expense required");

                result = conn.Delete(expense.Id);

                StatusMessage = string.Format("{0} lançamento removido (Despesa: {1})",
                    result, expense.Description);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Falha ao remover {0}. Erro: {1}",
                    expense.Description, ex.Message);
            }
        } */
    }
}
