using System;
using Application.Domain.Services;

namespace Application.Domain
{
    public class Account
    {
        private readonly INotificationService _notificationService;

        public Account(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public void CanWithdraw(decimal amount)
        {
            if (Balance - amount >= Constants.MinAccountValue) return;
            throw new InvalidOperationException("Insufficient funds to make transfer");
        }

        public void CanPaidIn(decimal amount)
        {
            if (PaidIn + amount < Constants.PayInLimit) return;
            throw new InvalidOperationException("Account pay in limit reached");
        }

        public void ShouldNotifyInBalance()
        {
            if (Balance >= Constants.MinimumBalanceForNotification) return;
            _notificationService.NotifyFundsLow(User.Email);
        }

        public void ShouldNotifyInLimit()
        {
            if (Constants.PayInLimit - PaidIn >= Constants.MinimumBalanceForNotification) return;
            _notificationService.NotifyApproachingPayInLimit(User.Email);
        }
    }
}
