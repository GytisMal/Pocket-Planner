using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Newtonsoft.Json;


namespace PocketPlanner.Models
{
    [Serializable]
    public class Transaction
    {
        [JsonProperty]
        public string Id { get; set; } //- for every transactions need to create a unique Id;
        public string IBAN { get; set; } //- user bank account and use it for transactions;
        public DateTime Date { get; set; }   //- the date of the transaction date;
        public double Amount { get; set; }  //- money spent, double because num could be 33,33;
        public bool IsCredit { get; set; }  //(true/false) - from current balance - credit;
        public string Payee { get; set; }  // the place or the people from transactions for categorization;
        public string PayeeIBAN { get; set; }
        public string Description { get; set; } // - purpose of payment
        public string InternalDescription { get; set; } // - purpose of payment
        public string AccountCurrency { get; set; } // - Eur, NOK, USD...
        public string Category { get; set; }
    }
}
