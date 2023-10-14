namespace BlueCalculator
{
    public interface IVatCalculatorService
    {
        decimal CalculateNetAmountByGrossAmount(decimal grossAmount, decimal vatRate);
        decimal CalculateNetAmountByVATAmount(decimal vatAmount, decimal vatRate);
        decimal CalculateVATAmountByNetAmount(decimal netAmount, decimal vatRate);

    }
}
