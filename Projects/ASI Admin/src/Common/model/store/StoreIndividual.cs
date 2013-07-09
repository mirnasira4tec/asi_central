using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreIndividual
    {
        public const char NAME_SEPARATOR = ' ';

        public int Id { get; set; }
        public bool IsPrimary { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual StoreAddress Address { get; set; }
        public virtual StoreCompany Company { get; set; }

        /// <summary>
        /// Get First Name from a string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFirstName(string name)
        {
            return name.Split(NAME_SEPARATOR)[0].Trim();
        }

        /// <summary>
        /// Get Last Name from a string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetLastName(string name)
        {
            string firstName = GetFirstName(name);
            return name.Substring(firstName.Length).Trim();
        }

        public override string ToString()
        {
            return "Individual (" + Id + ")" + FirstName + " - " + LastName;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreIndividual individual = obj as StoreIndividual;
            if (individual != null) equals = individual.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
