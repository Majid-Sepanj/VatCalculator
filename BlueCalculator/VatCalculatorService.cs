namespace BlueCalculator
{
    public class VatCalculatorService : IVatCalculatorService
    {
        public decimal CalculateNetAmountByGrossAmount(decimal grossAmount, decimal vatRate)
        {
            decimal NetAmount = grossAmount / (1 + vatRate / 100);
            return NetAmount;
        }
        public decimal CalculateNetAmountByVATAmount(decimal vatAmount, decimal vatRate)
        {
            decimal NetAmount = vatAmount / (vatRate / 100);
            return NetAmount;
        }
        public decimal CalculateVATAmountByNetAmount(decimal netAmount, decimal vatRate)
        {            
            decimal VatAmount = netAmount * (vatRate / 100);
            return VatAmount;
        }
    }
}
