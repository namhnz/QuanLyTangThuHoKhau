using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using CustomMVVMDialogs.ContentDialogFactories;
using CustomMVVMDialogs.DialogTypeLocators;
using CustomMVVMDialogs.FrameworkPickers.FileOpen;
using CustomMVVMDialogs.FrameworkPickers.FileSave;
using CustomMVVMDialogs.FrameworkPickers.Folder;
using log4net;
using log4net.Repository.Hierarchy;
using ModernWpf.Controls;

namespace CustomMVVMDialogs
{
    /// <summary>
    /// Class abstracting the interaction between view models and views when it comes to
    /// opening dialogs using the MVVM pattern in UWP applications.
    /// </summary>
    public class DialogService : IDialogService
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IContentDialogFactory contentDialogFactory;
        private readonly IDialogTypeLocator contentDialogTypeLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <remarks>
        /// By default <see cref="ReflectionContentDialogFactory"/> is used as dialog factory and
        /// <see cref="NamingConventionDialogTypeLocator"/> is used as dialog type locator.
        /// </remarks>
        public DialogService()
            : this(new ReflectionContentDialogFactory(), new NamingConventionDialogTypeLocator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <param name="contentDialogFactory">
        /// Factory responsible for creating content dialogs.
        /// </param>
        /// <remarks>
        /// By default <see cref="NamingConventionDialogTypeLocator"/> is used as dialog type
        /// locator.
        /// </remarks>
        public DialogService(IContentDialogFactory contentDialogFactory)
            : this(contentDialogFactory, new NamingConventionDialogTypeLocator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <param name="contentDialogTypeLocator">
        /// Interface responsible for finding a content dialog type matching a view model.
        /// </param>
        /// <remarks>
        /// By default <see cref="ReflectionContentDialogFactory"/> is used as dialog factory.
        /// </remarks>
        public DialogService(IDialogTypeLocator contentDialogTypeLocator)
            : this(new ReflectionContentDialogFactory(), contentDialogTypeLocator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <param name="contentDialogFactory">
        /// Factory responsible for creating content dialogs.
        /// </param>
        /// <param name="contentDialogTypeLocator">
        /// Interface responsible for finding a dialog type matching a view model.
        /// </param>
        public DialogService(
            IContentDialogFactory contentDialogFactory,
            IDialogTypeLocator contentDialogTypeLocator = null)
        {
            this.contentDialogFactory = contentDialogFactory ?? throw new ArgumentNullException(nameof(contentDialogFactory));
            this.contentDialogTypeLocator = contentDialogTypeLocator ?? throw new ArgumentNullException(nameof(contentDialogTypeLocator));
        }

        #region IDialogService Members

        /// <inheritdoc />
        public Task<ContentDialogResult> ShowContentDialogAsync<T>(INotifyPropertyChanged viewModel)
            where T : ContentDialog
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            return ShowContentDialogAsync(viewModel, typeof(T));
        }

        /// <inheritdoc />
        public Task<ContentDialogResult> ShowCustomContentDialogAsync<T>(INotifyPropertyChanged viewModel)
            where T : IContentDialog
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            return ShowContentDialogAsync(viewModel, typeof(T));
        }

        /// <inheritdoc />
        public Task<ContentDialogResult> ShowContentDialogAsync(INotifyPropertyChanged viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            Type contentDialogType = contentDialogTypeLocator.Locate(viewModel);
            return ShowContentDialogAsync(viewModel, contentDialogType);
        }

        /// <inheritdoc />
        public Task<IUICommand> ShowMessageDialogAsync(
            string content,
            string title = null,
            IEnumerable<IUICommand> commands = null,
            uint? defaultCommandIndex = default(uint?),
            uint? cancelCommandIndex = default(uint?),
            MessageDialogOptions options = MessageDialogOptions.None)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            Log.Info($"Title: {title}; Content: {content}");

            var messageDialog = new MessageDialog(content)
            {
                Options = options
            };

            DoIf(title != null, () => messageDialog.Title = title);
            DoIf(defaultCommandIndex != null, () => messageDialog.DefaultCommandIndex = defaultCommandIndex.Value);
            DoIf(cancelCommandIndex != null, () => messageDialog.CancelCommandIndex = cancelCommandIndex.Value);

            foreach (IUICommand uiCommand in commands ?? Enumerable.Empty<IUICommand>())
            {
                messageDialog.Commands.Add(uiCommand);
            }

            return messageDialog.ShowAsync().AsTask();
        }

        /// <inheritdoc />
        public Task<StorageFile> PickSingleFileAsync(FileOpenPickerSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            Log.Info($"Commit button text: {settings.CommitButtonText}");

            var dialog = new FileOpenPickerWrapper(settings);
            return dialog.PickSingleFileAsync();
        }

        /// <inheritdoc />
        public Task<IReadOnlyList<StorageFile>> PickMultipleFilesAsync(FileOpenPickerSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            Log.Info($"Commit button text: {settings.CommitButtonText}");

            var dialog = new FileOpenPickerWrapper(settings);
            return dialog.PickMultipleFilesAsync();
        }

        /// <inheritdoc />
        public Task<StorageFile> PickSaveFileAsync(FileSavePickerSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            Log.Info($"Commit button text: {settings.CommitButtonText}");

            var dialog = new FileSavePickerWrapper(settings);
            return dialog.PickSaveFileAsync();
        }

        /// <inheritdoc />
        public Task<StorageFolder> PickSingleFolderAsync(FolderPickerSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var dialog = new FolderPickerWrapper(settings);
            return dialog.PickSingleFolderAsync();
        }

        #endregion

        private Task<ContentDialogResult> ShowContentDialogAsync(
            INotifyPropertyChanged viewModel,
            Type contentDialogType)
        {
            Log.Info($"Content dialog: {contentDialogType}; View model: {viewModel.GetType()}");

            IContentDialog dialog = CreateContentDialog(contentDialogType, viewModel);
            return dialog.ShowAsync();
        }

        private IContentDialog CreateContentDialog(
            Type dialogType,
            INotifyPropertyChanged viewModel)
        {
            var dialog = contentDialogFactory.Create(dialogType);
            dialog.DataContext = viewModel;

            return dialog;
        }

        private static void DoIf(bool condition, Action action)
        {
            if (condition)
            {
                action();
            }
        }
    }
}