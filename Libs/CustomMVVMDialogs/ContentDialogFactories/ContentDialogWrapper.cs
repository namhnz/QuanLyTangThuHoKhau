using System;
using System.Threading.Tasks;
using Windows.Foundation;
using ModernWpf.Controls;

namespace CustomMVVMDialogs.ContentDialogFactories
{
    /// <summary>
    /// Class wrapping an instance of <see cref="IContentDialog"/> in <see cref="ContentDialog"/>.
    /// </summary>
    /// <seealso cref="IContentDialog" />
    public class ContentDialogWrapper : IContentDialog
    {
        private readonly ContentDialog contentDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogWrapper"/> class.
        /// </summary>
        /// <param name="contentDialog">The content dialog.</param>
        public ContentDialogWrapper(ContentDialog contentDialog)
        {
            this.contentDialog = contentDialog ?? throw new ArgumentNullException(nameof(contentDialog));
        }

        /// <inheritdoc />
        public object DataContext
        {
            get => contentDialog.DataContext;
            set => contentDialog.DataContext = value;
        }

        /// <inheritdoc />
        public Task<ContentDialogResult> ShowAsync()
        {
            return contentDialog.ShowAsync();
        }
    }
}
