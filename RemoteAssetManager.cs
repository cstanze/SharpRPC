using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;

namespace SharpRPC
{
    public class RemoteAsset
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class RemoteAssetManager
    {
        private RestClient client;
        private Client app;
        private Logger Logger;

        public RemoteAssetManager(Client app)
        {
            client = new RestClient("https://discord.com");
            this.app = app;
            this.Logger = new Logger("RemoteAssetManager");
        }

        public RemoteAsset[] GetAssets(string clientSecret)
        {
            IRestRequest req = new RestRequest("/api/v8/oauth2/applications/" + app.Config.applicationID + "/assets")
                        .AddHeader("Authorization", clientSecret);
            IRestResponse<List<RemoteAsset>> res = client.Get<List<RemoteAsset>>(req);
            if (!res.IsSuccessful)
            {
                Logger.debug("Response Unsuccessful");
                return new RemoteAsset[0];
            }
            else
            {
                return res.Data.ToArray();
            }
        }

        public static string AssetTypeFromValue(int type)
        {
            switch(type)
            {
                case 1:
                    return "Rich Image Asset";
                default:
                    return "Unknown Asset Type";
            }
        }
    }
}
