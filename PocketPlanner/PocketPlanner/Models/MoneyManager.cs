//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Newtonsoft.Json;
//using System.Threading.Tasks;
//using PocketPlanner;
//using Microsoft.VisualBasic;

//namespace PocketPlanner.Models
//{
//    public class MoneyManager
//    {
//        public List<Transaction> transactions;
//        public List<Category> categories;
//        public List<Budget> budgets;

//        public async Task ProcessTransactions()
//        {
//            Regex CSVParser = new Regex(";(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
//            transactions = new List<Transaction>();
//            foreach (string row in File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Data/Išrašas.csv"))
//            {
//                Transaction transaction = ParseTransaction(row);
//                if (transaction != null)
//                {
//                    transactions.Add(transaction);
//                }
//            }
//            string jsonTransactions = JsonConvert.SerializeObject(transactions, Formatting.Indented);
//            string pathTransactionsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Transactions.json");
//            await File.WriteAllTextAsync(pathTransactionsFile, jsonTransactions);
//        }

//        private Regex CSVParser = new Regex(";(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
//        public Transaction ParseTransaction(string row)
//        {
//            string[] parsedRow = CSVParser.Split(row);
//            if (parsedRow.Length > 5 && !parsedRow[0].Contains("DOK NR"))
//            {
//                Transaction transaction = new Transaction
//                {
//                    Id = ParseString(parsedRow[10]),
//                    Date = DateTime.Parse(parsedRow[1]),
//                    Amount = double.Parse(parsedRow[15].Replace(",", ".")),
//                    Payee = ParseString(parsedRow[4]),
//                    PayeeIBAN = ParseString(parsedRow[6]),
//                    InternalDescription = ParseString(parsedRow[12]),
//                    IsCredit = ParseString(parsedRow[14]) == "C",
//                    IBAN = ParseString(parsedRow[16]),
//                    Description = ParseString(parsedRow[9]),
//                    AccountCurrency = ParseString(parsedRow[17]),
//                    Category = ParseString(parsedRow[18]),
//                };
//                return transaction;
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private string ParseString(string element)
//        {
//            return element.Replace("\"", string.Empty);
//        }

//        public async void CategorizedTransactions(List<Category> categories)
//        {
//            if (categories == null || categories.Count == 0)
//            {
//                return; //exit method, if categories == null;
//            }
//            foreach (Transaction transaction in transactions)
//            {
//                foreach (Category category in categories)
//                {
//                    if (Regex.IsMatch(transaction.Payee, category.Pattern, RegexOptions.IgnoreCase) ||
//                        Regex.IsMatch(transaction.Description, category.Pattern, RegexOptions.IgnoreCase))
//                    {
//                        transaction.Category = category.Name;
//                        break;
//                    }
//                    else if (!Regex.IsMatch(transaction.Payee, category.Pattern, RegexOptions.IgnoreCase) ||
//                             !Regex.IsMatch(transaction.Description, category.Pattern, RegexOptions.IgnoreCase))
//                    {
//                        transaction.Category = "Other";
//                    }
//                }
//            }
//            string jsonTransactions = JsonConvert.SerializeObject(transactions, Formatting.Indented);
//            string pathTransactionsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Transactions.json");
//            await File.WriteAllTextAsync(pathTransactionsFile, jsonTransactions);
//        }
//        public List<Transaction> GetTransactions()
//        {
//            return transactions;
//        }

//        public List<Category> AddCategory(Category category)
//        {
//            if (categories != null)
//            {
//                int nextId = categories.Count + 1;
//                category.Id = nextId.ToString();
//                categories.Add(category);
//            }
//            else if (categories == null)
//            {
//               categories = new List<Category>();
//            }
//            SaveCategoriesToJson();
//            return categories;
//        }

//        public void SaveCategoriesToJson()
//        {
//            string jsonCategories = JsonConvert.SerializeObject(categories, Formatting.Indented);
//            string pathCategoriesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Categories.json");
//            File.WriteAllText(pathCategoriesFile, jsonCategories);
//        }

//        public List<Category> LoadCategories()
//        {
//            string jsonCategories = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Categories.json"));
//            categories = JsonConvert.DeserializeObject<List<Category>>(jsonCategories);
//            return categories;
//        }

//        public void UpdateCategory(string id, Category updatedCategory)
//        {
//            Category category = categories.FirstOrDefault(categories => categories.Id == id);
//            if (category != null)
//            {
//                category.Name = updatedCategory.Name;
//                category.Pattern = updatedCategory.Pattern;
//                SaveCategoriesToJson();
//            }
//        }

//        public void DeleteCategory(string id)
//        {
//            Category category = categories.FirstOrDefault(categories => categories.Id == id);
//            if (category != null)
//            {
//                categories.Remove(category);
//                SaveCategoriesToJson();
//            }
//        }

//        public void UpdateBudget(string id, Budget updatedBudget)
//        {
//            Budget budget = budgets.FirstOrDefault(budgets => budgets.Id == id);
//            if (budget != null)
//            {
//                budget.Type = updatedBudget.Type;
//                budget.Amount = updatedBudget.Amount;
//                budget.Date = updatedBudget.Date;
//                SaveBudgetsToJson();
//            }
//        }

//        public void DeleteBudget(string Id)
//        {
//            Budget budget = budgets.FirstOrDefault(budgets => budgets.Id == Id);
//            if (budget != null)
//            {
//                budgets.Remove(budget);
//                SaveBudgetsToJson();
//            }
//        }

//        public List<Budget> LoadBudgets()
//        {
//            string jsonBudgets = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Budget.json"));
//            budgets = JsonConvert.DeserializeObject<List<Budget>>(jsonBudgets);
//            return budgets;
//        }

//        public void SaveBudgetsToJson()
//        {
//            string jsonBudgets = JsonConvert.SerializeObject(budgets, Formatting.Indented);
//            string pathBudgetsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Budget.json");
//            File.WriteAllText(pathBudgetsFile, jsonBudgets);
//        }

//        public List<Budget> AddBudget(Budget budget)
//        {
//            if (budgets == null)
//            {
//                budgets = new List<Budget>();
//            }
//            else if (budgets != null)
//            {
//                int budgetId = budgets.Count + 1;
//                budget.Id = budgetId.ToString();
//                budgets.Add(budget);
//            }
//            SaveBudgetsToJson();
//            return budgets;
//        }
//    }
//}
