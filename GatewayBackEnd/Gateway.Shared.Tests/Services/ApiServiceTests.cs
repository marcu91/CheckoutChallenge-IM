namespace Gateway.Shared.Tests.Services
{
    using Gateway.Shared.Services;
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Gateway.Shared.Interfaces;
    using Gateway.Shared.Representers;
    using Gateway.Data.Model;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Gateway.Shared.Models;
    using System.Collections.Generic;

    [TestClass]
    public class ApiServiceTests
    {
        private ApiService _testClass;
        private Mock<IWebRequestService> _webRequestService;

        [TestInitialize]
        public void SetUp()
        {
            _webRequestService = new Mock<IWebRequestService>();
            _testClass = new ApiService(_webRequestService.Object);
        }

        [TestMethod]
        public void CanConstruct()
        {
            var instance = new ApiService(_webRequestService.Object);
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        [DynamicData(nameof(DataStatusItemsTestSetup))]
        public async Task CallProcessTransactionAsyncWithNullTransactionOrNullBankUrlReturnsNull(
            TransactionRepresenter transaction, string bankUrl)
        {
            var result =  await this._testClass.ProcessTransactionAsync(transaction, bankUrl).ConfigureAwait(false);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CanCallProcessTransactionAsync()
        {
            //Arrange
            TransactionRepresenter transactionRepresenter = SetExpectedTransaction();
            StringContent expectedResponse = SetExpectedResponse();
            var bankURL = "test-bank.com";

            _webRequestService
               .Setup(x => x.MakeAsyncRequest(bankURL, It.IsAny<string>()))
               .ReturnsAsync(new HttpResponseMessage { Content = expectedResponse });

            //Act
            var result = await _testClass.ProcessTransactionAsync(transactionRepresenter, bankURL);

            //Assert
            _webRequestService.Verify(
                x => x.MakeAsyncRequest(bankURL, It.IsAny<string>()),
                Times.AtMostOnce,
                "Method not called or called more than once");
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

        static IEnumerable<object[]> DataStatusItemsTestSetup
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new TransactionRepresenter(),
                        null
                    },
                    new object[]
                    {
                        null,
                        "bank-url"
                    }
                };
            }
        }
    }
}