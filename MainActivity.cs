using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Mono.Security.Interface;
using System;

namespace GradeCalculator
{
    [Activity(Label = "GradeCalculator", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
        }

        [Java.Interop.Export("getInfo")]
        public void InfoDialog(View v)
        {
            ShowAlertDialog("Информация", "Това е пробна информация за приложението. То има за цел да улесни работата на учителите при проверка на контролни и учениците да могат да проверят от колко точки се нуждаят за да получат желаната оценка. Формулата е стандартна. \n\n *Ако се използва за НВО се смятат 95 максимални.", "Затвори");
        }

        [Java.Interop.Export("calculateMark")]
        public void calculateMark(View v)
        {
            if (String.IsNullOrEmpty(FindViewById<EditText>(Resource.Id.receivedPoints).Text))
            {
                ShowAlertDialogOnlyMessage("Полетата НЕ трябва да са празни!");
                return;
            }
            if (String.IsNullOrEmpty(FindViewById<EditText>(Resource.Id.receivedPoints).Text))
            {
                ShowAlertDialogOnlyMessage("Полетата НЕ трябва да са празни!");
                return;
            }
            if (FindViewById<EditText>(Resource.Id.receivedPoints).Text.Length > 9)
            {
                ShowAlertDialogOnlyMessage("Точките са извън границите!");
                return;
            }
            if (FindViewById<EditText>(Resource.Id.maxPoints).Text.Length > 9)
            {
                ShowAlertDialogOnlyMessage("Точките са извън границите!");
                return;
            }
            var receivedPoints = float.Parse(FindViewById<EditText>(Resource.Id.receivedPoints).Text);
            var maxPoints = float.Parse(FindViewById<EditText>(Resource.Id.maxPoints).Text);
            bool error = false;
            error = ExceptionHandling(maxPoints, receivedPoints, error);
            if (error)
            {
                return;
            }
            float result = 2f + (receivedPoints / (maxPoints / 4f));
            TextView resultPrint = (TextView)FindViewById(Resource.Id.resultTextView);
            resultPrint.SetText("Оценка:  " + result.ToString("#0.00"), TextView.BufferType.Editable);

        }

        public bool ExceptionHandling(float maxPoints, float receivedPoints, bool error)
        {
            if (receivedPoints > maxPoints)
            {
                ShowAlertDialogOnlyMessage("Получените точки НЕ трябва да са повече от максималните!");
                return true;
            }
            if (receivedPoints < 0)
            {
                ShowAlertDialogOnlyMessage("Получените точки трябва да са 0 или повече");
                return true;
            }
            if (maxPoints < 1)
            {
                ShowAlertDialogOnlyMessage("Максималните точки трябва да са повече от 0");
                return true;
            }
            return false;
        }

        public void ShowAlertDialog(string title, string message, string button)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
            alertDialog.SetNeutralButton(button, delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        public void ShowAlertDialogOnlyMessage(string message)
        {
            ShowAlertDialog("Грешка!", message, "OK");
        }
    }
}


