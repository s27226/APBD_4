using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (IsFirstNameCorrect(firstName) || IsLastNameCorrect(lastName))
            {
                return false;
            }

            if (IsEmailCorrect(email))
            {
                return false;
            }

            var age = CalculateAgeUsingBirthdate(dateOfBirth);

            if (IsAgeCorrect(age))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (IsVeryImportantClient(client))
            {
                user.HasCreditLimit = false;
            }
            else if (IsImportantClient(client))
            {
                ChangeCreditLimit(creditLimit => creditLimit = creditLimit * 2, user);
            }
            else
            {
                user.HasCreditLimit = true;
                ChangeCreditLimit(creditLimit => creditLimit, user);
            }

            if (HasUserExceededCreditLimit(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private bool IsFirstNameCorrect(string firstName)
        {
            return string.IsNullOrEmpty(firstName);
        }

        private bool IsLastNameCorrect(string lastName)
        {
            return string.IsNullOrEmpty(lastName);
        }

        private bool IsEmailCorrect(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        private int CalculateAgeUsingBirthdate(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (!IsAlreadyBirthday(now,dateOfBirth)) 
                age--;
            return age;
        }

        private bool IsAgeCorrect(int age)
        {
            return age < 21;
        }

        private bool IsVeryImportantClient(Client client)
        {
            return client.Type == "VeryImportantClient";
        }

        private bool IsImportantClient(Client client)
        {
            return client.Type == "ImportantClient";
        }

        private bool HasUserExceededCreditLimit(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        private void ChangeCreditLimit(Func<int,int> creditChangeFunc, User user)
        {
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditChangeFunc(creditLimit);
                user.CreditLimit = creditLimit;
            }
        }

        private bool IsAlreadyBirthday(DateTime now, DateTime dateOfBirth)
        {
            return now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day);
        }
    }
}
