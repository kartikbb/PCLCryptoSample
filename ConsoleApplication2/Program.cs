using System;
using Security;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            int numErrors = 0;
            for (int i = 0; i < 1000; i++)
            {
                var password = RandomTextGenerator.GenerateRandomString(32);

                try
                {
                    var paymentCard = new PaymentCard();
                    paymentCard.SetTestPaymentCard();
                    string text = paymentCard.ToEncryptedString("8UfROeJudbbl", password);

                    var paymentCard2 = new PaymentCard();
                    paymentCard2.InitializeFromEncryptedString(text, "8UfROeJudbbl", password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(password);
                    numErrors++;
                }
            }
        }
    }
}
