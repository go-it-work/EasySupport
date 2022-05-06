using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using Nest;

partial class ElasticSearchAPI
{
    //public void ReadData()
    //{
    //    Debug.Log("Estou no loop de leitura de dados");

    //    var client = new RestClient("http://localhost:9200/usuarios/_search");
    //    client.Timeout = -1;

    //    var request = new RestRequest(Method.GET);
    //    request.AddHeader("source_content-Type", "application/json");

    //    var body = @"{
    //                " + "\n" +
    //                            @"  ""query"": {
    //                " + "\n" +
    //                            @"    ""match_all"": {}
    //                " + "\n" +
    //                            @"  },
    //                " + "\n" +
    //                            @"  ""_source"": [""usuarios""]
    //                " + "\n" +
    //                            @"}";

    //    request.AddParameter("application/json", body, ParameterType.RequestBody);

    //    IRestResponse response = client.Execute(request);
    //    Debug.Log(response.Content);

    //    Debug.Log("Consegui passar da parte de abrir a conection.");

    //    for (int i = 0; i < response.Headers.Count; i++)
    //    {
    //        Debug.Log(response.Headers[i].Name + " tem o valor " + response.Headers[i].Value);
    //        Debug.Log(response.Content);
    //    }
    //}

    //private void StartConnection()
    //{
    //    var uris = new Uri[]
    //    {
    //        new Uri("https://localhost:9200"),
    //        new Uri("https://localhost:9200/usuarios"),
    //        new Uri("https://localhost:9200/")
    //    };


    //    var connectionPool = new SniffingConnectionPool(uris);
    //    var settings = new ConnectionSettings(connectionPool)
    //        .DefaultMappingFor<Testing>(i => i.IndexName("usuarios"));

    //    var client = new ElasticClient(settings);

    //    var document = new Testing("this is a test", DateTime.Now);

    //    var indexResponse = client.Index(new IndexRequest<Testing>(document, IndexName.From<Testing>(), 1));
    //    if (!indexResponse.IsValid)
    //    {
    //        Console.Write("Failed to index document. ");
    //        if (indexResponse.ServerError != null)
    //        {
    //            Console.WriteLine(indexResponse.ServerError);
    //        }
    //        else if (indexResponse.OriginalException != null)
    //        {
    //            Console.WriteLine(indexResponse.OriginalException);
    //        }
    //        else
    //        {
    //            Console.WriteLine("Error code: " + indexResponse.ApiCall.HttpStatusCode);
    //        }
    //    }
    //    else
    //    {
    //        Console.WriteLine($"Indexed document to index \"testing\" with id 1");
    //    }


    //    var getResponse = client.Get<Testing>(new GetRequest<Testing>(1));
    //    if (getResponse.OriginalException != null)
    //    {
    //        throw getResponse.OriginalException;
    //    }

    //    if (!getResponse.IsValid)
    //    {
    //        Console.Write("Failed to retrieve document. ");
    //        if (getResponse.ServerError != null)
    //        {
    //            Console.WriteLine(getResponse.ServerError);
    //        }
    //        else if (getResponse.OriginalException != null)
    //        {
    //            Console.WriteLine(getResponse.OriginalException);
    //        }
    //        else
    //        {
    //            Console.WriteLine("Error code: " + getResponse.ApiCall.HttpStatusCode);
    //        }
    //    }
    //    else
    //    {
    //        Console.WriteLine($"Retrieved document: " +
    //            $"{{Id: {getResponse.Id}, " +
    //            $"Description: {getResponse.Source.Description}, " +
    //            $"Timestamp: {getResponse.Source.Timestamp}}}");
    //    }
    //}

    //ElasticClient elasticClient;
    //ConnectionSettings connectionSettings;

    //void Start()
    //{
    //    ReadData();

    //    StartConnection();
    //}

    //public void AllActions()
    //{
    //    CreatingConnection();
    //}

    //private async void CreatingConnection()
    //{
    //    var settings = new ConnectionConfiguration(new Uri("http://localhost:9200/"))
    //    .RequestTimeout(TimeSpan.FromMinutes(2));

    //    var lowlevelClient = new ElasticLowLevelClient(settings);

    //    var person = new Person
    //    {
    //        FirstName = "Martijn",
    //        LastName = "Laarman"
    //    };

    //    InsertData(lowlevelClient, person);
    //    ReadData(lowlevelClient);
    //}

    //private async void InsertData(ElasticLowLevelClient lowlevelClient, Person person)
    //{
    //    try
    //    {
    //        // var ndexResponse = lowlevelClient.Index<BytesResponse>("people", "1", PostData.Serializable(person));
    //        // byte[] responseBytes = ndexResponse.Body;

    //        var asyncIndexResponse = await lowlevelClient.IndexAsync<StringResponse>("people", "1", PostData.Serializable(person));
    //        string responseString = asyncIndexResponse.Body;

    //        Debug.Log("Dados inseridos com sucesso no banco de dados!" + responseString);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e.Message);
    //        Debug.Log("Deu erro no insert dos dados.");
    //    }
    //}

    //private void ReadData(ElasticLowLevelClient lowlevelClient)
    //{
    //    try
    //    {
    //        var searchResponse = lowlevelClient.Search<StringResponse>("people", PostData.Serializable(new
    //        {
    //            from = 0,
    //            size = 10,
    //            query = new
    //            {
    //                match = new
    //                {
    //                    field = "firstName",
    //                    query = "Martijn"
    //                }
    //            }
    //        }));

    //        var successful = searchResponse.Success;
    //        var responseJson = searchResponse.Body;

    //        Debug.Log("Search response: " + searchResponse);
    //        Debug.Log("Sucessful: " + successful);
    //        Debug.Log("Response JSON: " + responseJson);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e.Message);
    //        Debug.Log("Deu erro na leitura dos dados.");
    //    }
    //}
}

