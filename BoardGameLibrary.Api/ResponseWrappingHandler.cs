using BoardGameLibrary.Api.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BoardGameLibrary.Api
{
    public class ResponseWrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Step 1: Wait for the Response
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        private HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            List<string> modelStateErrors = new List<string>();

            //Step 2: Get the Response Content
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;
                if (error != null)
                {
                    //Step 3: If content is an error, return nothing for the Result.
                    content = null; //We have errors, so don't return any content
                                    //Step 4: Insert the ModelState errors              
                    if (error.ModelState != null)
                    {
                        //Read as string
                        var httpErrorObject = response.Content.ReadAsStringAsync().Result;

                        //Convert to anonymous object
                        var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };

                        // Deserialize anonymous object
                        var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

                        // Get error messages from ModelState object
                        var modelStateValues = deserializedErrorObject.ModelState.Select(kvp => string.Join(". ", kvp.Value));

                        for (int i = 0; i < modelStateValues.Count(); i++)
                        {
                            modelStateErrors.Add(modelStateValues.ElementAt(i));
                        }
                    }
                    if (error.Count > 0)
                    {
                        modelStateErrors.AddRange(error.Select(e => e.Value.ToString()));
                    }
                }
            }

            //Step 5: Create a new response
            var newResponse = request.CreateResponse(response.StatusCode, new ValidatedResponseModel(content, modelStateErrors));

            //Step 6: Add Back the Response Headers
            foreach (var header in response.Headers) //Add back the response headers
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
}