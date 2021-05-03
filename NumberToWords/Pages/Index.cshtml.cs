using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NumberToWords.Services;
using System;

namespace NumberToWords.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly INumberTranslatorService _translator;

        private readonly IStringLocalizer<IndexModel> _localizer;

        /// <summary>
        /// The maximum allowed value to be translated (is specific for a given NumberToWords.Services.INumberTranslatorService implementation).
        /// </summary>
        public string MaxValue { get; private set; }

        /// <summary>
        /// The number to be translated
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Result of translation; also used to present validation errors to user if required.
        /// </summary>
		public string Words { get; set; }

        public IndexModel(ILogger<IndexModel> logger, INumberTranslatorService translator, IStringLocalizer<IndexModel> localizer)
        {
            _logger = logger;
            _translator = translator;
            _localizer = localizer;
        }

        public void OnGet()
        {
            this.MaxValue = _translator.GetMaxValue().ToString();
        }

        public IActionResult OnPost()
        {
            _logger.LogDebug("Translating {Number}", this.Number);
            try
            {
                this.Words = _translator.Translate(BigCurrency.ValueOf(this.Number));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred attempting to translate {Number}", this.Number);
                if (e is FormatException)
                {
                    this.Words = _localizer["Invalid Number"];
                }
                else
                {
                    throw e;
                }
            }
            return Page();
        }
    }
}
