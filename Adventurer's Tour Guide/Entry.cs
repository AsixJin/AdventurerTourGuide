using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventurer_Tour_Guide
{
    public class Entry
    {
        public string Title;
        public string Description;
        public string Details;

        public Entry()
        {
            Title = "Null";
            Description = "N/A";
            Details = "This is an empty entry!\nThis was probably created in error. Either delete it or write over it.";
        }

        public Entry(string mTitle, string mDescription, string mDetails)
        {
            Title = mTitle;
            Description = mDescription;
            Details = mDetails;
        }
    }
}
