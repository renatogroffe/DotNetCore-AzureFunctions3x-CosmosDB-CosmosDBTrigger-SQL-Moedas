using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ConsolidacaoCotacoes.Data;

namespace ConsolidacaoCotacoes
{
    public static class UltimasCotacoesDolar
    {
        [FunctionName("UltimasCotacoesDolar")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function UltimasCotacoesDolar - HTTP GET");
            return new OkObjectResult(CotacoesRepository.ListUltimasCotacoesDolar());
        }
    }
}