using Application.DataAccess;
using Application.Domain;
using Application.Domain.Services;
using Application.Features;
using Moq;
using NUnit.Framework;

namespace Application.Tests
{
    public class Tests
    {
        [TestFixture]
        public class TransferMoneyTests
        {
            [Test]
            public void Execute_TransferMoney_SuccessfullyTransfersMoney()
            {
                // Arrange
                var notificationServiceMock = new Mock<INotificationService>();
                var fromUser = new User { Email = "test@example.com" };
                var fromAccount = new Account(notificationServiceMock.Object)
                {
                    Id = Guid.NewGuid(),
                    Balance = 100m,
                    User = fromUser
                };

                var toUser = new User { Email = "test@example.com" };
                var toAccount = new Account(new Mock<INotificationService>().Object)
                {
                    Id = Guid.NewGuid(),
                    Balance = 0m,
                    User = toUser
                };

                var accountRepositoryMock = new Mock<IAccountRepository>();
                accountRepositoryMock.Setup(r => r.GetAccountById(fromAccount.Id)).Returns(fromAccount);
                accountRepositoryMock.Setup(r => r.GetAccountById(toAccount.Id)).Returns(toAccount);
                var transferMoney = new TransferMoney(accountRepositoryMock.Object);

                // Act
                transferMoney.Execute(fromAccount.Id, toAccount.Id, 50m);

                // Assert
                Assert.That(fromAccount.Balance, Is.EqualTo(50m));
                Assert.That(toAccount.Balance, Is.EqualTo(50m));
            }
        }
    }
}