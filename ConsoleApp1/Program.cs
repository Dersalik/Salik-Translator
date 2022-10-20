﻿using System.Text;
using Newtonsoft.Json;

    string key = "1ed2a74e8de44e7c9693d78f2d4476a4";
      string endpoint = "https://api.cognitive.microsofttranslator.com";

    // location, also known as region.
    // required if you're using a multi-service or regional (not global) resource. It can be found in the Azure portal on the Keys and Endpoint page.
     string location = "germanywestcentral";

 
        // Input and output languages are defined as parameters.
        string route = "/translate?api-version=3.0&from=en&to=fr&to=zu";
        string textToTranslate = "I would really like to drive your car around the block a few times!";
        object[] body = new object[] { new { Text = textToTranslate } };
        var requestBody = JsonConvert.SerializeObject(body);

using (var client = new HttpClient())
using (var request = new HttpRequestMessage())
{
    // Build the request.
    request.Method = HttpMethod.Post;
    request.RequestUri = new Uri(endpoint + route);
    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
    request.Headers.Add("Ocp-Apim-Subscription-Key", key);
    // location required if you're using a multi-service or regional (not global) resource.
    request.Headers.Add("Ocp-Apim-Subscription-Region", location);

    // Send the request and get response.
    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
    // Read response as a string.
    string result = await response.Content.ReadAsStringAsync();
    Console.WriteLine(result);
}
