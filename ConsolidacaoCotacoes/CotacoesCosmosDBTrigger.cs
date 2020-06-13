using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ConsolidacaoCotacoes.Data;

namespace ConsolidacaoCotacoes
{
    public static class CotacoesCosmosDBTrigger
    {
        [FunctionName("CotacoesCosmosDBTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "DBMoedas",
            collectionName: "Cotacoes",
            ConnectionStringSetting = "DBCotacoesConnectionString",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                foreach (var doc in input)
                {
                    var document = CotacoesRepository.GetDocument(doc.Id);
                    log.LogInformation($"Dados: {JsonSerializer.Serialize(document)}");

                    if (document.id.StartsWith("USD"))
                    {
                        CotacoesRepository.SaveCotacaoDolar(document);
                        log.LogInformation("Cotação registrada com sucesso");
                    }
                }
            }
        }
    }
}