using Autofac;
using NtErp.Shared.Contracts.Repository;
using NtErp.Shared.Entities.CashJournal;
using NtErp.Shared.Services.Events;
using NtErp.Shared.Services.ViewModels;
using Prism.Events;
using System;
using System.Collections.ObjectModel;

namespace NtErp.Modules.Finances.ViewModels {
    public class CashJournalSearchViewModel : SearchViewModel {
        private IJournalBookRepository _cashJournalRepository;
        private ObservableCollection<CashJournal> _journals = new ObservableCollection<CashJournal>();


        #region Properties

        public ObservableCollection<CashJournal> Journals {
            get { return _journals; }
            set { _journals = value; RaisePropertyChanged(); }
        }

        #endregion


        #region Initialization

        public CashJournalSearchViewModel(IJournalBookRepository cashJournalRepository, IEventAggregator eventAggregator, ILifetimeScope scope) : base(scope, eventAggregator) {
            _cashJournalRepository = cashJournalRepository;

            RefreshJournals();
        }

        #endregion


        protected override void SelectCommand_OnExecute() {
            if (SelectedEntity != null) {
                DialogResult = true;
                SendResponseAndRequestCloseDialog();
            }
        }

        protected override void MouseDoubleClickCommand_OnExecute() {
            SelectCommand.Execute(null);
        }

        protected override void SearchCommand_OnExecute() {
            RefreshJournals();
        }

        protected override void ResetCommand_OnExecute() {
            throw new NotImplementedException();
        }

        protected override void CancelCommand_OnExecute() {
            DialogResult = false;
            SendResponseAndRequestCloseDialog();
        }

        private void RefreshJournals() {
            Journals.Clear();
            Journals.AddRange(_cashJournalRepository.Fetch());
        }

        private void SendResponseAndRequestCloseDialog() {
            long id = SelectedEntity != null ? SelectedEntity.Id : 0;

            _eventAggregator.GetEvent<PubSubEvent<EntitySearchResultEvent>>()
                            .Publish(new EntitySearchResultEvent(id, DialogResult));

            RaiseCloseDialogRequested();
        }
    }
}