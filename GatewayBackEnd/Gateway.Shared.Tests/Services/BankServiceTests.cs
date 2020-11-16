namespace Gateway.Shared.Tests.Services
{
    using Gateway.Shared.Services;
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Gateway.Interfaces.Services;
    using Gateway.Shared.Representers;
    using Gateway.Data.Model;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using MockQueryable.Moq;
    using Gateway.Shared.Interfaces;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Gateway.Shared.Models;

    [TestClass]
    public class BankServiceTests
    {
        private BankService _testClass;
        private Mock<IRepositoryService> _contextService;
        private Mock<IApiService> _apiService;


        [TestInitialize]
        public void SetUp()
        {
            _contextService = new Mock<IRepositoryService>();
            _apiService = new Mock<IApiService>();
            _testClass = new BankService(_contextService.Object, _apiService.Object);
        }

        [TestMethod]
        public void CanConstruct()
        {
            var instance = new BankService(_contextService.Object, _apiService.Object);
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void CannotConstructWithNullDependencies()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BankService(null,null));
        }

        [TestMethod]
        public async Task CanCallGetBankByNameAsync()
        {
            var bankName = "LocalTestBank";
            var expectedBank = new Bank { BankID = 1, BankName = "LocalTestBank", BankURL = "www.localBank.com" };
            SetExpectedBank(bankName, expectedBank);

            var actualbank = await _testClass.GetBankByNameAsync(bankName).ConfigureAwait(false);
            Assert.IsTrue(
                actualbank.BankID == expectedBank.BankID,
                $"Bank IDs do not match. Expected:{expectedBank.BankID} | Actual:{expectedBank.BankID}"
            );
            Assert.IsTrue(
                actualbank.BankName == expectedBank.BankName,
                $"Bank Names do not match. Expected:{expectedBank.BankName} | Actual:{expectedBank.BankName}"
            );
            Assert.IsTrue(
                actualbank.BankURL == expectedBank.BankURL,
                $"Bank Urls do not match. Expected:{expectedBank.BankURL} | Actual:{expectedBank.BankURL}"
            );
        }

        [TestMethod]
        public async Task CanCallProcessTransactionAsync()
        {
            //Arrange
            TransactionRepresenter transactionRepresenter = SetExpectedTransaction();
            StringContent expectedResponse = SetExpectedResponse();
            var bankURL = "test-bank.com";

            _apiService
               .Setup(x => x.ProcessTransactionAsync(transactionRepresenter, bankURL))
               .ReturnsAsync(new HttpResponseMessage { Content = expectedResponse });

            //Act
            var result = await _testClass.ProcessTransactionAsync(transactionRepresenter, bankURL);

            //Assert
            _apiService.Verify(
                x => x.ProcessTransactionAsync(transactionRepresenter, bankURL),
                Times.AtMostOnce,
                "Method not called or called more than once");
        }

        [TestMethod]
        public async Task CallProcessTransactionAsyncWithNullTransactionRepresenterOrBankUrlReturnsNull()
        {
            var result = await _testClass.ProcessTransactionAsync(null, null);
            Assert.IsNull(result);
        }


        private void SetExpectedBank(string bankName, Bank expectedBank)
        {
            var banks = new List<Bank>()
            {
              new Bank{BankID=2, BankName="Bank Two", BankURL="www.bank-two.com"},
              new Bank{BankID=3, BankName="Some Bank", BankURL="www.some-bank.com"},
              new Bank{BankID=4, BankName="Pirate Bank", BankURL="www.pirate-bank.com"}
            };
            banks.Add(expectedBank);

            var mock = banks.AsQueryable().BuildMock();

            _contextService
                .Setup(x => x.Find<Bank>(b => b.BankName == bankName))
                .Returns(mock.Object.Where(x => x.BankName == bankName));
        }

        private static StringContent SetExpectedResponse()
        {
            var bankResponse = new BankResponseDto
            {
                BankResponseID = new Guid(),
                Status = TransactionStatus.Successful.ToString(),
                SubStatus = TransactionSubStatus.Successful.ToString(),
            };
            var expectedResponse = new StringContent(JsonConvert.SerializeObject(bankResponse));
            return expectedResponse;
        }

        private static TransactionRepresenter SetExpectedTransaction()
        {
            //Arrange
            return new TransactionRepresenter
            {
                Currency = "USD",
                Amount = 200,
                Card = new CardDetails
                {
                    CardDetailsID = 1,
                    CardNumber = "5170111111111111",
                    Cvv = "892",
                    HolderName = "Holder",
                    ExpiryMonth = "09",
                    ExpiryYear = 2021
                },
                MerchantID = new Guid().ToString(),
                Bank = "LocalTestBank"
            };
        }
    }
}