using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Exam2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Click_Button(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile userPhoto = await picker.PickSingleFileAsync();
            if (userPhoto != null)
            {
                var filestream = await userPhoto.OpenAsync(FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(filestream);
                Picture.Source = bitmapImage;
                saveUserPhoto(userPhoto);
            }
        }

        private async void saveUserPhoto(StorageFile photo)
        {
            CloudStorageAccount storageAccount = createStorage();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("photo");
            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = container.GetBlockBlobReference(photo.Name);
            await blob.UploadFromFileAsync(photo);
            MessageDialog dialog = new MessageDialog("Photo sucessfully uploaded!");
            await dialog.ShowAsync();
        }
      
        private CloudStorageAccount createStorage()
        {
            var local = ApplicationData.Current.LocalSettings;

            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=vuong0210;AccountKey=vcUpfJa5EhfCbSUyAVMp/wjkJzBcMsjeMWOez5s7U/o++chCOclWEHdWnEL57BpTAZ67ND81PkwdjtOE7PP4Gg==;EndpointSuffix=core.windows.net");
            }
            catch (FormatException)
            {
                throw new FormatException("Invalid storage account information provided.");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid storage account information provided.");
            }
            return storageAccount;
        }
    }
}
