using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{
    internal class InvidiousHandler
    {

        // Create a struct to store items from Invidious
        public struct VideoItem
        {
            public string title;
            public string description;
            public string identifier;
            public Int64 views;
        }

        // Function to perform searches on Invidious
        public static List<VideoItem> Search(string query, int resultsNum)
        {
            // Ignore SSL Errors
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };

            //If query is blank, error out
            if (query == "")
            {
                MessageBox.Show("Error 02: Search queries cannot be blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Run the below code for the number of results needed, resultsNum / 20

            // Create a list of VideoItems
            List<VideoItem> results = new List<VideoItem>();
            
            for (int i = 1; i <= resultsNum/20; i++)
            {
                // Create search url using Invidious API
                string search_url = Properties.Settings.Default.InvidiousInstance + "/api/v1/search?q=" + query + "&type=video" + "&page=" + i;
                string results_json = "";


                // Try to download JSON response from search_url
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        // Download JSON response from search_url
                        results_json = client.DownloadString(search_url);
                    }
                }
                catch
                {
                    // Show message that search failed
                    MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection. Additionally, Your Invidious instance may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                try
                {
                    // Try to parse JSON response into JSON array
                    JArray results_array = JArray.Parse(results_json);

                    foreach (var item in results_array)
                    {
                        // Create a new ArchiveItem
                        VideoItem result = new VideoItem
                        {
                            title = item["title"].ToString(),
                            identifier = item["videoId"].ToString(),
                            description = item["description"].ToString(),
                            views = (Int64)item["viewCount"]
                        };

                        // Convert video length to TimeSpan
                        TimeSpan videoLength = TimeSpan.FromSeconds((double)item["lengthSeconds"]);

                        // Create string to store length in seconds and convert from seconds to MM:SS
                        string lengthInSeconds = string.Format("{0:D2}:{1:D2}", videoLength.Minutes, videoLength.Seconds);

                        // Create item description based on metadata (author, publishedtext, length in seconds converted to MM:SS)
                        result.description = "Author: " + item["author"].ToString() + Environment.NewLine + "Published: " + item["publishedText"].ToString() +
                            Environment.NewLine + "Length: " + lengthInSeconds + Environment.NewLine + "Views: " + result.views.ToString("N0");

                        // Check if an equivalent item already exists in the results list
                        bool exists = false;
                        foreach (var existingItem in results)
                        {
                            if (existingItem.identifier == result.identifier)
                            {
                                exists = true;
                            }
                        }

                        // Add VideoItem to results list if it is not already in the list
                        if (!exists)
                        {
                            results.Add(result);
                        }
                    }
                }
                catch
                {
                    // Show message that search failed
                    MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection. Additionally, Your Invidious instance may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                // If results list is empty, error out and return null
                if (results.Count == 0)
                {
                    MessageBox.Show("Error 03: No results found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            return results;
        }

        // Function to get video thumbnail
        public static string GetThumbnailUrl (string identifier)
        {
            return Properties.Settings.Default.InvidiousInstance + "/vi/" + identifier + "/mqdefault.jpg";
        }

        // Function to get video URL based on identifier and default quality
        public static string GetVideoUrl(string identifier)
        {
            /*// List to define qualities
            List<string> qualityList = new List<string> { "240p", "360p", "480p", "720p", "1080p" };
            
            // Get video metadata using Invidious API
            string metadata_url = Properties.Settings.Default.InvidiousInstance + "/api/v1/videos/" + identifier;

            // Store metadata JSON
            string metadata_json = "";

            // Try to download JSON response from metadata_url
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Download JSON response from search_url
                    metadata_json = client.DownloadString(metadata_url);
                }
            }
            catch
            {
                // Show message that search failed
                MessageBox.Show("Error 01: Error downloading metadata. Please check your Internet connection. Additionally, Your Invidious instance may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Parse JSON response into JObject
            JObject metadata_obj = JObject.Parse(metadata_json);

            // Parse needed metadata (formatStreams) into JArray
            JArray metadata_array = (JArray)metadata_obj["formatStreams"];

            // Find index of item in qualityList with same value as defaultQuality
            int qualityIndex = qualityList.FindIndex(a => a.Contains(Properties.Settings.Default.DefaultQuality));

            // Iterate through the array starting at desired quality and decreasing from there. If a quality is found, return the URL
            for (int i = qualityIndex; i >= 0; i--)
            {
                // Iterate through the array
                foreach (var item in metadata_array)
                {
                    // If the quality of the item matches the quality in the qualityList, return the URL
                    if (item["resolution"].ToString() == qualityList[i] && item["container"].ToString() == "mp4")
                    {
                        return item["url"].ToString();
                    }
                }
            }

            // Iterate through the array starting at desired quality and increasing from there. If a quality is found, return the URL
            for (int i = qualityIndex; i < qualityList.Count; i++)
            {
                // Iterate through the array
                foreach (var item in metadata_array)
                {
                    // If the quality of the item matches the quality in the qualityList, return the URL
                    if (item["resolution"].ToString() == qualityList[i] && item["container"].ToString() == "mp4")
                    {
                        return item["url"].ToString();
                    }
                }
            }

            // If no MP4 items exist, return null
            return null;*/

            return "https://youtube.com/watch?v=" + identifier;

        }
    }
}
