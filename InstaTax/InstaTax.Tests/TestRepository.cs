﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InstaTax.Core;
using InstaTax.Core.DomainObjects;
using NHibernate;
using NUnit.Framework;

namespace InstaTax.Tests
{
    [TestFixture]
    public class TestRepository 
    {
        [Test]
        public void ShouldSaveAndLoadUser()
        {
            IRepository repository = new Repository();

            var password = new Password { PasswordString = "abc" };
            var email = new EmailAddress("b@c.com");
            var user = new User(email, password);

            user.FromMetro = true;
            user.RentPaid = 8000.00;
            
            user.Repository = repository;
            
            user.Register();
            
            User actualUser = repository.LoadByEmailId(email);

            Assert.IsNotNull(actualUser);
            Assert.AreEqual(email, actualUser.EmailAddress);
            Assert.AreEqual(user.FromMetro, actualUser.FromMetro);
            Assert.AreEqual(user.RentPaid, actualUser.RentPaid);
        }
        
        [Test]
        public void ShouldSaveAndLoadTaxStatement()
        {
            IRepository repository = new Repository();
            var password = new Password { PasswordString = "abc" };
            var email = new EmailAddress("balaji@gmail.com");
            var taxPayer= new User(email, password);

            taxPayer.FromMetro = true;
            taxPayer.RentPaid = 8000.00;
            
  
            AnnualSalary salary = new AnnualSalary() { Basic = 10000, Epf = 2000, Hra = 6000, Id = "salary", ProfessionalTax = 200, SpecialAllowance = 5000, TaxDedeuctedAtSource = 5000};
            TaxStatement taxStatement = new TaxStatement(salary, taxPayer);

            DonationsUnder80G donationsUnder80G = new DonationsUnder80G();
            donationsUnder80G.AddDonation(new HalfExemptDonation(20000));
            donationsUnder80G.AddDonation(new FullyExemptDonation(10000));
            taxStatement.DonationsUnder80G = donationsUnder80G;
            repository.Save(taxStatement);

            List<TaxStatement> taxStatements = repository.LoadAll<TaxStatement>();
            var loadedTaxStatement = taxStatements.FirstOrDefault(stmt => stmt.Id == taxStatement.Id);
            Assert.IsNotNull(loadedTaxStatement);
            Assert.IsNotNull(loadedTaxStatement.TaxPayer);

        }

    }
}