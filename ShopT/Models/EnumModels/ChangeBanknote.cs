using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopT.Models.EnumModels
{
    public enum ChangeBanknote
    {
        fiveHundred = 500,
        thousand = 1000,
        twoThousand = 2000,
        fiveThousand = 5000
    }

    public class ChangeBanknoteDictionaries 
    {
        public static Dictionary<ChangeBanknote, string> ChangeBanknoteToString = new Dictionary<ChangeBanknote, string> 
        {
            { ChangeBanknote.fiveHundred, "500" },
            { ChangeBanknote.thousand, "1000" },
            { ChangeBanknote.twoThousand, "2000" },
            { ChangeBanknote.fiveThousand, "5000" }
        };

        public static Dictionary<string, ChangeBanknote> StringToChangeBanknote = new Dictionary<string, ChangeBanknote>
        {
            { "500", ChangeBanknote.fiveHundred },
            { "1000", ChangeBanknote.thousand },
            { "2000", ChangeBanknote.twoThousand },
            { "5000", ChangeBanknote.fiveThousand }
        };

        public static List<BanknotePair> AllBanknotes = new List<BanknotePair> 
        {
            new BanknotePair{ ChangeBanknote = null, StringBanknote = "Нет необходимости" },
            new BanknotePair{ ChangeBanknote = ChangeBanknote.fiveHundred, StringBanknote = "500" },
            new BanknotePair{ ChangeBanknote = ChangeBanknote.thousand, StringBanknote = "1000" },
            new BanknotePair{ ChangeBanknote = ChangeBanknote.twoThousand, StringBanknote = "2000" },
            new BanknotePair{ ChangeBanknote = ChangeBanknote.fiveThousand, StringBanknote = "5000" }
        };

        public class BanknotePair
        {
            public ChangeBanknote? ChangeBanknote { get; set; }
            public string StringBanknote { get; set; }
        }
    }
}
