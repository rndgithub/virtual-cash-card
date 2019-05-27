using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RNDITConsultants.BusinessLogic;
using RNDITConsultants.Log;
using RNDITConsultants.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.BusinessLogic.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        Mock<ILogger> _logger;
        Mock<IAccountRepository> _accountRepository;
        IAccountController _accountController;

        [TestInitialize]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(x => x.GetAll()).Returns(_testAccounts);
            _accountController = new AccountController(_accountRepository.Object, _logger.Object);
        }


        [TestMethod()]
        public void Should_TopUp_Any_Arbitrary_Positive_Amount()
        {
            // Arrange
            var testInput = new { AccountNumber = 123456, TopUpAmount = 10 };
      
            // Act
            _accountController.TopUp(testInput.AccountNumber, testInput.TopUpAmount);

            // Assert
            _accountRepository.Verify(x => x.Update(It.IsAny<Account>()), Times.Exactly(1));
            var account = _testAccounts.First(x => x.Number == testInput.AccountNumber);
            account.Balance.Should().Be(testInput.TopUpAmount);
            account.Transactions.Count.Should().Be(1);

        }

        [TestMethod()]
        public void Should_Withdraw_Money_With_Valid_Pin()
        {
            // Arrange
            var testInput = new { AccountNumber = 123456, Pin = "1234", WithdrawalAmount = 10 };
            var initialBalance = testInput.WithdrawalAmount;
            _testAccounts.First(x => x.Number == testInput.AccountNumber).Balance = initialBalance;

            // Act
            _accountController.Withdraw(testInput.AccountNumber, testInput.Pin, testInput.WithdrawalAmount);

            // Assert
            _accountRepository.Verify(x => x.Update(It.IsAny<Account>()), Times.Exactly(1));
            var account = _testAccounts.First(x => x.Number == testInput.AccountNumber);
            account.Balance.Should().Be(0);
            account.Transactions.Count.Should().Be(1);
        }

        [TestMethod()]
        public void Should_Withdraw_Money_Fail_With_Invalid_Pin()
        {
            // Arrange
            var testInput = new { AccountNumber = 123456, Pin = "5555", WithdrawalAmount = 10 };

            // Act
            Action result = () => { _accountController.Withdraw(testInput.AccountNumber, testInput.Pin, testInput.WithdrawalAmount); } ;

            // Assert
            result.Should().Throw<Exception>().WithMessage("Account number/PIN combination is incorrect!");
        }

        [TestMethod()]
        public void Should_Be_Able_To_Be_Used_Simultaneously()
        {
            // Arrange
            var testInput = new { AccountNumber = 123456, PIN = "1234", Amount = 10, Iterations = 10000 };
            var initialBalance = testInput.Amount * testInput.Iterations;
            _testAccounts.First(x => x.Number == testInput.AccountNumber).Balance = initialBalance;

            // Act
            var tasks = new List<Task>();
            for (int i = 0; i < testInput.Iterations; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => _accountController.TopUp(testInput.AccountNumber, testInput.Amount)));
                tasks.Add(Task.Factory.StartNew(() => _accountController.Withdraw(testInput.AccountNumber, testInput.PIN, testInput.Amount)));
            }

            Task.WaitAll(tasks.ToArray());

            // Assert
            var account = _testAccounts.First(x => x.Number == testInput.AccountNumber);
            account.Balance.Should().Be(initialBalance);
            account.Transactions.Count.Should().Be(testInput.Iterations * 2);

        }

        [TestMethod()]
        public void Should_Negative_Amount_TopUp_Not_Be_Allowed()
        {
            // Arrange
            var testInput = new { AccountNumber = 123456, TopUpAmount = -1 };

            // Act
            Action result = () => { _accountController.TopUp(testInput.AccountNumber, testInput.TopUpAmount); };

            // Assert
            result.Should().Throw<Exception>().WithMessage("Only positive amount top up is allowed!");
        }

        [TestMethod()]
        public void Should_Balance_Match_TopUps()
        {
            // Arrange
            var testInput = new { AccountNumber = 123456, TopUpAmount1 = 15, TopUpAmount2 = 20 };

            // Act
            _accountController.TopUp(testInput.AccountNumber, testInput.TopUpAmount1);
            _accountController.TopUp(testInput.AccountNumber, testInput.TopUpAmount2);

            // Assert
            _accountRepository.Verify(x => x.Update(It.IsAny<Account>()), Times.Exactly(2));
            var account = _testAccounts.First(x => x.Number == testInput.AccountNumber);
            account.Balance.Should().Be(testInput.TopUpAmount1 + testInput.TopUpAmount2);
            account.Transactions.Count.Should().Be(2);
        }

        private ICollection<Account> _testAccounts = new List<Account>
        {
            new Account
            {
                Number = 123456,
                Pin = "1234",
                Balance = 0,
                AccountHolder = new Person { Name = "Joe Bloggs" }
            },
            new Account
            {
                Number = 456789,
                Pin = "4567",
                Balance = 0,
                AccountHolder = new Person { Name = "Fred Bloggs" }
            },
        };
    }
}