using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace HR4You.Components.Handler;

public abstract class HttpQueryHandler
{
    private const string ErrorPageUri = "/Error";

    public static HttpRequestMessage CreateEndpointRequest(HttpMethod httpMethod, string endpoint,
        Dictionary<string, string> parameters)
    {
        endpoint = QueryHelpers.AddQueryString(endpoint, parameters!);
        var request = new HttpRequestMessage(httpMethod, endpoint);

        return request;
    }

    public static HttpRequestMessage CreateEndpointRequestWithJwt(HttpMethod httpMethod, string endpoint,
        Dictionary<string, string> parameters, string jwtToken)
    {
        endpoint = QueryHelpers.AddQueryString(endpoint, parameters!);
        var request = new HttpRequestMessage(httpMethod, endpoint);
        request.Headers.Add(HttpRequestHeader.Authorization.ToString(), "Bearer " + jwtToken);

        return request;
    }

    public static async Task<string?> ProcessHttpResponse(NavigationManager navigationManager,
        HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            navigationManager.NavigateTo(navigationManager.BaseUri);
            return null;
        }

        if (response.IsSuccessStatusCode) 
            return await response.Content.ReadAsStringAsync();
        
        navigationManager.NavigateTo(ErrorPageUri);
        return null;
    }
}