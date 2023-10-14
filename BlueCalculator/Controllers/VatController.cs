using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Numerics;
using System.Reflection;

namespace BlueCalculator.Controllers
{
    /// <summary>
    /// This is a sample API controller for VAT calculations.
    /// </summary>
    [Route("api/vat")]
    [ApiController]
    public class VatController : Controller
    {
        private readonly IVatCalculatorService _vatCalculatorService;

        public VatController(IVatCalculatorService vatCalculatorService)
        {
            _vatCalculatorService = vatCalculatorService;
        }


        /// <summary>
        /// Calculate VAT, Net, and Gross amounts based on input data.
        /// </summary>
        /// <param name="netAmount">The net amount.</param>
        /// <param name="grossAmount">The gross amount.</param>
        /// <param name="vatAmount">The VAT amount.</param>
        /// <param name="vatRate">The VAT rate.</param>
        /// <returns>Returns the calculated VAT, Net, and Gross amounts.</returns>
        [HttpGet("calculate")]
        public IActionResult Calculate(decimal? netAmount,
                                       decimal? grossAmount,
                                       decimal? vatAmount,
                                       decimal vatRate)

        {

            // Check for missing or invalid inputs

            if (new[] { netAmount, grossAmount, vatAmount }.Count(p => p != null) > 1)
            {
                var errorResponse = new
                {
                    error = "more than one input"
                };
                string jsonError = JsonConvert.SerializeObject(errorResponse);

                //return BadRequest(jsonError);
                return new ContentResult
                {
                    Content = jsonError,
                    ContentType = "application/json",
                    StatusCode = 400

                    // Bad Request
                };
            }

            decimal chkDecimal;
            if (new[] { netAmount, grossAmount, vatAmount }.Count(p => p != null && !decimal.TryParse(p.ToString(), out chkDecimal)) > 0 ||
                new[] { netAmount, grossAmount, vatAmount }.Count(p => p != null) == 0)
            {
                var jsonError = new
                {
                    error = "missing or invalid (0 or non-numeric) amount input"
                };

                return BadRequest(jsonError);
                // Bad Request
            }

            if (!new decimal[] { 10, 13, 20 }.Contains(vatRate))
            {
                var jsonError = new
                {
                    error = "missing or invalid(0 or non - numeric) VAT rate input"
                };

                return BadRequest(jsonError);
                // Bad Request
            }

            if (netAmount != null)
            {

                // Calculate Gross and VAT from Net

                vatAmount = _vatCalculatorService.CalculateVATAmountByNetAmount((decimal)netAmount, vatRate);
                grossAmount = netAmount + vatAmount;

            }
            else if (grossAmount != null)
            {

                // Calculate Net and VAT from Gross

                netAmount = _vatCalculatorService.CalculateNetAmountByGrossAmount((decimal)grossAmount, vatRate);
                vatAmount = grossAmount - netAmount;

            }
            else
            {

                // Calculate Net and Gross from VAT

                netAmount = _vatCalculatorService.CalculateNetAmountByVATAmount((decimal)vatAmount, vatRate);
                grossAmount = netAmount + vatAmount;

            }


            // Return the calculated values in a meaningful structure

            var result = new
            {
                netAmount,
                grossAmount,
                vatAmount
            };
            string jsonResult = JsonConvert.SerializeObject(result);

            return Ok(jsonResult);
        }
    }
}
