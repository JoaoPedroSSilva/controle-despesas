using SQLite;
using System.Text.Json;
using ExpenseControl.Models;

namespace ExpenseControl.Services
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
            await ApplyMigrationsAsync();
        }

        private async Task ApplyMigrationsAsync()
        {
            try
            {
                List<SQLiteConnection.ColumnInfo> tableInfo = await conn.GetTableInfoAsync("expenses");
                var hasPaymentType = tableInfo.Any(c => c.Name == "PaymentType");

                if (!hasPaymentType)
                    await conn.ExecuteAsync("ALTER TABLE expenses ADD COLUMN PaymentType TEXT DEFAULT 'Cartão'");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao aplicar migração: {ex.Message}";
            }
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

        public async Task<List<ExpenseEntry>> GetMonthExpenses(int month, int year)
        {
            try
            {
                await Init();
                string querryYear = year.ToString();
                string querryMonth = month.ToString("D2");
                string sqlQuerry = "SELECT * FROM expenses WHERE STRFTIME('%Y', DateString) = ? AND STRFTIME('%m', DateString) = ?";
                List<ExpenseEntry> currentMonthExpenses = await conn.QueryAsync<ExpenseEntry>(sqlQuerry, querryYear, querryMonth);
                return currentMonthExpenses;
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

        public async Task DeleteExpense(ExpenseEntry expense)
        {
            try
            {
                await Init();
                await conn.ExecuteAsync("DELETE FROM expenses WHERE id = ?", expense.Id);
                StatusMessage = "Lançamento excluído com sucesso.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao exluir: {ex.Message}";
            }
        }

        public async Task ExportExpensesToJsonAsync(string filePath, int month, int year)
        {
            try
            {
                await Init();
                List<ExpenseEntry> exportExpenses = await GetMonthExpenses(month, year);

                if (exportExpenses.Count == 0)
                {
                    throw new InvalidOperationException($"Não há despesas para serem exportadas do mês {month} de {year}");
                }

                string json = JsonSerializer.Serialize(exportExpenses, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });

                File.WriteAllText(filePath, json);
                StatusMessage = $"Dados exportados para: {filePath}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao exportar dados: {ex.Message}";
                throw new Exception(ex.Message);
            }
        }

        public async Task ImportExpensesFromJsonAsync(string filePath)
        {
            try
            {
                await Init();

                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Arquivo não encontrado.");

                string json = File.ReadAllText(filePath);
                List<ExpenseEntry> importedExpenses = JsonSerializer.Deserialize<List<ExpenseEntry>>(json);

                if (importedExpenses == null || !importedExpenses.Any())
                {
                    StatusMessage = "Nenhum despesa encontrada no arquivo.";
                    return;
                }

                foreach (ExpenseEntry expense in importedExpenses)
                {
                    expense.Id = 0;
                    await conn.InsertAsync(expense);
                }

                StatusMessage = $"Importação concluída com sucesso. Total de despesas importadas: {importedExpenses.Count}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao importar dados {ex.Message}";
            }
        }

        internal async Task<List<string>> GetExpensesPaymentsTypes()
        {
            try
            {
                await Init();
                var paymentsTypes = await conn.QueryAsync<ExpenseEntry>("SELECT DISTINCT PaymentType FROM expenses");
                return paymentsTypes.Select(x => x.PaymentType).ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Falha ao recuperar dados de tipos de pagamentos. {0}", ex.Message);
            }

            return new List<string>()
            {
                "Cartão",
                "Débito",
                "Dinheiro"
            };
        }
    }
}
