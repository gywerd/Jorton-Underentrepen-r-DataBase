using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Mail;

namespace UkZipApi
{
    public class UkZipApi
    {
        #region Fields
        private HttpClient client = new HttpClient();
        private RootObject ukZipRootObject = new RootObject();

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public UkZipApi() { }

        #endregion

        #region Properties
        //public RootObject CvrRootRootObject { get => this.cvrRootRootObject; set => this.cvrRootRootObject = value; }

        #endregion

        #region Methods
        /// <summary>
        /// Method, that validates a UK postcode
        /// </summary>
        /// <param name="postcode">string</param>
        /// <returns>bool</returns>
        public bool ValidateUkZip(string postcode)
        {
            bool result = false;

            try
            {
                GetUkZipRootsObject(postcode);
                result = ukZipRootObject.result;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves JSON data from api.postcodes.io
        /// </summary>
        /// <param name="postcode">string</param>
        private void GetUkZipRootsObject(string postcode)
        {
            string query = $":{postcode}/validate";
            string baseUrl = @"https://api.postcodes.io/postcodes/";

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(Path.Combine(baseUrl, query));
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/18.17763";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            ukZipRootObject = JsonConvert.DeserializeObject<RootObject>(responseString);
        }

        #endregion

    }
}
