using SWP.Application.LegalSwp.ContactPeopleAdmin;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Radzen.Blazor;
using Radzen;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ClientDetailsPage : BlazorPageBase
    {
        public LegalBlazorApp App { get; private set; }
        private UpdateContactPerson UpdateContactPerson => serviceProvider.GetService<UpdateContactPerson>();
        private CreateContactPerson CreateContactPerson => serviceProvider.GetService<CreateContactPerson>();
        private DeleteContactPerson DeleteContactPerson => serviceProvider.GetService<DeleteContactPerson>();
        public ContactPersonViewModel SelectedContact { get; set; }
        public CreateContactPerson.Request NewContact { get; set; } = new CreateContactPerson.Request();
        public RadzenGrid<ContactPersonViewModel> ContactsGrid { get; set; }

        public ClientDetailsPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public void ContactSelected(object value)
        {
            var input = (ContactPersonViewModel)value;
            if (value != null)
            {
                SelectedContact = App.ActiveClientWithData.ContactPeople.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                SelectedContact = null;
            }
        }

        #region Contact

        private void RefreshSelectedContact() => SelectedContact = App.ActiveClientWithData.ContactPeople.FirstOrDefault(x => x.Id == SelectedContact.Id);

        public void EditContactRow(ContactPersonViewModel contact) => ContactsGrid.EditRow(contact);

        public async Task OnUpdateContactRow(ContactPersonViewModel contact)
        {
            try
            {
                var result = await UpdateContactPerson.UpdateForClient(new UpdateContactPerson.Request
                {
                    Id = contact.Id,
                    Address = contact.Address,
                    Email = contact.Email,
                    Name = contact.Name,
                    Surname = contact.Surname,
                    PhoneNumber = contact.PhoneNumber,
                    AlternativePhoneNumber = contact.AlternativePhoneNumber,
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName
                });

                App.ActiveClientWithData.ContactPeople[App.ActiveClientWithData.ContactPeople.FindIndex(x => x.Id == result.Id)] = result;

                await ContactsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveContactRow(ContactPersonViewModel contact) => ContactsGrid.UpdateRow(contact);

        public void CancelContactEdit(ContactPersonViewModel contact)
        {
            ContactsGrid.CancelEditRow(contact);
            App.RefreshClientWithData();
            RefreshSelectedContact();
        }

        public async Task DeleteContactRow(ContactPersonViewModel contact)
        {
            try
            {
                await DeleteContactPerson.DeleteForClient(contact.Id);
                App.ActiveClientWithData.ContactPeople.RemoveAll(x => x.Id == contact.Id);

                await ContactsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kontakt: {contact.Name} {contact.Surname} został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewContact(CreateContactPerson.Request request)
        {
            request.UpdatedBy = App.User.UserName;

            try
            {
                var result = await CreateContactPerson.CreateContactPersonForClient(App.ActiveClient.Id, request);
                NewContact = new CreateContactPerson.Request();

                App.ActiveClientWithData.ContactPeople.Add(result);
                await ContactsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        #endregion



    }
}
