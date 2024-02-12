using System;
using Application.DataAccess;
using Application.Domain;
using Application.Domain.Services;
using Application.Features;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TransferMoneyTests
    {
        [Test]
        public void Execute_TransferMoney_SuccessfullyTransfersMoney()
        {
            // Arrange
            var fromAccount = new Account(new Mock<INotificationService>().Object)
            {
                Id = Guid.NewGuid(),
                Balance = 100m
            };
            var toAccount = new Account(new Mock<INotificationService>().Object)
            {
                Id = Guid.NewGuid(),
                Balance = 0m
            };
            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(r => r.GetAccountById(fromAccount.Id)).Returns(fromAccount);
            accountRepositoryMock.Setup(r => r.GetAccountById(toAccount.Id)).Returns(toAccount);
            var transferMoney = new TransferMoney(accountRepositoryMock.Object);

            // Act
            transferMoney.Execute(fromAccount.Id, toAccount.Id, 50m);

            // Assert
            Assert.AreEqual(50m, fromAccount.Balance);
            Assert.AreEqual(50m, toAccount.Balance);
        }
    }
}
