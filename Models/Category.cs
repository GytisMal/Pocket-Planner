using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PocketPlanner
{
    [Serializable]
    public class Category
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string Pattern { get; set; }
    }
}
