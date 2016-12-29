using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace app.android
{
    public static class ActivityExtensions
    {
        public static string GetToken(this Activity activity)
        {
            var json = default(string);
            using (var reader = new StreamReader(activity.Assets.Open("private.json")))
            {
                json = reader.ReadToEnd();
                reader.Close();
            }
            var jtoken = (JToken)JsonConvert.DeserializeObject(json);
            if (jtoken != null && jtoken["token"] != null) return Convert.ToString(jtoken["token"]);
            return null;
        }

        public static ProgressDialog GetProcessDialog(this Activity ac, string title, string message)
        {
            var dialog = new ProgressDialog(ac);
            dialog.SetTitle(title);
            dialog.SetMessage(message);
            dialog.SetCancelable(false);
            dialog.Show();
            return dialog;
        }

        public static void ShowDialogOk(this Activity ac, string title, string content)
        {
            var alert = new AlertDialog.Builder(ac).Create();
            alert.SetTitle(title);
            alert.SetMessage(content);
            alert.SetButton("OK", (senderAlert, args) => {
                alert.Dismiss();
            });
            alert.Show();

        }

    }
}