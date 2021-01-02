using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.ContactPeopleAdmin;
using SWP.Domain.Models.LegalApp;
using SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails.Actions;
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
        public List<ContactPersonViewModel> ContactPeople { get; set; } = new List<ContactPersonViewModel>();
    }

    [UIScopedService]
    public class ClientDetailsStore : StoreBase<ClientDetailsState>
    {
        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ClientDetailsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {

        }

        public void Initialize()
        {
            GetContactPeople(MainStore.GetState().ActiveClient.Id);
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case ContactSelectedAction.ContactSelected:
                    var contactSelectedAction = (ContactSelectedAction)action;
                    ContactSelected(contactSelectedAction.Arg);
                    break;
                case EditContactRowAction.EditContactRow:
                    var editContactRowAction = (EditContactRowAction)action;
                    EditContactRow(editContactRowAction.Arg);
                    break;
                case OnUpdateContactRowAction.OnUpdateContactRow:
                    var onUpdateContactRowAction = (OnUpdateContactRowAction)action;
                    await OnUpdateContactRow(onUpdateContactRowAction.Arg);
                    break;
                case SaveContactRowAction.SaveContactRow:
                    var saveContactRowAction = (SaveContactRowAction)action;
                    SaveContactRow(saveContactRowAction.Arg);
                    break;
                case CancelContactEditAction.CancelContactEdit:
                    var cancelContactEditAction = (CancelContactEditAction)action;
                    CancelContactEdit(cancelContactEditAction.Arg);
                    break;
                case DeleteContactRowAction.DeleteContactRow:
                    var deleteContactRowAction = (DeleteContactRowAction)action;
                    await DeleteContactRow(deleteContactRowAction.Arg);
                    break;
                case SubmitNewContactAction.SubmitNewContact:
                    var submitNewContactAction = (SubmitNewContactAction)action;
                    await SubmitNewContact(submitNewContactAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private void GetContactPeople(int clientId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getContactPeople = scope.ServiceProvider.GetRequiredService<GetContactPeople>();

                _state.ContactPeople = getContactPeople.GetClientContactPeople(clientId).Select(x => (ContactPersonViewModel)x).ToList();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void UpdateClientContactPerson(ClientContactPerson input) => _state.ContactPeople[_state.ContactPeople.FindIndex(x => x.Id == input.Id)] = input;

        private void RemoveClientContactPerson(int id) => _state.ContactPeople.RemoveAll(x => x.Id == id);

        private void AddClientContactPerson(ClientContactPerson input) => _state.ContactPeople.Add(input);

        private void ContactSelected(object value)
        {
            var input = (ContactPersonViewModel)value;
            if (value != null)
            {
                _state.SelectedContact = _state.ContactPeople.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedContact = null;
            }
        }

        #region Contact

        private void RefreshSelectedContact()
        {
            if (_state.SelectedContact != null && _state.ContactPeople.Any(x => x.Id == _state.SelectedContact.Id))
            {
                _state.SelectedContact = _state.ContactPeople.FirstOrDefault(x => x.Id == _state.SelectedContact.Id);
            }
            else
            {
                _state.SelectedContact = null;
            }
        }
           
        private void EditContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.EditRow(contact);

        private async Task OnUpdateContactRow(ContactPersonViewModel contact)
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
                    UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                });

                UpdateClientContactPerson(result);
                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został zmieniony.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void SaveContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.UpdateRow(contact);

        private void CancelContactEdit(ContactPersonViewModel contact)
        {
            _state.ContactsGrid.CancelEditRow(contact);
            MainStore.RefreshMainComponent();
            RefreshSelectedContact();
            BroadcastStateChange();
        }

        private async Task DeleteContactRow(ContactPersonViewModel contact)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteContactPerson = scope.ServiceProvider.GetRequiredService<DeleteContactPerson>();

                await deleteContactPerson.DeleteForClient(contact.Id);
                RemoveClientContactPerson(contact.Id);
   
                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kontakt: {contact.Name} {contact.Surname} został usunięty.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task SubmitNewContact(CreateContactPerson.Request request)
        {
            request.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createContactPerson = scope.ServiceProvider.GetRequiredService<CreateContactPerson>();

                var result = await createContactPerson.CreateContactPersonForClient(MainStore.GetState().ActiveClient.Id, request);
                _state.NewContact = new CreateContactPerson.Request();
                AddClientContactPerson(result);

                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został dodany.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        #endregion

        public void CleanUpStore()
        {
            _state.SelectedContact = null;
        }

        public void RefreshSore()
        {
            GetContactPeople(MainStore.GetState().ActiveClient.Id);
        }
    }
}
