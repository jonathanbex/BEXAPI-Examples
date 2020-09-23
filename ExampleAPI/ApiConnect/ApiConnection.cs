using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAPI.ApiConnect
{
  public class ApiConnection
  {

    //Setup Connection Request
    public async Task<HttpWebRequest> CreateConnection(string url, string method, string username, string password)
    {

      var request = (HttpWebRequest)WebRequest.Create(url);

      if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
      {
        throw new Exception("Missing username or password");
      }

      var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(username + ":" + password);
      var base64EncodedToken = System.Convert.ToBase64String(plainTextBytes);
      request.Method = method;
      request.ContentType = "application/json";
      request.PreAuthenticate = true;
      request.Headers.Add("Authorization", "Basic " + base64EncodedToken);
      return request;
    }

    //Make Request generic
    //Pass through what model you want to parse
    //optional parameter of json to pass through
    public async Task<T> MakeRequest<T>(HttpWebRequest request, string json = null)
    {
      try
      {
        if (json != null)
        {
          using (var streamWriter = new StreamWriter(request.GetRequestStream()))
          {
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
          }
        }
        var responsen = "";
        using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
          responsen = await reader.ReadToEndAsync();
        }

        try
        {
          var result = JsonConvert.DeserializeObject<T>(responsen);
          return (T)result;
        }

        catch (Exception e)
        {
          throw e;
        }

      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
