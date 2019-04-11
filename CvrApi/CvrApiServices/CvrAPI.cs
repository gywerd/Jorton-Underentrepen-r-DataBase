using CvrApiRepository;
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
using System.Windows.Controls;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Mail;

namespace CvrApiServices
{
    public class CvrAPI
    {
        #region Fields
        private HttpClient client = new HttpClient();
        private RootObject cvrRootRootObject = new RootObject();
        private Builder tempBuilder = new Builder();
        private LegalEntity tempLegalEntity = new LegalEntity();
        private Entrepeneur tempEntrepeneur = new Entrepeneur();
        private List<ZipTown> zipTowns = new List<ZipTown>();
        private UpdateData updatedData = new UpdateData();

        #endregion

        #region Constructors
        public CvrAPI(List<ZipTown> zipTowns)
        {
            this.zipTowns = zipTowns;
        }

        #endregion

        #region Properties
        //public RootObject CvrRootRootObject { get => this.cvrRootRootObject; set => this.cvrRootRootObject = value; }

        #endregion

        #region Methods
        private void CheckEntityData()
        {
            if (updatedData.Entity.Name.Length > 0)
            {
                if (updatedData.Entity.Name.Length >= tempLegalEntity.Name.Length)
                {
                    string name = updatedData.Entity.Name;
                    if (updatedData.Entity.Name.Length > tempLegalEntity.Name.Length)
                    {
                        name = updatedData.Entity.Name.Remove(tempLegalEntity.Name.Length);
                    }

                    if (name != tempLegalEntity.Name)
                    {
                        updatedData.Entity.Name = tempLegalEntity.Name;
                    }
                }
            }


            if (updatedData.Entity.CoName.Length > 0)
            {
                if (updatedData.Entity.CoName.Length >= tempLegalEntity.CoName.Length && updatedData.Entity.CoName.Remove(tempLegalEntity.Name.Length - 1) != tempLegalEntity.CoName)
                {
                    updatedData.Entity.CoName = tempLegalEntity.CoName;
                }
            }

            if (updatedData.Entity.Address.Street.Length > 0)
            {
                if (updatedData.Entity.Address.Street != tempLegalEntity.Address.Street)
                {
                    updatedData.Entity.Address.Street = tempLegalEntity.Address.Street;

                    if (updatedData.Entity.Address.Place != tempLegalEntity.Address.Place)
                    {
                        updatedData.Entity.Address.Place = tempLegalEntity.Address.Place;
                    }

                    if (updatedData.Entity.Address.ZipTown.Zip != tempLegalEntity.Address.ZipTown.Zip)
                    {
                        updatedData.Entity.Address.ZipTown = GetZipTown(tempLegalEntity.Address.ZipTown.Zip);
                    }

                }
            }


            if (updatedData.Entity.ContactInfo.Phone != tempLegalEntity.ContactInfo.Phone && tempLegalEntity.ContactInfo.Phone != "")
            {
                bool invalidNumber = CheckPhoneNumber(tempLegalEntity.ContactInfo.Phone);
                if (!invalidNumber)
                {
                    updatedData.Entity.ContactInfo.Phone = tempLegalEntity.ContactInfo.Phone;
                }
            }

            if (updatedData.Entity.ContactInfo.Fax != tempLegalEntity.ContactInfo.Fax && tempLegalEntity.ContactInfo.Fax != "")
            {
                bool invalidNumber = CheckPhoneNumber(tempLegalEntity.ContactInfo.Fax);
                if (!invalidNumber)
                {
                    updatedData.Entity.ContactInfo.Fax = tempLegalEntity.ContactInfo.Fax;
                }
            }

            if (updatedData.Entity.ContactInfo.Email != tempLegalEntity.ContactInfo.Email && tempLegalEntity.ContactInfo.Email != "")
            {
                bool validEmail = tempLegalEntity.ContactInfo.CheckEmail(tempLegalEntity.ContactInfo.Email);
                if (!validEmail)
                {
                    updatedData.Entity.ContactInfo.Email = tempLegalEntity.ContactInfo.Email;
                }
            }

        }

        /// <summary>
        /// Method, that checks wether a phone number is valid
        /// </summary>
        /// <param name="phone">string</param>
        /// <returns>bool</returns>
        private bool CheckPhoneNumber(string phone)
        {
            bool result = true;

            if (phone.Length < 8)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Method, that converts productionUnit data from cvr-api to a LegalEntity
        /// </summary>
        /// <param name="cvr">string</param>
        private void ConvertProductionUnitDataToLegalEntity(string cvr)
        {
            foreach (Productionunit productionUnit in cvrRootRootObject.productionunits)
            {
                if (productionUnit.pno.ToString() == cvr)
                {
                    this.tempLegalEntity = new LegalEntity(productionUnit.pno.ToString(), productionUnit.name, productionUnit.addressco, new Address(productionUnit.address, productionUnit.cityname, new ZipTown(productionUnit.zipcode.ToString(), productionUnit.city)), new ContactInfo(productionUnit.phone.ToString(), productionUnit.fax.ToString(), "", productionUnit.email), "");
                }
                break;
            }
        }

        private bool CheckCvrRootRootObject()
        {
            bool result = false;

            if (cvrRootRootObject.name != null)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Method, that retrieves Legal Entity data
        /// </summary>
        /// <param name="cvr">string</param>
        /// <param name="legalEntity">LegalEntity</param>
        public LegalEntity GetCvrData(string cvr)
        {
            this.tempLegalEntity = new LegalEntity();

            try
            {
                GetCvrRootsObject(cvr);
                GetLegalEntityFromRootObject(cvr);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return this.tempLegalEntity;
        }

        /// <summary>
        /// Method, that retrieves JSON data from cvr-api.dk
        /// </summary>
        /// <param name="cvr">string</param>
        private void GetCvrRootsObject(string cvr)
        {
            string query = $"api?search={cvr}&country=dk";
            string baseUrl = @"https://cvrapi.dk/";

            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(Path.Combine(baseUrl, query));
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/18.17763";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            cvrRootRootObject = JsonConvert.DeserializeObject<RootObject>(responseString);
        }

        /// <summary>
        /// Method, that converts JSON dato in
        /// </summary>
        /// <param name="cvr">string</param>
        /// <returns>LegalEntity</returns>
        private void GetLegalEntityFromProductionUnitData()
        {
            

            foreach (Productionunit productionUnit in cvrRootRootObject.productionunits)
            {
                if (productionUnit.pno.ToString() == updatedData.Entity.Cvr)
                {
                    tempLegalEntity = new LegalEntity(productionUnit.pno.ToString(), productionUnit.name, productionUnit.addressco, new Address(productionUnit.address, productionUnit.cityname, new ZipTown(productionUnit.zipcode.ToString(), productionUnit.city)), new ContactInfo(productionUnit.phone.ToString(), productionUnit.fax.ToString(), "", productionUnit.email), "");
                }
                break;
            }

        }

        /// <summary>
        /// Method, retrieves a Legal Entity from a RootObject
        /// </summary>
        /// <param name="cvr"></param>
        private void GetLegalEntityFromRootObject(string cvr)
        {
            int cvrLength = cvr.Length;

            switch (cvrLength)
            {
                case 8:
                    this.tempLegalEntity = new LegalEntity(cvrRootRootObject.vat.ToString(), cvrRootRootObject.name, cvrRootRootObject.addressco, new Address(cvrRootRootObject.address, cvrRootRootObject.cityname, new ZipTown(cvrRootRootObject.zipcode.ToString(), cvrRootRootObject.city)), new ContactInfo(cvrRootRootObject.phone.ToString(), cvrRootRootObject.fax.ToString(), "", cvrRootRootObject.email), "");
                    break;
                case 10:
                    ConvertProductionUnitDataToLegalEntity(cvr);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Method that retrieves a valid ZipTown
        /// </summary>
        /// <param name="zip"></param>
        /// <returns>ZipTown</returns>
        private ZipTown GetZipTown(string zip)
        {
            ZipTown result = new ZipTown();

            foreach (ZipTown zipTown in zipTowns)
            {
                if (zipTown.Zip == zip)
                {
                    result = zipTown;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Method, that updates Entrepeneur data
        /// </summary>
        /// <param name="entity">LegalEntity</param>
        /// <returns>LegalEntity</returns>
        public UpdateData UpdateCvrData(LegalEntity entity)
        {
            updatedData.Entity = entity;
            cvrRootRootObject = new RootObject();

            try
            {
                GetCvrRootsObject(updatedData.Entity.Cvr);
                UpdateLegalEntityFromRootObject();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return updatedData;
        }

        /// <summary>
        /// Method, that updates JSON data from cvr-api.dk 
        /// </summary>
        /// <param name="cvr"></param>
        private void UpdateLegalEntityFromRootObject()
        {
            int cvrLength = updatedData.Entity.Cvr.Length;
            tempLegalEntity = new LegalEntity();
            

            switch (cvrLength)
            {
                case 8:
                    tempLegalEntity = new LegalEntity(cvrRootRootObject.vat.ToString(), cvrRootRootObject.name, cvrRootRootObject.addressco, new Address(cvrRootRootObject.address, cvrRootRootObject.cityname, new ZipTown(cvrRootRootObject.zipcode.ToString(), cvrRootRootObject.city)), new ContactInfo(cvrRootRootObject.phone.ToString(), cvrRootRootObject.fax.ToString(), "", cvrRootRootObject.email), "");
                    break;
                case 10:
                    GetLegalEntityFromProductionUnitData();
                    break;
                default:
                    break;
            }

            UpdateTempEntity();


        }


        private void UpdateTempEntity()
        {
            if (cvrRootRootObject.enddate != null)
            {
                updatedData.Active = false;
            }
            else
            {
                updatedData.Active = true;
            }

            CheckEntityData();

        }

        #endregion

    }
}
