using System;
using Newtonsoft.Json;

namespace Security
{
    public class PaymentCard
    {
        public string CardRawNameCaptured { get; set; }

        public string CardFirstName { get; set; }

        public string CardLastName { get; set; }

        public string CardNumber { get; set; }

        public CreditCardBrand CardBrand { get; set; }

        public int CardExpiryMonth { get; set; }

        public int CardExpiryYear { get; set; }

        // KB: For the most part, cards will not expire until the end of their expiration month.
        [JsonIgnore]
        public DateTime CardExpiryDate => new DateTime(CardExpiryYear, CardExpiryMonth, 1).AddMonths(1).AddDays(-1);

        [JsonIgnore]
        public bool HasCardExpired => CardExpiryDate < DateTime.Now;

        public string Cvv { get; set; }

        public string CardLast4 { get; set; }

        public string IssuerIdentificationNumber { get; set; }

        public CardSimulationMode CardSimulationMode { get; set; }

        public string ProviderToken { get; set; }

        public string ProviderCardId { get; set; }

        public string ProviderValidationErrorMessage { get; set; }

        public string ProviderValidationErrorCode { get; set; }

        public void SetTestPaymentCard()
        {
            CardRawNameCaptured = "Kartik Bansal";
            CardFirstName = "Kartik";
            CardLastName = "Bansal";
            CardNumber = "4242424242424242";
            CardBrand = CreditCardBrand.Visa;
            CardExpiryYear = 2018;
            CardExpiryMonth = 1;
            CardLast4 = "4242";
            IssuerIdentificationNumber = "424242";
            Cvv = "123";
        }

        public string ToEncryptedString(string saltString, string password)
        {
            string data = JsonConvert.SerializeObject(this);

            return new CryptoAesEncryptionProvider().Encrypt(data, password, saltString);
        }

        public void InitializeFromEncryptedString(string encryptedString, string saltString, string password)
        {
            string decryptedString = new CryptoAesEncryptionProvider().Decrypt(encryptedString, password, saltString);
            var paymentCard = JsonConvert.DeserializeObject<PaymentCard>(decryptedString);

            CardRawNameCaptured = paymentCard.CardRawNameCaptured;
            CardFirstName = paymentCard.CardFirstName;
            CardLastName = paymentCard.CardLastName;
            if (!string.IsNullOrEmpty(paymentCard.CardNumber))
                CardNumber = paymentCard.CardNumber;
            CardBrand = paymentCard.CardBrand;
            ProviderToken = paymentCard.ProviderToken;
            ProviderCardId = paymentCard.ProviderCardId;
            CardExpiryYear = paymentCard.CardExpiryYear;
            CardExpiryMonth = paymentCard.CardExpiryMonth;
            Cvv = paymentCard.Cvv;
            CardLast4 = paymentCard.CardLast4;
            IssuerIdentificationNumber = paymentCard.IssuerIdentificationNumber;
            CardSimulationMode = paymentCard.CardSimulationMode;
        }
    }

    public enum CreditCardBrand
    {
        Unknown = 0,
        Visa = 1,
        MasterCard = 2,
        AmericanExpress = 3,
        Discover = 4,
        Jcb = 5,
        DinersClub = 6
    }

    public enum CardSimulationMode
    {
        NoSimulation = 0,
        FailCvv,
        CardDeclined,
        SuccessVisa,
        SuccessMastercard,
        SuccessAmericanExpress
    }
}