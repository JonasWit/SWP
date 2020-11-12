using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.ContactPeopleAdmin;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails
{
    public class ClientDetailsState
    {
        public ContactPersonViewModel SelectedContact { get; set; }
        public CreateContactPerson.Request NewContact { get; set; } = new CreateContactPerson.Request();
        public RadzenGrid<ContactPersonViewModel> ContactsGrid { get; set; }
    }

    [UIScopedService]
    public class ClientDetailsStore : StoreBase
    {
        private readonly ClientDetailsState _state;

        public ClientDetailsState GetState() => _state;

        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public DialogService DialogService { get; }

        public ClientDetailsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _state = new ClientDetailsState();
        }

        public void Initialize()
        {

        }

        public void ResetSelections()
        {
            _state.SelectedContact = null;
        }

        public void ContactSelected(object value)
        {
            var input = (ContactPersonViewModel)value;
            if (value != null)
            {
                _state.SelectedContact = MainStore.GetState().ActiveClient.ContactPeople.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedContact = null;
            }
        }

        #region Contact

        private void RefreshSelectedContact() => _state.SelectedContact = MainStore.GetState().ActiveClient.ContactPeople.FirstOrDefault(x => x.Id == _state.SelectedContact.Id);

        public void EditContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.EditRow(contact);

        public async Task OnUpdateContactRow(ContactPersonViewModel contact)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateContactPerson = scope.ServiceProvider.GetRequiredService<UpdateContactPerson>();

                var result = await updateContactPerson.UpdateForClient(new UpdateContactPerson.Request
                {
                    Id = contact.Id,
                    Address = contact.Address,
                    Email = contact.Email,
                    Name = contact.Name,
                    Surname = contact.Surname,
                    PhoneNumber = contact.PhoneNumber,
                    AlternativePhoneNumber = contact.AlternativePhoneNumber,
                    Updated = DateTime.Now,
                    UpdatedBy = MainStore.GetState().User.UserName
                });

                MainStore.UpdateClientContactPerson(result);
                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }
        public void SaveContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.UpdateRow(contact);

        public void CancelContactEdit(ContactPersonViewModel contact)
        {
            _state.ContactsGrid.CancelEditRow(contact);
            MainStore.RefreshActiveClientData();
            RefreshSelectedContact();
            BroadcastStateChange();
        }

        public async Task DeleteContactRow(ContactPersonViewModel contact)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteContactPerson = scope.ServiceProvider.GetRequiredService<DeleteContactPerson>();

                await deleteContactPerson.DeleteForClient(contact.Id);
                MainStore.RemoveClientContactPerson(contact.Id);
   
                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kontakt: {contact.Name} {contact.Surname} został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public async Task SubmitNewContact(CreateContactPerson.Request request)
        {
            request.UpdatedBy = MainStore.GetState().User.UserName;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createContactPerson = scope.ServiceProvider.GetRequiredService<CreateContactPerson>();

                var result = await createContactPerson.CreateContactPersonForClient(MainStore.GetState().ActiveClient.Id, request);
                _state.NewContact = new CreateContactPerson.Request();
                MainStore.AddClientContactPerson(result);

                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }

        #endregion







    }
}
