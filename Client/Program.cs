// discover endpoints from metadata
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

var client = new HttpClient();
//дай настройку идентити сервера, больше про авторизацию чтобы получить доступ к api.
//GetDiscoveryDocumentAsync - описывает открытую конфигурацию IS
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

// request token
//получаем токен от идентити сервера
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    //прописан в config.cs identity serverа, список клиентов
    Address = disco.TokenEndpoint,
    ClientId = "first-client",
    ClientSecret = "secret",

    Scope = "test-api"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.Json);
Console.WriteLine("\n\n");

// call api
// обращаемся к апишке, передаем токен, разреши нам запрос
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

var response = await apiClient.GetAsync("https://localhost:7021/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JArray.Parse(content));
}