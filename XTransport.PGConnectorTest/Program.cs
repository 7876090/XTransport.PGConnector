
using XTransport.PGConnector;
using XTransport.PGConnector.Collections;

QueryResult<object> result = await Connector.UpdateDatabaseTables();
if(!result.Success)
{
    Console.WriteLine(result.ErrorDescription);
}

Console.WriteLine("");
Console.WriteLine("All comands executed!");
Console.ReadLine();
