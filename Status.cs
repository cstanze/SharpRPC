using System;
using System.Collections.Generic;
using DiscordRPC;

public class SharpAsset
{
    public string key { get; set; }
    public string text { get; set; }
}

public class SharpButton
{
    public string label { get; set; }
    public string url { get; set; }
}

public class SharpStatus
{
    public SharpAsset largeImage { get; set; }
    public SharpAsset smallImage { get; set; }
    public string details { get; set; }
    public string state { get; set; }
    public SharpButton[] buttons { get; set; }

    public RichPresence ConvertToDiscord()
    {
        if(buttons == null)
        {
            buttons = new SharpButton[0];
        }

        List<Button> btns = new List<Button>();
        for (int idx = 0; idx < Math.Min(buttons.Length, 2); idx++)
        {
            Button b = new Button();
            b.Label = buttons[idx].label;
            b.Url = buttons[idx].url;
            btns.Add(b);
        }

        if (largeImage == null)
        {
            largeImage = new SharpAsset();
        }

        if(smallImage == null)
        {
            smallImage = new SharpAsset();
        }

        return new RichPresence() {
            Details = details,
            State = state,
            Assets = new Assets()
            {
                LargeImageKey = largeImage.key,
                LargeImageText = largeImage.text,
                SmallImageKey = smallImage.key,
                SmallImageText = smallImage.text
            },
            Buttons = btns.ToArray()
        };
    }
}
