﻿namespace Phundus.Core.Tests
{
    using Core.Shop.Contracts.Model;

    public class BorrowerFactory
    {
        public static Borrower Create()
        {
            return Create(1);
        }

        public static Borrower Create(int borrowerId)
        {
            return new Borrower(borrowerId, "Hans", "Muster", "Strasse", "6000", "Luzern", "hans.muster@test.phundus.ch",
                "+4179123456", "");
        }

        public static Borrower Create(int borrowerId, string firstName, string lastName, string street = "",
            string postcode = "", string city = "", string emailAddress = "", string mobilePhoneNumber = "",
            string memberNumber = "")
        {
            return new Borrower(borrowerId, firstName, lastName, street, postcode, city, emailAddress, mobilePhoneNumber,
                memberNumber);
        }
    }
}