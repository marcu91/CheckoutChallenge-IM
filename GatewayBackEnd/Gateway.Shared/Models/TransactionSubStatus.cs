namespace Gateway.Shared.Models
{
    public enum TransactionSubStatus
    {
        DeclinedDonothonour,
        InvalidPayment,
        InvalidCardNumber,
        InsufficientFunds,
        BadTrackData,
        RestrictedCard,
        SecurityViolation,
        ResponseTimeout,
        CardNot3DSecureEnabled,
        UnableToSpecifyIfCardIsSecureEnabled,
        SecureSystemMalfunction3DSecure,
        SecureAuthenticationRequired3DSecure,
        ExpiredCard,
        PaymentBlockedDueToRisk,
        CardNumberBlacklisted,
        StopPaymenttThisAuth,
        StopPaymentAll,
        Successful
    }
}