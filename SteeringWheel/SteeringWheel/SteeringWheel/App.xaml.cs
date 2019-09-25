﻿using Plugin.BLE;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using SteeringWheel.ViewModels;
using SteeringWheel.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SteeringWheel
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(CrossBluetoothLE.Current.Adapter);
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ConnectView, ConnectViewViewModel>();
            containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
        }

        protected async override void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync(new Uri("http://www.SteeringWheel/NavigationPage/ConnectView", UriKind.Absolute));

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    var statuses = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                }

                if (status == PermissionStatus.Granted)
                {
                    //Query permission
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //location denied
                }
            }
            catch (Exception ex)
            {
                //Something went wrong
            }
        }
    }
}
