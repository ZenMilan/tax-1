using System.Text.RegularExpressions;

namespace InstaTax.Core.DomainObjects
{
    public enum Gender
    {
        Male, Female
    }

    public class User{
        public virtual double HousingLoanInterestAmount { get; set; }
        public virtual double RentPaid { get; set; }
        public virtual bool? FromMetro { get; set; }
        public virtual IUserRepository Repository { get; set; }
        public virtual string EmailId { get; set; }
        public virtual Password Password { get; set; }
        public virtual string Id { get; set; }
        public virtual Gender Gender { get; set; }

        public User()
        {

        }

        public User(double rentPaid, bool? fromMetro, Gender gender){
            RentPaid = rentPaid;
            FromMetro = fromMetro;
            Gender = gender;
        }

        public User(string emailId, Password password, IUserRepository repository)
        {
            Repository = repository;
            EmailId = emailId;
            Password = password;
        }

        public User(string emailId, Password password)
        {
            EmailId = emailId;
            Password = password;
        }

        public virtual void Save()
        {
            if (Repository.CheckIfUnique())
            {
                Repository.Save();
            }
            else
            {
                throw new DuplicateUserException("Unique user not found");
            }
        }

        public virtual bool IsValidId()
        {
            const string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);
            return re.IsMatch(EmailId);
        }

        public virtual bool IsFemale()
        {
            return Gender == Gender.Female;
        }
    }
}