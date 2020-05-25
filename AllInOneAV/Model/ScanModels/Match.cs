using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class Match
    {
        public string AvID { get; set; }
        public string AvName { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        //public DateTime CreateTime { get; set; }
        public int MatchAVId { get; set; }
    }

    public class MatchComparer : IEqualityComparer<Match>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Match x, Match y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Location == y.Location && x.AvID == y.AvID && x.Name == y.Name && x.AvName == y.AvName;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Match match)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(match, null)) return 0;

            int hashMatchLocation = string.IsNullOrEmpty(match.Location) ? 0 : match.Location.GetHashCode();
            int hashMatchName = string.IsNullOrEmpty(match.Name) ? 0 : match.Name.GetHashCode();
            int hashMatchAvName = string.IsNullOrEmpty(match.AvName) ? 0 : match.AvName.GetHashCode();
            int hashMatchAvid = string.IsNullOrEmpty(match.AvID) ? 0 : match.AvID.GetHashCode();

            //Calculate the hash code for the product.
            return hashMatchLocation ^ hashMatchName ^ hashMatchAvid ^ hashMatchAvName;
        }
    }
}
