using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBizz
{
    public class IttLetterPdfLists
    {
        private IttLetterPdfData ittLetterPdfData;
        private List<Enterprise> enterprises;
        private List<Description> descriptionList;
        private List<IttLetterReceiver> ittLetterReceivers;
        private List<IttLetterShipping> ittLetterShippingList;
        private List<BluePrint> bluePrints;
        private List<TimeSchedule> timeSchedules;
        private List<Miscellaneous> miscellaneusList;

        private MyEntityFrameWork BFW;


        public IttLetterPdfLists() { }

        public IttLetterPdfLists(MyEntityFrameWork bfw, IttLetterPdfData pdfData)
        {
            this.BFW = bfw;
            this.ittLetterPdfData = pdfData;

            this.enterprises = BFW.GetEnterprises(ittLetterPdfData.Project.Id);
            this.descriptionList = BFW.GetDescriptions(ittLetterPdfData.Project.Id);
            this.ittLetterReceivers = BFW.GetIttLetterReceivers(ittLetterPdfData.Project.Id);
            this.ittLetterShippingList = BFW.GetIttLetterShippingList(ittLetterPdfData.Project.Id);
            this.bluePrints = BFW.GetBluePrints(ittLetterPdfData.Project.Id);
            this.timeSchedules = BFW.GetTimeSchedules(ittLetterPdfData.Project.Id);
            this.miscellaneusList = BFW.GetMiscellaneousList(ittLetterPdfData.Project.Id);


        }
    }
}
