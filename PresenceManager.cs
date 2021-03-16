using System;
using DiscordRPC;
using DiscordRPC.Message;
using System.Threading;

namespace SharpRPC
{
    public class PresenceManager
    {
        private Logger Logger;
        private Client app;
        private DiscordRpcClient client;
        private int currentStatus = 0;

        public PresenceManager(Client app)
        {
            this.app = app;
            this.Logger = new Logger("PresenceManager");
            ConnectPresence();
        }

        public void StartPresence()
        {
            int timeBefore = DateTime.UtcNow.Millisecond;
            Logger.debug("Starting presence loop at a rate of " + app.Config.updateInterval + " seconds.");
            while(true)
            {
                try
                {
                    client.SetPresence(app.Config.statusList[this.currentStatus % app.Config.statusList.Count].ConvertToDiscord());
                    app.LoadedPresence = true;
                    Logger.info("Successfully set presence");
                }
                catch (Exception ex)
                {
                    app.LoadedPresence = false;
                    Logger.exception("Exception occurred while setting the presence from config: " + ex.Message);
                    Logger.debug(ex.StackTrace);
                    Logger.debug(ex.HelpLink);
                    client.ClearPresence();
                    Logger.info("Cleared discord presence due to failed connection");
                }
                Console.WriteLine("");

                int timeElapsed = DateTime.UtcNow.Millisecond - timeBefore;
                Thread.Sleep(Math.Max(app.Config.updateInterval * 1000 - timeElapsed, 0));
                Logger.info("Reloading config . . .");
                app.ReloadConfig();
                this.currentStatus++;
            }
        }

        private void DisconnectClient()
        {
            if(client != null)
            {
                Logger.info("Disposing client (possibly due to reconnection attempt)");
                client.Dispose();
            }
        }

        private void ConnectPresence()
        {
            try
            {
                client = new DiscordRpcClient(app.Config.applicationID);
                client.Initialize();
                client.OnConnectionFailed += OnDiscordDisconnect;
            } catch(Exception ex)
            {
                Logger.exception("Failed to connect to discord " + ex.Message);
            }
        }

        private void OnDiscordDisconnect(object sender, ConnectionFailedMessage args)
        {
            Logger.exception("Discord got disconnected . . .");
            Logger.info("Connecting to Discord . . .");

            DisconnectClient();
            ConnectPresence();
        }
    }
}
