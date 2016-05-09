﻿using Autofac;
using System.Diagnostics;

namespace NtErp.Modules.CashJournal {
    public class CashJournalModuleConfiguration : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<CashJournalModule>();

            builder.RegisterType<ViewModels.JournalBookViewModel>();
            builder.RegisterType<Views.JournalBookView>();

            builder.RegisterType<ViewModels.JournalEntryViewModel>();
            builder.RegisterType<Views.JournalEntryView>();

            builder.RegisterType<Shared.Services.ViewModels.MenuItemViewModel>()
                   .As<Shared.Contracts.Infrastructure.IMenuItemViewModel>();

            Debug.WriteLine(" > MODULE LOADED: " + nameof(CashJournalModule));
        }
    }
}
