using System;
using System.Text;
using Leaf.xNet;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Net;
using Newtonsoft.Json;


namespace TTS_Tool
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Text To Speech By SteamKey | Nulled.to";
            Console.WriteLine("Nulled Auth Key?");
            var code = Console.ReadLine();
            CreateMD5(code);
            var codeMD5 = CreateMD5(code);
            var val = Validate(code);
            if (val != null && val.auth != false)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fail");
                Console.ReadKey();
                return;
            }


            HttpRequest req = new HttpRequest();
            {

                for (int i = 0; i < 1; i++)
                {
                    new Thread(() =>
                    {
                        while (true)
                        {
                            TTS();
                        }
                    }).Start();
                }
            }
        }

        public static response Validate(string auth)
        {
            response resp = null;
            using (WebClient wc = new WebClient())
            {
                string ur = $"https://www.nulled.to/misc.php?action=validateKey&authKey={CreateMD5(auth)}";
                string json = wc.DownloadString(ur);
                resp = JsonConvert.DeserializeObject<response>(json);
                if (resp != null)
                {
                    resp.KeyUsed = auth;
                }
            }
            return resp;
        }
        public static string CreateMD5(string input)
        {

            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public class response
        {
            #region Public Properties

            public bool auth { get; set; }

            public string KeyUsed { get; set; }

            public string status { get; set; }

            #endregion Public Properties
        }
        static void TTS()
        {
            using (var req = new HttpRequest())
            {
                try
                {
                    byte[] bytes = new byte[5];
                    Stream inputStream = Console.OpenStandardInput(bytes.Length);
                    Console.SetIn(new StreamReader(inputStream));
                    // The options etc
                    Console.WriteLine("What voice would you like to use?");
                    string Voice = Console.ReadLine();
                    Console.WriteLine("What do you want to say?");
                    string Text = Console.ReadLine();
                    Console.WriteLine("What do you want the .mp3 file to be called?");
                    string MP3Name = Console.ReadLine();

                    req.UserAgent = Http.ChromeUserAgent();

                    // Headers
                    req.AddHeader("Accept", "*/*");
                    req.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                    // Post Data
                    var postdata = "msg=" + Text + "&lang=" + Voice + "&source=ttsmp3";
                    byte[] postBytes = Encoding.ASCII.GetBytes(postdata);

                    // Posting the post data
                    string res = req.Post("https://ttsmp3.com/makemp3.php", postBytes).ToString();

                    // Capture MP3 Part from page
                    string mp3 = Regex.Match(res, "(?<=\"MP3\":\")(.*?)(?=\"})").ToString();

                    // Download path and where to download from
                    string path = Environment.CurrentDirectory + @"\Export\" + MP3Name + ".mp3";
                    string download = "https://ttsmp3.com/created_mp3/" + mp3;


                    // Downloading
                    WebClient Client = new WebClient();
                    Client.DownloadFile(download, @path);

                    // Output
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n.mp3 file called " + MP3Name + " Has been exported with the voice " + Voice + ".");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to use the tool again");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch
                {

                }
            }
        }
    }
}
