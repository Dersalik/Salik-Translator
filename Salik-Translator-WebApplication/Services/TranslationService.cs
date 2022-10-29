using Newtonsoft.Json;
using Salik_Translator_WebApplication.Models;
using System.Text;

namespace Salik_Translator_WebApplication.Services
{
    public class TranslationService
    {
        private static readonly string location = "germanywestcentral";
        private static readonly string subscriptionKey = "1ed2a74e8de44e7c9693d78f2d4476a4";

        public TranslationService()
        {

        }
        public async Task<TranslationResult[]> GetTranslatation(string inputLanguage,string textToTranslate, string targetLanguage)
        {

            //string subscriptionKey = "1ed2a74e8de44e7c9693d78f2d4476a4";
            string apiEndpoint = "https://api.cognitive.microsofttranslator.com/";

            //if (string.IsNullOrEmpty(inputLanguage))
            //{
              var result= await detectLanguage(textToTranslate);
                inputLanguage = result[0].language;
            //}


            string route = $"/translate?api-version=3.0&from={inputLanguage}&to={targetLanguage}";
            string requestUri = apiEndpoint + route;
            TranslationResult[] translationResult = await TranslateText(subscriptionKey, requestUri, textToTranslate,inputLanguage);
            return translationResult;
        }

        public async Task<DetectedLanguage[]> detectLanguage(string text)
        {

            object[] body = new object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            string apiEndpoint = "https://api.cognitive.microsofttranslator.com/";
            string route = "Detect?api-version=3.0";
            string requestUri = apiEndpoint + route;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(requestUri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                DetectedLanguage[] deserializedOutput = JsonConvert.DeserializeObject<DetectedLanguage[]>(result);

                return deserializedOutput;
            }

        }
        async Task<TranslationResult[]> TranslateText(string subscriptionKey, string requestUri, string inputText,string inputLanguage)
        {
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(requestUri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);
                deserializedOutput[0].DetectedLanguage = new DetectedLanguage { language=inputLanguage};
                return deserializedOutput;
            }
        }

        public async Task<AvailableLanguage> GetAvailableLanguages()
        {
            string endpoint = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";
            var client = new HttpClient();
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(endpoint);
                var response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();

                AvailableLanguage deserializedOutput = JsonConvert.DeserializeObject<AvailableLanguage>(result);

                return deserializedOutput;
            }
        }
    }
}
