using System;
using System.Linq;
using Assignment_04_EFC_.AppDbConntext;
using Assignment_04_EFC_.Enums;
using Assignment_04_EFC_.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_04_EFC_
{
    class Program
    {
        static void Pause()
        {
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        // 1) Add a new Customer
        static void AddNewCustomer(BankDbContext context)
        {
            Console.Clear();
            Console.WriteLine("--- Add New Customer ---");

            Console.Write("Full Name: ");
            string name = Console.ReadLine()!;

            Console.Write("National ID: ");
            string nationalId = Console.ReadLine()!;

            Console.Write("Date of Birth (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine()!, out DateTime dob))
            {
                Console.WriteLine("Invalid date format!");
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine()!;

            Console.Write("Phone: ");
            string phone = Console.ReadLine()!;

            Console.Write("Address: ");
            string address = Console.ReadLine()!;

            Console.WriteLine("Customer Type:\n1) Individual\n2) Business");
            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine()!, out int typeChoice) || (typeChoice != 1 && typeChoice != 2))
            {
                Console.WriteLine("Invalid customer type choice!");
                return;
            }

            CustomerType customerType = (typeChoice == 1) ? CustomerType.Individual : CustomerType.Business;

            var customer = new Customer
            {
                FullName = name,
                NationalId = nationalId,
                DateOfBirth = dob,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = customerType
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            Console.WriteLine($"\nCustomer created successfully. CustomerId = {customer.Id}");
        }

        // 2) Open a new Account for a Customer
        static void OpenNewAccount(BankDbContext context)
        {
            Console.Clear();
            Console.WriteLine("--- Open New Account ---");

            Console.Write("Account Number: ");
            string accNumber = Console.ReadLine()!;

            if (context.Accounts.Any(a => a.AccountNumber == accNumber))
            {
                Console.WriteLine("Error: Account number already exists!");
                return;
            }

            Console.WriteLine("Account Type:\n1) Savings\n2) Current\n3) Business");
            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine()!, out int typeChoice)) return;
            AccountType accType = typeChoice switch
            {
                1 => AccountType.Savings,
                2 => AccountType.Current,
                _ => AccountType.Business
            };

            Console.Write("Branch Code: ");
            string branchCode = Console.ReadLine()!;

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine()!, out int customerId)) return;

            Console.WriteLine($"Validating branch '{branchCode}' and customer #{customerId}...");

            var branch = context.Branches.Find(branchCode);
            var customer = context.Customers.Find(customerId);

            if (branch == null || customer == null)
            {
                Console.WriteLine("Error: Branch or Customer not found!");
                return;
            }

            Console.WriteLine("Ownership Role:\n1) Primary\n2) CoHolder");
            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine(), out int roleChoice)) return;
            OwnershipType ownershipType = (roleChoice == 1) ? OwnershipType.Primary : OwnershipType.CoHolder;

            var account = new Account
            {
                AccountNumber = accNumber,
                AccountType = accType,
                CurrentBalance = 0,
                OpeningDate = DateTime.Now,
                AccountStatus = AccountStatus.Active,
                BranchCode = branchCode
            };

            var customerAccount = new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = accNumber,
                OwnershipType = ownershipType,
                OwnershipStartDate = DateTime.Now
            };

            context.Accounts.Add(account);
            context.CustomerAccounts.Add(customerAccount);
            context.SaveChanges();

            string roleName = ownershipType == OwnershipType.Primary ? "Primary" : "CoHolder";
            Console.WriteLine($"\nAccount {accNumber} created and linked to customer {customerId} as {roleName} owner.");
        }

        // 3) Update Account Status
        static void UpdateAccountStatus(BankDbContext context)
        {
            Console.Clear();
            Console.WriteLine("--- Update Account Status ---");

            Console.Write("Account Number: ");
            string accNumber = Console.ReadLine()!;

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine()!, out int customerId))
            {
                Console.WriteLine("Invalid Customer ID!");
                return;
            }

            var customerAccount = context.CustomerAccounts
                .Include(ca => ca.Account)
                .FirstOrDefault(ca => ca.AccountNumber == accNumber && ca.CustomerId == customerId);

            if (customerAccount == null || customerAccount.Account == null)
            {
                Console.WriteLine("Error: Account or Customer link not found!");
                return;
            }

            var account = customerAccount.Account;

            Console.WriteLine($"Current Status: {account.AccountStatus}");
            Console.WriteLine("New Status:\n1) Active\n2) Closed");
            Console.Write("Choice: ");

            if (!int.TryParse(Console.ReadLine(), out int statusChoice) || (statusChoice != 1 && statusChoice != 2))
            {
                Console.WriteLine("Invalid status choice!");
                return;
            }

            account.AccountStatus = (statusChoice == 1) ? AccountStatus.Active : AccountStatus.Closed;
            context.SaveChanges();

            Console.WriteLine($"\nStatus updated to {account.AccountStatus}.");
        }

        // 4) Remove an Account from a Customer
        static void RemoveAccountFromCustomer(BankDbContext context)
        {
            Console.Clear();
            Console.WriteLine("--- Remove Account From Customer ---");

            Console.Write("Account Number: ");
            string accNumber = Console.ReadLine()!;

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine()!, out int customerId)) return;

            var link = context.CustomerAccounts
                .FirstOrDefault(ca => ca.AccountNumber == accNumber && ca.CustomerId == customerId);

            if (link == null)
            {
                Console.WriteLine("Link between customer and account not found!");
                return;
            }

            context.CustomerAccounts.Remove(link);
            context.SaveChanges();

            Console.WriteLine("Ownership link deleted.");

            var remainingOwners = context.CustomerAccounts.Count(ca => ca.AccountNumber == accNumber);
            if (remainingOwners == 0)
            {
                var account = context.Accounts.Find(accNumber);
                if (account != null)
                {
                    context.Accounts.Remove(account);
                    context.SaveChanges();
                    Console.WriteLine($"That was the last owner. Account '{accNumber}' was also removed.");
                }
            }
            else
            {
                Console.WriteLine("That was not the last owner.");
            }
        }

        // 5) List all Customers (with accounts) using Eager Loading
        static void ListAllCustomers(BankDbContext context)
        {
            Console.Clear();
            Console.WriteLine("--- All Customers ---");

            var customers = context.Customers
                .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                        .ThenInclude(a => a.Branch)
                .ToList();

            if (!customers.Any())
            {
                Console.WriteLine("No customers found.");
                return;
            }

            foreach (var c in customers)
            {
                Console.Write($"#{c.Id} {c.FullName} ({c.CustomerType})");

                if (c.CustomerAccounts == null || !c.CustomerAccounts.Any())
                {
                    Console.WriteLine(" (no accounts)");
                }
                else
                {
                    Console.WriteLine();
                    foreach (var ca in c.CustomerAccounts)
                    {
                        var acc = ca.Account;
                        if (acc != null)
                        {
                            string branchName = acc.Branch != null ? acc.Branch.Name : "Unknown Branch";
                            Console.WriteLine($"\t{acc.AccountNumber} Current Balance: {acc.CurrentBalance:N2} {ca.OwnershipType} {acc.AccountStatus} @ {branchName}");
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {

            #region Part 03: Interactive Menu

            while (true)
            {
                Console.Clear();
                Console.WriteLine("National Bank Management System");
                Console.WriteLine("===============================");
                Console.WriteLine("1) Add a new Customer");
                Console.WriteLine("2) Open a new Account for a Customer");
                Console.WriteLine("3) Update Account Status (Active / Closed)");
                Console.WriteLine("4) Remove an Account from a Customer");
                Console.WriteLine("5) List all Customers (with accounts)");
                Console.WriteLine("0) Exit");
                Console.Write("Enter choice: ");

                string input = Console.ReadLine()!;

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("\nInvalid input! Please enter a valid number.");
                    Pause();
                    continue;
                }

                using var context = new BankDbContext();

                switch (choice)
                {
                    case 1:
                        AddNewCustomer(context);
                        break;
                    case 2:
                        OpenNewAccount(context);
                        break;
                    case 3:
                        UpdateAccountStatus(context);
                        break;
                    case 4:
                        RemoveAccountFromCustomer(context);
                        break;
                    case 5:
                        ListAllCustomers(context);
                        break;
                    case 0:
                        Console.WriteLine("\nExiting application. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice! Please select from 0 to 5.");
                        break;
                }

                Pause();
            }

            #endregion
        }
    }
}