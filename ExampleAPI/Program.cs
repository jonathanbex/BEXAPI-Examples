using ExampleAPI.ApiConnect;
using ExampleAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExampleAPI
{
  class Program
  {
    async static void Main(string[] args)
    {

      //Base path of api
      var baseUrl = "https://api.bexonline.com/api/";

      // First integration Information
      // Get From Bex
      var apiKey = "";
      var secret = "";

      //Choose location to query from
      //Either use a fixed value or choose one from the method https://developer.bexonline.com//api-details#api=bex-api&operation=WarehouseManagement_GetLocations
      var location = "";

      var apiConnection = new ApiConnection();

      //First request, Get queued stock entries
      //https://developer.bexonline.com//api-details#api=bex-api&operation=WarehouseManagement_GetQueuedStockChanges
      var stockchangesUrl = baseUrl + "WarehouseManagement/GetQueuedStockChanges";
      //Setup request
      var stockChangesRequest = await apiConnection.CreateConnection(stockchangesUrl, WebRequestMethods.Http.Get, apiKey, secret);
      var stockChangesResponse = await apiConnection.MakeRequest<QueuedStockChangesResult>(stockChangesRequest);

      var productStock = new List<ProductStock>();
      //check if we get any entries
      if (stockChangesResponse.stockChangeEntries.Count() == 0) return;


      foreach (var stockEntry in stockChangesResponse.stockChangeEntries)
      {
        //Make request per product to get stock
        //https://developer.bexonline.com//api-details#api=bex-api&operation=WarehouseManagement_GetStockinformation
        var getStockInformationUrl = string.Format("WarehouseManagement/GetStockinformation?locationCode={0}&productIdentifier={1}", new[] { location, stockEntry.productIdentifier });
        var getStockInformationRequest = await apiConnection.CreateConnection(getStockInformationUrl, WebRequestMethods.Http.Get, apiKey, secret);
        var getStockInformationResponse = await apiConnection.MakeRequest<ProductStock>(getStockInformationRequest);

        if (getStockInformationResponse != null)
        {
          productStock.Add(getStockInformationResponse);
        }

        //Send Confirmation To BeX that stock has been received
        //https://developer.bexonline.com//api-details#api=bex-api&operation=WarehouseManagement_ConfirmStockChange
        var confirmStockUrl = "WarehouseManagement/ConfirmStockChange";
        var jsonString = JsonConvert.SerializeObject(new { entryId = stockEntry.id });
        var confirmStockRequest = await apiConnection.CreateConnection(confirmStockUrl, WebRequestMethods.Http.Put, apiKey, secret);
        await apiConnection.MakeRequest<object>(confirmStockRequest, jsonString);


      }


      //Cycle done


    }
  }
}
