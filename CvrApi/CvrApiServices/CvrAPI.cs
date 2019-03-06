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

namespace CvrApiServices
{
    public class CvrAPI
    {
        #region Fields
        private HttpClient client;
        private RootObject cvrRootRootObject = new RootObject();
        private Builder tempBuilder = new Builder();
        private LegalEntity tempLegalEntity = new LegalEntity();
        private Entrepeneur tempEntrepeneur = new Entrepeneur();
        private List<ZipTown> zipTowns = new List<ZipTown>();

        #endregion

        #region Constructors
        public CvrAPI(List<ZipTown> zipTowns)
        {
            this.zipTowns = zipTowns;
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cvrapi.dk/");
        }

        #endregion

        #region Methods
        private void CheckEntrepeneurData(LegalEntity tempEntity)
        {
            if (tempEntrepeneur.Entity.Name.Remove(tempEntity.Name.Length) != tempEntity.Name)
            {
                tempEntrepeneur.Entity.Name = tempEntity.Name;
            }

            if (tempEntrepeneur.Entity.CoName != tempEntity.CoName)
            {
                tempEntrepeneur.Entity.CoName = tempEntity.CoName;
            }

            if (tempEntrepeneur.Entity.Address.Street != tempEntity.Address.Street)
            {
                tempEntrepeneur.Entity.Address.Street = tempEntity.Address.Street;
            }

            if (tempEntrepeneur.Entity.Address.Place != tempEntity.Address.Place)
            {
                tempEntrepeneur.Entity.Address.Place = tempEntity.Address.Place;
            }

            if (tempEntrepeneur.Entity.Address.ZipTown.Zip != tempEntity.Address.ZipTown.Zip)
            {
                tempEntrepeneur.Entity.Address.ZipTown = GetZipTown(tempEntity.Address.ZipTown.Zip);
            }

            if (tempEntrepeneur.Entity.Address.Street != tempEntity.Address.Street)
            {
                tempEntrepeneur.Entity.Address.Street = tempEntity.Address.Street;
            }

            if (tempEntrepeneur.Entity.ContactInfo.Phone != tempEntity.ContactInfo.Phone && tempEntity.ContactInfo.Phone != "")
            {
                tempEntrepeneur.Entity.ContactInfo.Phone = tempEntity.ContactInfo.Phone;
            }

            if (tempEntrepeneur.Entity.ContactInfo.Fax != tempEntity.ContactInfo.Fax && tempEntity.ContactInfo.Fax != "")
            {
                tempEntrepeneur.Entity.ContactInfo.Fax = tempEntity.ContactInfo.Fax;
            }

            if (tempEntrepeneur.Entity.ContactInfo.Email != tempEntity.ContactInfo.Email && tempEntity.ContactInfo.Email != "")
            {
                tempEntrepeneur.Entity.ContactInfo.Email = tempEntity.ContactInfo.Email;
            }

        }

        private void CheckNullStringFieldsCvr()
        {
            if (cvrRootRootObject.cityname == null)
            {
                cvrRootRootObject.cityname = "";
            }

            if (cvrRootRootObject.email == null)
            {
                cvrRootRootObject.email = "";
            }

            if (cvrRootRootObject.addressco == null)
            {
                cvrRootRootObject.addressco = "";
            }

        }

        private void CheckNullStringFieldsProductionUnit(Productionunit productionUnit)
        {
            

            if (productionUnit.cityname == null)
            {
                productionUnit.cityname = "";
            }

            if (productionUnit.phone == null)
            {
                productionUnit.phone = 0;
            }

            if (cvrRootRootObject.fax == null)
            {
                productionUnit.phone = 0;
            }

            if (productionUnit.email == null)
            {
                productionUnit.email = "";
            }

            if (productionUnit.addressco == null)
            {
                productionUnit.addressco = "";
            }

        }

        /// <summary>
        /// Method, that converts cvr data in r to a LegalEntity
        /// </summary>
        private void ConvertCvrDataToLegalEntity()
        {
            CheckNullStringFieldsCvr();
            string fax = "";
            string phone = "";

            if (cvrRootRootObject.fax != null)
            {
                fax = cvrRootRootObject.fax.ToString();
            }

            if (cvrRootRootObject.phone != null)
            {
                phone = cvrRootRootObject.phone.ToString();
            }

            this.tempLegalEntity = new LegalEntity(cvrRootRootObject.vat.ToString(), cvrRootRootObject.name, cvrRootRootObject.addressco, new Address(cvrRootRootObject.address, cvrRootRootObject.cityname, new ZipTown(cvrRootRootObject.zipcode.ToString(), cvrRootRootObject.city)), new ContactInfo(phone, fax, "", cvrRootRootObject.email), "");
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
                    CheckNullStringFieldsProductionUnit(productionUnit);
                    string fax = "";
                    string phone = "";

                    if (productionUnit.fax != null)
                    {
                        fax = productionUnit.fax.ToString();
                    }

                    if (productionUnit.phone != null)
                    {
                        phone = productionUnit.phone.ToString();
                    }

                    this.tempLegalEntity = new LegalEntity(productionUnit.pno.ToString(), productionUnit.name, productionUnit.addressco, new Address(productionUnit.address, productionUnit.cityname, new ZipTown(productionUnit.zipcode.ToString(), productionUnit.city)), new ContactInfo(phone, fax, "", productionUnit.email), "");
                }
                break;
            }
        }

        /// <summary>
        /// Method, that retrieves Legal Entity data
        /// </summary>
        /// <param name="cvr">string</param>
        /// <param name="legalEntity">LegalEntity</param>
        public void GetCvrData(string cvr, LegalEntity legalEntity)
        {
            this.tempLegalEntity = legalEntity;
            cvrRootRootObject = new RootObject();

            try
            {
                GetCvrRootsObjectAsync(cvr);
                Task.Delay(2200);
                GetLegalEntityFromRootObject(cvr);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Method, that retrieves JSON data from cvr-api.dk
        /// </summary>
        /// <param name="cvr">string</param>
        private async void GetCvrRootsObjectAsync(string cvr)
        {
            string query = $"api?search={cvr}&country=dk";
            client.Timeout = new TimeSpan(0,0,2);
            HttpResponseMessage responseMessageExchange = await client.GetAsync(query);

            string responseStream = await responseMessageExchange.Content.ReadAsStringAsync();

            JavaScriptSerializer js = new JavaScriptSerializer();
            cvrRootRootObject = JsonConvert.DeserializeObject<RootObject>(responseStream);
        }

        private LegalEntity GetLegalEntityFromCvrData()
        {
            CheckNullStringFieldsCvr();
            string fax = "";
            string phone = "";

            if (cvrRootRootObject.fax != null)
            {
                fax = cvrRootRootObject.fax.ToString();
            }

            if (cvrRootRootObject.phone != null)
            {
                phone = cvrRootRootObject.phone.ToString();
            }

            return new LegalEntity(cvrRootRootObject.vat.ToString(), cvrRootRootObject.name, cvrRootRootObject.addressco, new Address(cvrRootRootObject.address, cvrRootRootObject.cityname, new ZipTown(cvrRootRootObject.zipcode.ToString(), cvrRootRootObject.city)), new ContactInfo(phone, fax, "", cvrRootRootObject.email), "");
        }

        private LegalEntity GetLegalEntityFromProductionUnitData(string cvr)
        {
            LegalEntity result = new LegalEntity();

            foreach (Productionunit productionUnit in cvrRootRootObject.productionunits)
            {
                if (productionUnit.pno.ToString() == cvr)
                {
                    CheckNullStringFieldsProductionUnit(productionUnit);
                    string fax = "";
                    string phone = "";

                    if (productionUnit.fax != null)
                    {
                        fax = productionUnit.fax.ToString();
                    }

                    if (productionUnit.phone != null)
                    {
                        phone = productionUnit.phone.ToString();
                    }

                    result = new LegalEntity(productionUnit.pno.ToString(), productionUnit.name, productionUnit.addressco, new Address(productionUnit.address, productionUnit.cityname, new ZipTown(productionUnit.zipcode.ToString(), productionUnit.city)), new ContactInfo(phone, fax, "", productionUnit.email), "");
                }
                break;
            }

            return result;
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
                    ConvertCvrDataToLegalEntity();
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
        /// <returns></returns>
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
        /// <param name="cvr"></param>
        /// <param name="entrepeneur"></param>
        public void UpdateCvrData(Entrepeneur entrepeneur)
        {
            this.tempEntrepeneur = entrepeneur;
            cvrRootRootObject = new RootObject();

            try
            {
                GetCvrRootsObjectAsync(entrepeneur.Entity.Cvr);
                Task.Delay(2200);
                UpdateLegalEntityFromRootObject(entrepeneur.Entity.Cvr);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Method, that updates Entrepeneur data
        /// </summary>
        /// <param name="cvr"></param>
        /// <param name="builder"></param>
        public void UpdateCvrData(Builder builder)
        {
            this.tempBuilder = builder;
            cvrRootRootObject = new RootObject();

            try
            {
                GetCvrRootsObjectAsync(builder.Entity.Cvr);
                Task.Delay(2200);
                UpdateLegalEntityFromRootObject(builder.Entity.Cvr);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Method, that updates JSON data from cvr-api.dk 
        /// </summary>
        /// <param name="cvr"></param>
        private void UpdateLegalEntityFromRootObject(string cvr)
        {
            int cvrLength = cvr.Length;
            LegalEntity tempEntity = new LegalEntity();
            

            switch (cvrLength)
            {
                case 8:
                    tempEntity = GetLegalEntityFromCvrData();
                    break;
                case 10:
                    tempEntity = GetLegalEntityFromProductionUnitData(cvr);
                    break;
                default:
                    break;
            }

            UpdateTempEntrepeneur(tempEntity);

        }


        private void UpdateTempEntrepeneur(LegalEntity tempEntity)
        {
            bool active = true;

            if (cvrRootRootObject.enddate != null)
            {
                active = false;
            }

            switch (active.ToString())
            {
                case "false":
                    if (tempEntrepeneur.Active)
                    {
                        tempEntrepeneur.ToggleActive();
                    }
                    break;
                case "true":
                    CheckEntrepeneurData(tempEntity);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Fields
        //public RootObject CvrRootRootObject { get => this.cvrRootRootObject; set => this.cvrRootRootObject = value; }

        #endregion

    }
}
