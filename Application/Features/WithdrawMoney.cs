using Application.DataAccess;
using System;

namespace Application.Features
{
    public class WithdrawMoney
    {
        private readonly IAccountRepository _accountRepository;

        public WithdrawMoney(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var account = _accountRepository.GetAccountById(fromAccountId);

            account.CanWithdraw(amount);
            account.ShouldNotifyInBalance();

            account.Balance -= amount;
            _accountRepository.Update(account);
        }
    }
}
