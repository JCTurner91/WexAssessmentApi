// Authentication pieces pulled and adapted from official Duende IdentityServer documenation and QuickGuide.
// QuickGuide found here: https://docs.duendesoftware.com/identityserver/v6/quickstarts/1_client_credentials/

// discover endpoints from metadata
using IdentityModel.Client;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WexAssessmentApi.Models;

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,

    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

//Console.WriteLine(tokenResponse.AccessToken);

// call api
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);
string baseUrl = "https://localhost:7153";

// Getting initial dummy inventory
Console.WriteLine("\nInitial Inventory:");
var getAllResponse = await apiClient.GetAsync($"{baseUrl}/api/products");
if (!getAllResponse.IsSuccessStatusCode)
{
    Console.WriteLine(getAllResponse.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await getAllResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// Getting a single item from intial dummy inventory
Console.WriteLine("\nProduct with Id = 3:");
var getOneResponse = await apiClient.GetAsync($"{baseUrl}/api/products/3");
if (!getOneResponse.IsSuccessStatusCode)
{
    Console.WriteLine(getOneResponse.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await getOneResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// Getting a single item not found in intial dummy inventory
Console.WriteLine("\nProduct with Id = 10:");
getOneResponse = await apiClient.GetAsync($"{baseUrl}/api/products/10");
if (!getOneResponse.IsSuccessStatusCode)
{
    Console.WriteLine(getOneResponse.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await getOneResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// Adding to dummy inventory
Console.WriteLine("\nAdding Plushies:");
var createResponse = await apiClient.PostAsJsonAsync($"{baseUrl}/api/products", new Product()
{
    Id = 4,
    Name = "Plushies",
    Price = 25,
    Category = "Toys",
    StockQuantity = 15,
});
if (!createResponse.IsSuccessStatusCode)
{
    Console.WriteLine(createResponse.StatusCode);
}
else
{
    // Getting and printing new inventory
    getAllResponse = await apiClient.GetAsync($"{baseUrl}/api/products");
    var doc = JsonDocument.Parse(await getAllResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// Adding invalid item to dummy inventory
Console.WriteLine("\nAdding Dog Food:");
createResponse = await apiClient.PostAsJsonAsync($"{baseUrl}/api/products", new Product()
{
    Id = 4,
    Name = "Dog Food",
    Price = -10,
    Category = "Pet Supplies",
    StockQuantity = 1,
});
if (!createResponse.IsSuccessStatusCode)
{
    Console.WriteLine(createResponse.StatusCode);
}
else
{
    // Getting and printing new inventory
    getAllResponse = await apiClient.GetAsync($"{baseUrl}/api/products");
    var doc = JsonDocument.Parse(await getAllResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// Updating dummy inventory
Console.WriteLine("\nReplacing Toilet Paper with Trash Bags:");
var updateResponse = await apiClient.PutAsJsonAsync($"{baseUrl}/api/products/3", new Product()
{
    Id = 3,
    Name = "Trash Bags",
    Price = 7,
    Category = "Household Goods",
    StockQuantity = 25,
});
if (!updateResponse.IsSuccessStatusCode)
{
    Console.WriteLine(updateResponse.StatusCode);
}
else
{
    // Getting and printing new inventory
    getAllResponse = await apiClient.GetAsync($"{baseUrl}/api/products");
    var doc = JsonDocument.Parse(await getAllResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// Updating dummy inventory with item that doesn't already exist
Console.WriteLine("\nReplacing item that doesn't exist with Dog Food:");
updateResponse = await apiClient.PutAsJsonAsync($"{baseUrl}/api/products/3", new Product()
{
    Id = 7,
    Name = "Dog Food",
    Price = 25,
    Category = "Pet Supplies",
    StockQuantity = 30,
});
if (!updateResponse.IsSuccessStatusCode)
{
    Console.WriteLine(updateResponse.StatusCode);
}
else
{
    // Getting and printing new inventory
    getAllResponse = await apiClient.GetAsync($"{baseUrl}/api/products");
    var doc = JsonDocument.Parse(await getAllResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

// Deleting from dummy inventory
Console.WriteLine("\nRemoving Toilet Paper:");
var deleteResponse = await apiClient.DeleteAsync($"{baseUrl}/api/products/3");
if (!deleteResponse.IsSuccessStatusCode)
{
    Console.WriteLine(deleteResponse.StatusCode);
}
else
{
    // Getting and printing new inventory
    getAllResponse = await apiClient.GetAsync($"{baseUrl}/api/products");
    var doc = JsonDocument.Parse(await getAllResponse.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

Console.ReadLine();