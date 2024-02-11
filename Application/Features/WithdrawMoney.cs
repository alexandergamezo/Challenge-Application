using Application.DataAccess;
using Application.Domain.Services;
using System;
using Application.Domain;

namespace Application.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var account = accountRepository.GetAccountById(fromAccountId);

            var balance = account.Balance - amount;
            if (balance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            if (balance < 500m)
            {
                notificationService.NotifyFundsLow(account.User.Email);
            }

            account.Balance -= amount;
            accountRepository.Update(account);
        }
    }
}
