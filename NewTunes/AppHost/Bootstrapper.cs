﻿using System;
using System.Collections.Generic;
using Microsoft.Phone.Tasks;
using Caliburn.Micro;
using NewTunes;

namespace WindowsPhonePanoramaAppHost
{
    public class Bootstrapper : PhoneBootstrapper
    {
        PhoneContainer container;

        protected override void Configure()
        {
            container = new PhoneContainer();

            container.RegisterSingleton(typeof(MainPageViewModel), "MainPageViewModel", typeof(MainPageViewModel));
            container.RegisterSingleton(typeof(ItemViewModel), "ItemViewModel", typeof(ItemViewModel));

            container.RegisterInstance(typeof(iTunesRepository), null, new iTunesRepository());
            container.RegisterInstance(typeof(INavigationService), null, new FrameAdapter(RootFrame));
            container.RegisterInstance(typeof(IPhoneService), null, new PhoneApplicationServiceAdapter(PhoneService));

            container.Activator.InstallChooser<PhoneNumberChooserTask, PhoneNumberResult>();
            container.Activator.InstallLauncher<EmailComposeTask>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}