using DuoNotes.Model;
using DuoNotes.Resources;

using Firebase.Auth;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace DuoNotes.Utils {
    public class Exceptions {

        public static void GetErrorMessage(FirebaseAuthException ex) {

            var stringError = JsonConvert.DeserializeObject<Response>(ex.ResponseData);

            switch (stringError.Error.Message) {
                case "EMAIL_EXISTS":
                    App.Current.MainPage.DisplayAlert(AppResources.
                        ServerError, AppResources.EMAIL_EXISTS,
                        AppResources.OK);
                    break;

                case "WEAK_PASSWORD : Password should be at least 6 characters":
                    App.Current.MainPage.DisplayAlert(
                        AppResources.ServerError,
                        AppResources.WEAK_PASSWORD___Password_should_be_at_least_6_characters,
                        AppResources.OK);
                    break;

                case "EMAIL_NOT_FOUND":
                    App.Current.MainPage.DisplayAlert(
                       AppResources.ServerError,
                       AppResources.EMAIL_NOT_FOUND,
                       AppResources.OK);
                    break;

                default:
                    break;
            }
        }
    }
}
