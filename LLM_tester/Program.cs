// See https://aka.ms/new-console-template for more information
using System.Text;


Console.WriteLine("Hello, World!");

// read the API key from a config file
string apiKey = System.IO.File.ReadAllText("keys/openai-api-key.txt");

//get luminos api key from a config file
string luminosApiKey = System.IO.File.ReadAllText("keys/luminos-api-key.txt");

//ask the user to enter a prompt
Console.WriteLine("Enter a prompt: ");

//read the prompt from the console
string promptText = Console.ReadLine();
if(string.IsNullOrEmpty(promptText))
{
    Console.WriteLine("Prompt cannot be empty");
    return;
}

// ask the user to enter a service name
Console.WriteLine("Enter a service name: CG: ChatGPT or LU: Luminous");

//read the service name from the console
string serviceName = Console.ReadLine();


string response = "NO RESPONSE";

if(serviceName=="CG")
{
    //call chatGPT api and send the promptText as promt
    response = await AskChatGPT(promptText);

    //print  an ASCII art formatted string "RISPOSTA" to the console
    Console.WriteLine(@"
  _   _   _   _   _   _   _   _   _  
 / \ / \ / \ / \ / \ / \ / \ / \ / \ 
( R | I | S | P | O | S | T | A | ! )
 \_/ \_/ \_/ \_/ \_/ \_/ \_/ \_/ \_/");

    //print the response from the API
    Console.WriteLine(response);
}
else if (serviceName == "LU")
{
    //call luminos api and send the promptText as promt
    response = await AskLuminos(promptText);

    //print  an ASCII art formatted string "RISPOSTA" to the console
    Console.WriteLine(@"
  _   _   _   _   _   _   _   _   _  
 / \ / \ / \ / \ / \ / \ / \ / \ / \ 
( R | I | S | P | O | S | T | A | ! )
 \_/ \_/ \_/ \_/ \_/ \_/ \_/ \_/ \_/");

    //print the response from the API
    Console.WriteLine(response);
}
else
{
    Console.WriteLine("Service not found");
}
async Task<string> AskLuminos(string promptText)
{

    var requestBody = new
    {
        model = "luminous-base",
        prompt = promptText,
        maximum_tokens = 64
    };
        

    var requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
    var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + luminosApiKey);
    var response = await httpClient.PostAsync("https://api.aleph-alpha.com/complete", requestContent);
    var responseContent = await response.Content.ReadAsStringAsync();

    //parse the json response to an object
    LuminousCompletionTaskResponse  responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<LuminousCompletionTaskResponse>(responseContent);


    // Process the API response
    if (!response.IsSuccessStatusCode)
    {
        // The API request failed :(
        Console.WriteLine($"The API request failed with status code {response.StatusCode}");
        Console.WriteLine(responseContent);
        throw new Exception("API request failed");
    }
    return responseObject.GetCompletionText();
}


async Task<string> AskChatGPT(string name)
{
    if (string.IsNullOrEmpty(name))
    {
        throw new ArgumentException("message", nameof(name));
    }


    // Set the API endpoint
    string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

    // Prepare the request payload
    var requestBody = new
    {
        prompt = "Once upon a time",
        max_tokens = 100,
        temperature = 0.7
    };
    var requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
    var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

    // Prepare the HTTP client
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

    // Send the API request
    var response = await httpClient.PostAsync(apiUrl, requestContent);
    var responseContent = await response.Content.ReadAsStringAsync();

    // Process the API response
    if (!response.IsSuccessStatusCode)
    {
        // The API request failed :(
        Console.WriteLine($"The API request failed with status code {response.StatusCode}");
        Console.WriteLine(responseContent);
        throw new Exception("API request failed");
    }
    return responseContent;

}

