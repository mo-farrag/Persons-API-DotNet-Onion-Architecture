
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Business.Contacts;
using Business.Contacts.Repositories;
using DTO;
using Newtonsoft.Json;

namespace Business.Services
{
    public class PersonsService : Services.Contracts.IPersonsService
    {
        IPersonsRepository _personsRepository;
        ICountryRepository _countryRepository;

        public PersonsService(IPersonsRepository personsRepository, ICountryRepository countryRepository)
        {
            this._personsRepository = personsRepository;
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// validate person object and add it to database
        /// </summary>
        /// <param name="person"></param>
        /// <returns>return PersonResult(Errors object and Result json object if no validating object passed)</returns>
        public PersonResult Add(PersonRequest person)
        {
            var result = new PersonResult();
            var error = ValidatePerson(person);
            if (!error.HasErrors)
                result.Result = JsonConvert.SerializeObject(_personsRepository.Add(person));
            else
                result.Errors = error;

            return result;
        }

        /// <summary>
        /// validate person object from the data annotation added in the personRequest class 
        /// + check if email, phone and country code are exists
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Error ValidatePerson(PersonRequest person)
        {
            Error error = new Error() { HasErrors = false };

            var context = new ValidationContext(person, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(person, context, validationResults, true);

            CheckEmailIfExists(person.Email, ref validationResults);
            CheckPhoneIfExists(person.Phone, ref validationResults);
            ValidateCountryCode(person.CountryCode, ref validationResults);

            if (validationResults.Count > 0)
            {
                error.HasErrors = true;
                error.ErrorMessage = JsonConvert.SerializeObject(validationResults);
            }

            return error;
        }

        void CheckEmailIfExists(string email, ref List<ValidationResult> validationResult)
        {
            if (_personsRepository.IsEmailExists(email))
                validationResult.Add(new ValidationResult("taken", new string[] { "Email" }));
        }

        void CheckPhoneIfExists(string phone, ref List<ValidationResult> validationResult)
        {
            if (_personsRepository.IsPhoneExists(phone))
                validationResult.Add(new ValidationResult("taken", new string[] { "Phone" }));
        }

        void ValidateCountryCode(string countryCode, ref List<ValidationResult> validationResult)
        {
            if (!_countryRepository.IsCountryCodeExists(countryCode))
                validationResult.Add(new ValidationResult("inclusion", new string[] { "CountryCode" }));
        }
    }
}
