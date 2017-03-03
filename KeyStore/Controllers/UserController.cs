﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FidoU2f.Models;
using KeyStore.Services;
using KeyStore.Models;
using FidoU2f;
using KeyStore.Services.Interfaces;

namespace KeyStore.Controllers
{
    public class UserController : Controller
    {
        private static IUserRepository _userRepository;

        public static IUserRepository GetFidoRepository()
        {
            return _userRepository ?? (_userRepository = new InMemoryUserRepository());

        }

        //public UserController(IMemberShipService membershipService)
        //{
        //    _userRepository = membershipService;
        //}


        public static string GetCurrentUser()
        {

            return "12"; // users are ignored in this implementation
        }



        public ActionResult Registered()
        {
            var model = new RegistrationsViewModel
            {
                StartedRegistrations = GetFidoRepository().GetAllStartedRegistrationsOfUser(GetCurrentUser()).ToList(),
                DeviceRegistrations = GetFidoRepository().GetDeviceRegistrationsOfUser(GetCurrentUser()).ToList()
            };

            return View(model);
        }
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult RegNewUser()
        {
            var model = new NewUserViewModel();
            return View(model);
        }
        //Partial View
        [HttpGet]
        public ActionResult _Register(NewUserViewModel newUserModel)
        {
            var u2f = new FidoUniversalTwoFactor();
            var appId = new FidoAppId(Request.Url);
            var startedRegistration = u2f.StartRegistration(appId);


            GetFidoRepository().StoreStartedRegistration(newUserModel.UserName, startedRegistration);

            var model = new RegisterNewDeviceViewModel
            {
                AppId = startedRegistration.AppId.ToString(),
                Challenge = startedRegistration.Challenge,
                UserName = newUserModel.UserName,
                Email = newUserModel.Email
            };

            return View(model);
        }

        //Main Register
        [HttpGet]
        public ActionResult Register(NewUserViewModel newUserModel)
        {
            var u2f = new FidoUniversalTwoFactor();
            var appId = new FidoAppId(Request.Url);
            var startedRegistration = u2f.StartRegistration(appId);


            GetFidoRepository().StoreStartedRegistration(newUserModel.UserName, startedRegistration);

            var model = new RegisterNewDeviceViewModel
            {
                AppId = startedRegistration.AppId.ToString(),
                Challenge = startedRegistration.Challenge,
                UserName = newUserModel.UserName,
                Email = newUserModel.Email
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterNewDeviceViewModel model)
        {
            model = model ?? new RegisterNewDeviceViewModel();

            if (!String.IsNullOrEmpty(model.RawRegisterResponse))
            {
                var u2f = new FidoUniversalTwoFactor();

                var challenge = model.Challenge;
                var startedRegistration = GetFidoRepository().GetStartedRegistration(GetCurrentUser(), challenge);

                var deviceRegistration = u2f.FinishRegistration(startedRegistration, model.RawRegisterResponse, GetTrustedDomains());
                GetFidoRepository().StoreDeviceRegistration(GetCurrentUser(), deviceRegistration);
                GetFidoRepository().RemoveStartedRegistration(GetCurrentUser(), model.Challenge);

                return RedirectToAction("Registered");
            }

            return View(model);
        }


        //Login Methods
        [HttpGet]
        public ActionResult Login(string keyHandle)
        {
            var model = new LoginDeviceViewModel { KeyHandle = keyHandle };

            try
            {
                var u2f = new FidoUniversalTwoFactor();
                var appId = new FidoAppId(Request.Url);

                var deviceRegistration = GetFidoRepository().GetDeviceRegistrationsOfUser(GetCurrentUser()).FirstOrDefault(x => x.KeyHandle.ToWebSafeBase64() == keyHandle);
                if (deviceRegistration == null)
                {
                    ModelState.AddModelError("", "Unknown key handle: " + keyHandle);
                    return View(model);
                }

                var startedRegistration = u2f.StartAuthentication(appId, deviceRegistration);

                model = new LoginDeviceViewModel
                {
                    AppId = startedRegistration.AppId.ToString(),
                    Challenge = startedRegistration.Challenge,
                    KeyHandle = startedRegistration.KeyHandle.ToWebSafeBase64(),
                    UserName = GetCurrentUser()
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.GetType().Name + ": " + ex.Message);
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginDeviceViewModel model)
        {
            model = model ?? new LoginDeviceViewModel();

            try
            {
                if (!String.IsNullOrEmpty(model.RawAuthenticationResponse))
                {
                    var u2f = new FidoUniversalTwoFactor();
                    var appId = new FidoAppId(Request.Url);

                    var deviceRegistration = GetFidoRepository().GetDeviceRegistrationsOfUser(GetCurrentUser()).FirstOrDefault(x => x.KeyHandle.ToWebSafeBase64() == model.KeyHandle);
                    if (deviceRegistration == null)
                    {
                        ModelState.AddModelError("", "Unknown key handle: " + model.KeyHandle);
                        return View(new LoginDeviceViewModel());
                    }

                    var challenge = model.Challenge;

                    var startedAuthentication = new FidoStartedAuthentication(appId, challenge,
                        FidoKeyHandle.FromWebSafeBase64(model.KeyHandle ?? ""));

                    var counter = u2f.FinishAuthentication(startedAuthentication, model.RawAuthenticationResponse, deviceRegistration, GetTrustedDomains());

                    // save the counter somewhere, the device registration of the next authentication should use this updated counter
                    //deviceRegistration.Counter = counter;

                    return RedirectToAction("LoginSuccess");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.GetType().Name + ": " + ex.Message);
            }

            return View(model);
        }
        public ActionResult LoginSuccess()
        {
            return View();
        }


        private FidoFacetId[] GetTrustedDomains()
        {
            return new[] { new FidoFacetId(Request.Url) };
        }

        #region TEST


        public ActionResult RegNewUse1r()
        {
            var model = new NewUserViewModel();
            return View(model);
        }



        //public static IUserRepository GetFidoRepository()
        //{
        //    return _userRepository ?? (_userRepository = new InMemoryUserRepository());

        //}


        [HttpGet]
        public ActionResult TESTREG()
        {
            var u2f = new FidoUniversalTwoFactor();
            var appId = new FidoAppId(Request.Url);
            var startedRegistration = u2f.StartRegistration(appId);

            GetFidoRepository().StoreStartedRegistration(GetCurrentUser(), startedRegistration);

            var model = new RegisterNewDeviceViewModel
            {
                AppId = startedRegistration.AppId.ToString(),
                Challenge = startedRegistration.Challenge,
                UserName = GetCurrentUser()
            };

            return View(model);
        }


        [HttpPost]
        public ActionResult TESTSUBMIT(RegisterNewDeviceViewModel model)
        {
            model = model ?? new RegisterNewDeviceViewModel();

            if (!String.IsNullOrEmpty(model.RawRegisterResponse))
            {
                var u2f = new FidoUniversalTwoFactor();

                var challenge = model.Challenge;
                var startedRegistration = GetFidoRepository().GetStartedRegistration(GetCurrentUser(), challenge);

                var deviceRegistration = u2f.FinishRegistration(startedRegistration, model.RawRegisterResponse, GetTrustedDomains());
                GetFidoRepository().StoreDeviceRegistration(GetCurrentUser(), deviceRegistration);
                GetFidoRepository().RemoveStartedRegistration(GetCurrentUser(), model.Challenge);

            }
            return RedirectToAction("Registred");
        }
        #endregion


    }
}


