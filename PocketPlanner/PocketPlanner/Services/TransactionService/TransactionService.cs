using AutoMapper;
using Microsoft.AspNetCore.Http;
using PocketPlanner.Data;
using PocketPlanner.Dtos.Transactions;
using PocketPlanner.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Text.RegularExpressions;
using System.Text;
using PocketPlanner.Services.CategoriesService;
using System.Text.Json;
using PocketPlanner.Services.TransactionService;

namespace PocketPlanner.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private static List<Transaction> _transactions = new List<Transaction>();
        private readonly ICategoriesService _categoriesService;

        public TransactionService(IMapper mapper, DataContext context, ICategoriesService categoriesService)
        {
            _mapper = mapper;
            _context = context;
            _transactions = new List<Transaction>();
            _categoriesService = categoriesService;
        }

        public async Task<ServiceResponse<List<Transaction>>> ProcessTransactions(TransactionDataDto transactionDataDto)
        {
            var serviceResponse = new ServiceResponse<List<Transaction>>();
            try
            {
                byte[] bytes = Convert.FromBase64String(transactionDataDto.Base64Data);
                string decodedData = Encoding.UTF8.GetString(bytes);

                _transactions = new List<Transaction>();

                using (StringReader reader = new StringReader(decodedData))
                {
                    string row;
                    while ((row = reader.ReadLine()) != null)
                    {
                        Transaction transaction = ParseTransaction(row);
                        if (transaction != null)
                        {
                            _transactions.Add(transaction);
                        }
                    }
                }
                ServiceResponse<List<GetCategoryDto>> categoriesResponse = await _categoriesService.GetAllCategories(); //works
                if (categoriesResponse != null && categoriesResponse.Data != null) // works, shows categories
                {
                    //List<Category> categories = _mapper.Map<List<Category>>(categoriesResponse.Data);// do not work - 'Missing type map configuration or unsupported mapping.'
                    List<GetCategoryDto> categories = categoriesResponse.Data;
                    foreach (Transaction transaction in _transactions)
                    {
                        foreach (GetCategoryDto category in categories)
                        {
                            category.Pattern = category.Pattern.TrimEnd('\r', '\n', '"').Replace("\b", "");

                            if (Regex.IsMatch(transaction.Payee, category.Pattern, RegexOptions.IgnoreCase) ||
                                Regex.IsMatch(transaction.Description, category.Pattern, RegexOptions.IgnoreCase))
                            {
                                transaction.Category = category.Name;
                                break;
                            }
                            else if (!Regex.IsMatch(transaction.Payee, category.Pattern, RegexOptions.IgnoreCase) ||
                                     !Regex.IsMatch(transaction.Description, category.Pattern, RegexOptions.IgnoreCase))
                            {
                                transaction.Category = "Other";
                            }
                        }
                    }
                }
                // await _context.Transactions.AddRangeAsync(_transactions); //databasse
                // await _context.SaveChangesAsync();

                serviceResponse.Data = _transactions;
                serviceResponse.Message = "Transactions processed successfully.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Error processing transactions";
                serviceResponse.Error = ex.ToString();
            }
            return serviceResponse;
        }

        //public async Task<ServiceResponse<List<GetTransactionDto>>> GetAllTransactions()
        //{
        //    var serviceResponse = new ServiceResponse<List<GetTransactionDto>>();
        //    try
        //    {
        //        // Map the list of transactions to GetTransactionDto
        //        var transactionDtos = _mapper.Map<List<GetTransactionDto>>(_transactions);

        //        serviceResponse.Data = transactionDtos;
        //        serviceResponse.Message = "Transactions retrieved successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResponse.Success = false;
        //        serviceResponse.Message = "Error retrieving transactions.";
        //        serviceResponse.Error = ex.ToString();
        //    }
        //    return serviceResponse;
        //}

        public void CategorizeTransactions(List<Transaction> transactions, List<Category> categories)
        {
            if (categories == null || categories.Count == 0)
                return;

            foreach (Transaction transaction in transactions)
            {
                foreach (Category category in categories)
                {
                    if (Regex.IsMatch(transaction.Payee, category.Pattern, RegexOptions.IgnoreCase) ||
                        Regex.IsMatch(transaction.Description, category.Pattern, RegexOptions.IgnoreCase))
                    {
                        transaction.Category = category.Name;
                        break;
                    }
                    else
                    {
                        transaction.Category = "Other";
                    }
                }
            }
        }

        private Transaction ParseTransaction(string row)
        {
            string[] parsedRow = CSVParser.Split(row);
            if (parsedRow.Length > 5 && !parsedRow[0].Contains("DOK NR"))
            {
                DateTime date;
                if (string.IsNullOrEmpty(parsedRow[1]) || !DateTime.TryParse(parsedRow[1], out date))
                {
                    date = DateTime.MinValue;
                }

                double amount;
                if (!double.TryParse(parsedRow[15].Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out amount))
                {
                    amount = 0.0;
                    return null;
                }

                Transaction transaction = new Transaction
                {
                    Id = ParseString(parsedRow[10]),
                    Date = date,
                    Amount = amount,
                    Payee = ParseString(parsedRow[4]),
                    PayeeIBAN = ParseString(parsedRow[6]),
                    InternalDescription = ParseString(parsedRow[12]),
                    IsCredit = ParseString(parsedRow[14]) == "C",
                    IBAN = ParseString(parsedRow[16]),
                    Description = ParseString(parsedRow[9]),
                    AccountCurrency = ParseString(parsedRow[17]),
                    Category = ParseString(parsedRow[18]),
                };

                return transaction;
            }
            else
            {
                return null;
            }
        }
        private Regex CSVParser = new Regex(";(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        private string ParseString(string element)
        {
            return element.Replace("\"", string.Empty);
        }
    }
}

