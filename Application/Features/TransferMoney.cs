using Application.DataAccess;
using System;

namespace Application.Features
{
    public class TransferMoney
    {
        private readonly IAccountRepository _accountRepository;

        public TransferMoney(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = _accountRepository.GetAccountById(fromAccountId);
            var to = _accountRepository.GetAccountById(toAccountId);

            from.CanWithdraw(amount);
            from.ShouldNotifyInBalance();
            
            to.CanPaidIn(amount);
            to.ShouldNotifyInLimit();

            from.Balance -= amount;
            from.Withdrawn -= amount;

            to.Balance += amount;
            to.PaidIn += amount;

            _accountRepository.Update(from);
            _accountRepository.Update(to);
        }
    }
}
