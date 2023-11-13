using IntelliTect.Coalesce.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests.Integration.Helpers;
public static class HttpClientExtensions
{
    /// <summary>
    /// Sends a POST request to the specified Uri with the given entity as form data.
    /// </summary>
    public static async Task<HttpResponseMessage> PostAsFormDataAsync<T>(
        this HttpClient client,
        string path,
        T entity
    ) where T : class
    {
        using HttpContent formData = entity.AsFormData();
        return await client.PostAsync(path, formData);
    }

    /// <summary>
    /// Converts the given object into HTTP form data.
    /// </summary>
    public static HttpContent AsFormData<T>(this T entity) where T : class
    {
        if (entity is null)
        {
            throw new Exception("Could not form url encoded content from null content");
        }

        return new FormUrlEncodedContent(entity.ToDictionary());
    }

    /// <summary>
    /// Converts an object into key-value pairs.
    /// </summary>
    /// <remarks>
    /// Taken from https://gunnarpeipman.com/serialize-url-encoded-form-data/.
    /// </remarks>
    private static IDictionary<string, string> ToDictionary(this object entity)
    {
        if (entity is not JToken token)
        {
            token = JObject.FromObject(entity, new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        var result = new Dictionary<string, string>();
        if (token.HasValues)
        {
            foreach (JToken child in token.Children().ToList())
            {
                if (child is JProperty property && entity is ISparseDto dto && !dto.ChangedProperties.Contains(property.Name))
                {
                    // This is a property on a coalesce DTO that was never assigned a value.
                    // This means it should be skipped for serialization, e.g. a property omitted from a surgical save.
                    continue;
                }

                foreach (var nestedItem in child.ToDictionary())
                {
                    result.Add(nestedItem.Key, nestedItem.Value);
                }
            }
            return result;
        }

        // Skip empty arrays, as they will have HasValues == false but are not a scalar type.
        if (token is JArray array && array.Count == 0) return result;

        JValue jValue = (JValue)token;

        result[token.Path] = jValue.Type switch
        {
            JTokenType.Null => "", // Nulls are represented in form data as *nothing*.
            JTokenType.Date => jValue.ToString("o", CultureInfo.InvariantCulture),
            JTokenType.Bytes => Convert.ToBase64String((byte[])jValue!),
            _ => jValue.ToString(CultureInfo.InvariantCulture),
        };

        return result;
    }
}
