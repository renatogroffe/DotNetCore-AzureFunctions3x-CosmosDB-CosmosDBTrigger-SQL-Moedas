using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Data.SqlClient;
using Dapper;
using ConsolidacaoCotacoes.Models;

namespace ConsolidacaoCotacoes.Data
{
    public static class CotacoesRepository
    {
        public static CotacaoDocument GetDocument(string id)
        {
            using (var client = new DocumentClient(
                new Uri(Environment.GetEnvironmentVariable("DBCotacoesEndpointUri")),
                        Environment.GetEnvironmentVariable("DBCotacoesEndpointPrimaryKey")))
            {
                FeedOptions queryOptions =
                    new FeedOptions { MaxItemCount = -1 };

                return client.CreateDocumentQuery<CotacaoDocument>(
                        UriFactory.CreateDocumentCollectionUri(
                            "DBMoedas", "Cotacoes"),
                            "SELECT * FROM c " +
                           $"WHERE c.id = '{id}'", queryOptions)
                        .ToArray()[0];
            }
        }

        public static void SaveCotacaoDolar(CotacaoDocument document)
        {
            using (var conexao = new SqlConnection(
                Environment.GetEnvironmentVariable("BaseIndicadores")))
            {
                conexao.Execute(
                    "INSERT INTO dbo.CotacoesDolar(CodReferencia, DataReferencia, Valor) " +
                    "VALUES(@CodReferencia, @DataReferencia, @Valor)",
                    new
                    {
                        CodReferencia = document.id,
                        DataReferencia = document.Data,
                        Valor = document.Valor
                    });
            }
        }

        public static IEnumerable<CotacaoDolar> ListUltimasCotacoesDolar()
        {
            using (var conexao = new SqlConnection(
                Environment.GetEnvironmentVariable("BaseIndicadores")))
            {
                return conexao.Query<CotacaoDolar>(
                    "SELECT TOP 5 * FROM dbo.CotacoesDolar " +
                    "ORDER BY Id DESC");
            }
        }
    }
}