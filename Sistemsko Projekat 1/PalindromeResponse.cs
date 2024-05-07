using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistemsko_Projekat_1
{
     struct PalindromeResponse
     {
        public PalindromeResponse(string[] files, int[] palindromes) {
            Files = files;
            Palindromes = palindromes;
            LastRequested = DateTime.Now;
        }
        public PalindromeResponse(PalindromeResponse oldResponse)
        {
            Files = oldResponse.Files;
            Palindromes = oldResponse.Palindromes;
            LastRequested = oldResponse.LastRequested;
        }

        public string[] Files { get; set; } 
        public int[] Palindromes { get; set; }  
        public DateTime LastRequested { get; set; }
     }
}   
