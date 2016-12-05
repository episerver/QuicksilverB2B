using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find;
using EPiServer.Logging;
using EPiServer.PlugIn;
using EPiServer.Reference.Commerce.Site.B2B.DomainServiceContracts;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Reference.Commerce.Site.B2B.Models.Search;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.B2B.Jobs
{
    [ScheduledPlugIn(
       DisplayName = "Users Index Job",
       Description = "Index users in the database ",
       SortIndex = 1)]
    public class UsersIndexJob : ScheduledJobBase
    {
        private readonly int _batchSize = 500;

        public Injected<ILogger> Logger { get; set; }
        public Injected<ICustomerDomainService> CustomerDomainService { get; set; }
        public Injected<IClient> Find { get; set; }

        public UsersIndexJob()
        {
            IsStoppable = false;
        }

        public override string Execute()
        {
            OnStatusChanged("Started execution.");

            try
            {
                IndexContacts();

                return "Done";
            }
            catch (Exception ex)
            {
                Logger.Service.Log(Level.Critical, ex.Message, ex);
                throw new Exception("Error: " + ex.Message);
            }
        }

        private void IndexContacts()
        {
            var batchNumber = 0;
            List<B2BContact> contacts;

            do
            {
                contacts = ReadContacts(batchNumber);

                var contactsToIndex = new List<UserSearchResultModel>(batchNumber);
                contactsToIndex.AddRange(contacts.Select(ConvertToUserSearchResultModel));

                try
                {
                    if (contactsToIndex.Count > 0)
                    {
                        Find.Service.Index(contactsToIndex);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Service.Log(Level.Error, ex.Message, ex);
                }

                int indexed = batchNumber * _batchSize + contacts.Count;
                OnStatusChanged($"Indexed {indexed} contacts");
                batchNumber++;
            } while (contacts.Count > 0);
        }

        private UserSearchResultModel ConvertToUserSearchResultModel(B2BContact contact) =>
            new UserSearchResultModel
            {
                ContactId = contact.ContactId,
                Email = contact.Email,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                FullName = contact.FullName
            };

        private List<B2BContact> ReadContacts(int batchNumber)
        {
            var customerService = CustomerDomainService.Service;
            return customerService.GetContacts()
                .OrderBy(x => x.ContactId)
                .Skip(_batchSize*batchNumber)
                .Take(_batchSize).ToList();
        }
    }
}
