using System;

using Android.App;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace app.core
{
    public static class ActivityExtensions
    {
        public static JToken GetJsonAsset(this Activity ac, string fileName)
        {
            var json = default(string);
            using (var reader = new StreamReader(ac.Assets.Open(fileName)))
            {
                json = reader.ReadToEnd();
                reader.Close();
            }
            return (JToken)JsonConvert.DeserializeObject(json);
        }

        public static void ExecuteLongTask(this Activity ac, Action onExecute, Action onDone, string title, string message)
        {
            var wait = ProgressDialog.Show(ac, title, message, true, false);
            new Thread(new ThreadStart(delegate
            {
                onExecute();
                ac.RunOnUiThread(() =>
                {
                    wait.Dismiss();
                    onDone();
                });
            })).Start();
        }

        public static void ShowDialogOk(this Activity ac, Action onOk, string title, string content)
        {
            var alert = new AlertDialog.Builder(ac).Create();
            alert.SetTitle(title);
            alert.SetMessage(content);
            alert.SetButton("OK", (senderAlert, args) => {
                onOk();
            });
            alert.Show();

        }

    }
}