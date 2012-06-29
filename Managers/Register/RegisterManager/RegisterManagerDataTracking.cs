namespace RegOnline.RegressionTest.Managers.Register
{
    using System;
    using System.Collections.Generic;
    using RegOnline.RegressionTest.Attributes;

    public partial class RegisterManager : ManagerBase
    {
        #region Classes for tracking registration data

        //TODO: consider building out the response classes in the domain model name space.

        public enum CustomFieldCategoriesList
        {
            Invalid = 0,
            Custom_Field = 1,
            Agenda = 2,
            Event_Fee = 3,
        }

        public class CustomFieldResponses
        {
            public int RegistrationID { get; set; }

            public Dictionary<int, CustomFieldResponse> customFields { get; set; }

            public CustomFieldResponses()
            {
                this.customFields = new Dictionary<int, CustomFieldResponse>();
            }
        }

        /// <summary>
        /// purpose of this class is to track the responsed generated during the registration regression test.
        /// This is instead of having to load responses from the database when checking the attendee info page since it increase number of database
        /// hits while the tests run.
        /// </summary>
        public class CustomFieldResponse
        {
            public int category;
            public int customFieldId = -1;
            public string customFieldDescription = String.Empty;
            public string code = String.Empty;
            public string response = String.Empty;
            public string amount = String.Empty;
        }

        [Step]
        public void FillCustomFieldResponseList(
            int registrationID,
            List<CustomFieldResponses> targetList,
            int cfID,
            string description,
            int category,
            string response,
            string amount,
            string code)
        {
            CustomFieldResponse customFieldResponse = new CustomFieldResponse();
            customFieldResponse.code = code;
            customFieldResponse.category = category;
            customFieldResponse.customFieldDescription = description;
            customFieldResponse.customFieldId = cfID;
            customFieldResponse.response = response;
            customFieldResponse.amount = amount;

            if (targetList.Exists(s => s.RegistrationID == registrationID))
            {
                CustomFieldResponses existedResponses = targetList.Find(s => s.RegistrationID == registrationID);

                if (existedResponses.customFields == null)
                {
                    existedResponses.customFields = new Dictionary<int, CustomFieldResponse>();
                    
                    existedResponses.customFields.Add(
                        customFieldResponse.customFieldId, 
                        customFieldResponse);
                }
                else
                {
                    if (existedResponses.customFields.ContainsKey(customFieldResponse.customFieldId))
                    {
                        existedResponses.customFields[customFieldResponse.customFieldId] = customFieldResponse;
                    }
                    else
                    {
                        existedResponses.customFields.Add(
                        customFieldResponse.customFieldId,
                        customFieldResponse);
                    }
                }
            }
            else
            {
                CustomFieldResponses cfResponses = new CustomFieldResponses();
                cfResponses.RegistrationID = registrationID;
                cfResponses.customFields.Add(customFieldResponse.customFieldId, customFieldResponse);
                targetList.Add(cfResponses);
            }
        }

        public class MerchandiseResponses
        {
            public int RegistrationID { get; set; }

            public Dictionary<int, MerchandiseResponse> merchandises { get; set; }

            public MerchandiseResponses()
            {
                this.merchandises = new Dictionary<int, MerchandiseResponse>();
            }
        }

        /// <summary>
        /// Purpose of this class is to track responses to any Merchandise during the registration regression test.
        /// </summary>
        public class MerchandiseResponse
        {
            public int merchandiseId = -1;
            public string merchandiseDescription = String.Empty;
            public string code = string.Empty;
            public string response = String.Empty;
            public string amount = String.Empty;
        }

        public void FillMerchandiseResponseList(
            int registrationID,
            List<MerchandiseResponses> targetList,
            int merchID,
            string description,
            string response,
            string amount)
        {
            MerchandiseResponse merchandiseResponse = new MerchandiseResponse();
            merchandiseResponse.merchandiseDescription = description;
            merchandiseResponse.merchandiseId = merchID;
            merchandiseResponse.response = response;
            merchandiseResponse.amount = amount;

            if (targetList.Exists(s => s.RegistrationID == registrationID))
            {
                MerchandiseResponses existedResponses = targetList.Find(s => s.RegistrationID == registrationID);

                if (existedResponses.merchandises == null)
                {
                    existedResponses.merchandises = new Dictionary<int, MerchandiseResponse>();

                    existedResponses.merchandises.Add(
                        merchandiseResponse.merchandiseId,
                        merchandiseResponse);
                }
                else
                {
                    if (existedResponses.merchandises.ContainsKey(merchandiseResponse.merchandiseId))
                    {
                        existedResponses.merchandises[merchandiseResponse.merchandiseId] = merchandiseResponse;
                    }
                    else
                    {
                        existedResponses.merchandises.Add(
                        merchandiseResponse.merchandiseId,
                        merchandiseResponse);
                    }
                }
            }
            else
            {
                MerchandiseResponses merchResponses = new MerchandiseResponses();
                merchResponses.RegistrationID = registrationID;
                merchResponses.merchandises.Add(merchandiseResponse.merchandiseId, merchandiseResponse);
                targetList.Add(merchResponses);
            }
        }

        /// <summary>
        /// Purpose of this class is to track responses to lodging questions
        /// </summary>
        public class LodgingResponses
        {
            public DateTime ArrivalDate = DateTime.MinValue;
            public DateTime DepartureDate = DateTime.MinValue;

            public string RoomType = String.Empty;
            public string BedType = String.Empty;
            public string SmokingPreference = String.Empty;
        }

        /// <summary>
        /// Purpose of this class is to track responses to travel questions.
        /// </summary>
        public class TravelResponses
        {
            public string ArrivalAirline = String.Empty;
            public string ArrivalFlightNumber = String.Empty;
            public string DepartureAirline = String.Empty;
            public string DepartureFlightNumber = String.Empty;
        }

        /// <summary>
        /// Purpose of this class is to verify fees on confirmation page
        /// </summary>
        public class FeeResponse
        {
            public string FeeName = null;
            public string FeeQuantity = null;
            public string FeeUnitPrice = null;
            public string FeeAmount = null;

            public override bool Equals(object obj)
            {
                FeeResponse response = obj as FeeResponse;

                if (obj != null)
                {
                    return this.FeeName == response.FeeName &&
                        this.FeeAmount == response.FeeAmount &&
                        this.FeeQuantity == response.FeeQuantity &&
                        this.FeeUnitPrice == response.FeeUnitPrice;
                }

                return false;
            }
        }

        #endregion
    }
}