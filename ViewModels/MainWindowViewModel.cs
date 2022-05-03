using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RSVForTagPrint.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private string comment = "コメント";
        public string Comment
        {
            get { return this.comment; }
            set
            {
                this.SetProperty(ref this.comment, value);
                // 入力値に変化がある度にコマンドのCanExecuteの状態が変わったことを通知する
                this.PrintCommand.RaiseCanExecuteChanged();
            }
        }

        // System.Windows.Controls.BooleanToVisibilityConverter
        private bool progressVisibility;
        public bool ProgressVisibility
        {
            get { return this.progressVisibility; }
            set
            {
                this.SetProperty(ref this.progressVisibility, value);
                this.PrintCommand.RaiseCanExecuteChanged();
            }
        }

        private Models.Job selectedItem;
        public Models.Job SelectedItem
        {
            get { return selectedItem; }
            set
            {
                this.SetProperty(ref this.selectedItem, value);
            }
        }

        public DelegateCommand WindowLoadedCommand { get; private set; }
        public DelegateCommand ClosingCommand { get; private set; }
        public DelegateCommand OptionsCommand { get; private set; }
        public DelegateCommand GotKeyboardFocusCommand { get; private set; }
        public DelegateCommand PrintCommand { get; private set; }
        public DelegateCommand RePrintCommand { get; private set; }

        private InteractionRequest<Notification> notification = new InteractionRequest<Notification>();
        public IInteractionRequest NotificationRequest { get { return notification; } }

        private InteractionRequest<Confirmation> confirmation = new InteractionRequest<Confirmation>();
        public IInteractionRequest ConfirmationRequest { get { return confirmation; } }

        private static readonly string IEProcessName = "iexplore";
        private Helpers.VirgoApi Api = new Helpers.VirgoApi();

        public ICollectionView Histories { get; private set; }

        private InteractionRequest<Notification> preferenceNotificationRequest = new InteractionRequest<Notification>();
        public IInteractionRequest PreferenceNotificationRequest { get { return preferenceNotificationRequest; }}

        public MainWindowViewModel()
        {
            this.Histories = new ListCollectionView(Models.Environment.Instance.Histories);
            this.Histories.CurrentChanged += (a, b) =>
                {
                    this.RePrintCommand.RaiseCanExecuteChanged();
                };

            this.WindowLoadedCommand = new DelegateCommand(
                () =>
                {
                    Debug.WriteLine("WindowLoadedCommand");
                });

            this.ClosingCommand = new DelegateCommand(
                () =>
                {
                    Debug.WriteLine("ClosingCommand");
                });

            this.OptionsCommand = new DelegateCommand(
                () =>
                {
                    Debug.WriteLine("OptionsCommand");
                    preferenceNotificationRequest.Raise(new Notification { Title = "オプション" });
                });

            this.GotKeyboardFocusCommand = new DelegateCommand(
                () =>
                {
                    Debug.WriteLine("Execute GotKeyboardFocusCommand");
                    Helpers.TouchPanel.StartOnScreenKeyboard();
                },
                () =>
                {
                    Debug.WriteLine("Can GotKeyboardFocusCommand");
                    return Helpers.TouchPanel.CanStartOnScreenKeyboard();
                });

            this.PrintCommand = new DelegateCommand(
                Print,
                () => { return !string.IsNullOrWhiteSpace(this.Comment); });

            this.RePrintCommand = new DelegateCommand(
                RePrint,
                () => { return this.Histories.CurrentItem != null; });
        }

        private void CloseBrowserBefore(Action printing)
        {
            Debug.Assert(printing != null);

            // IEをすべて閉じる？
            if (Process.GetProcessesByName(IEProcessName).Length > 0)
            {
                var q = new Confirmation { Title = "確認", Content = "Internet Explorerをすべて閉じます。" };
                confirmation.Raise(q);
                if (!q.Confirmed)
                {
                    // 閉じない場合
                    return;
                }
                Helpers.Process.DestroyAll(IEProcessName);
            }
            printing();
        }

        private void RePrint()
        {
            CloseBrowserBefore(() =>
            {
                //var current = this.Histories.CurrentItem as Models.Job;
                //Debug.WriteLine(current);
                //Helpers.Process.OpenPrintPreviewWithIE(current.CodeToPrint);
                Helpers.Process.OpenPrintPreviewWithIE(this.SelectedItem.CodeToPrint);
            });
        }

        private void Print()
        {
            CloseBrowserBefore(() =>
            {
                ProgressVisibility = true;
                var task = Task.Run<string>(() =>
                {
                    Api.FetchSource();
                    if (Api.UniqueJanCodes.Count == 0)
                    {
                        throw new Exception("印刷枚数 0 です。");
                    }
                    // CSVファイル生成
                    string csvFileFullName = System.IO.Path.GetTempFileName();
                    var keyToPrint = Text.RandomString.Generate();
                    Helpers.Fmww.WriteFile(csvFileFullName, keyToPrint, Api.UniqueJanCodes);

                    Helpers.Fmww.Upload(csvFileFullName);

                    Helpers.Process.OpenPrintPreviewWithIE(keyToPrint);

                    Api.DestroyAll();
                    return keyToPrint;
                });

                task.ContinueWith((t) =>
                {
                    ProgressVisibility = false;
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        // 印刷コード保存
                        Models.Environment.Instance.Histories.Add(new Models.Job()
                        {
                            CodeToPrint = t.Result,
                            Comment = this.Comment,
                            CreatedAt = DateTime.Now
                        });
                        Models.Environment.Instance.Save();
                        //this.Comment = String.Empty;
                    }
                    else if (task.Status == TaskStatus.Faulted)
                    {
                        notification.Raise(new Notification { Title = "確認", Content = task.Exception.GetBaseException().Message });
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            });
        }
    }
}
