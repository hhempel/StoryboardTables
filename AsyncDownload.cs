using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using MonoTouch.AssetsLibrary;
using System.Collections.Generic;
using System.Threading;


namespace StoryboardTables
{
	public partial class AsyncDownload : UIViewController
   {      public string downlodPicLink {get;set;}
      string linkBilgisi; string dokBilgisi;
		public AsyncDownload (IntPtr handle) : base (handle)
		{
		}
      string localPath;
      string dokumanadresi;

      public override void ViewDidLoad()
      {
         base.ViewDidLoad();

         GetButton.TouchUpInside += async (sender, e) =>
         {
            Task<int> sizeTask = DownloadHomepageAsync();

            ResultLabel.Text = "loading...";
            ResultTextView.Text = "loading...\n";
            DownloadedImageView.Image = null;

            // await! control returns to the caller
            var intResult = await sizeTask;

            // when the Task<int> returns, the value is available and we can display on the UI
            ResultLabel.Text = "Length: " + intResult;

            // effectively returns void
         };
       

      

        



      }

      async Task   SaveBytesToFileAsync(byte[] bytesToSave, string fileName)
      {
         string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
         string localFilename = fileName;
         localPath = Path.Combine(documentsPath, localFilename);

         if (File.Exists(localPath))
         {
            File.Delete(localPath);
         }

         using (FileStream fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
         {
            await fs.WriteAsync(bytesToSave, 0, bytesToSave.Length);
         }
      }

      public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
      {

         if (segue.Identifier == "segueResimGonder")
         { // set in Storyboard
            {

               var navctlr = segue.DestinationViewController as imageViewCode;

               if (navctlr != null)
                  navctlr.ImgUrl = linkBilgisi;

    
            }
         }


         else  if (segue.Identifier == "DokGosterX")
         { // set in Storyboard
            {

               var navctlr = segue.DestinationViewController as docView;

               if (navctlr != null)
                  navctlr.DokAdresi = dokumanadresi;


            }
         }
      }

      public async Task<int> DownloadHomepageAsync()
      {
         try
         {
            var httpClient = new HttpClient(); // Xamarin supports HttpClient!

            //
            // download HTML string
            Task<string> contentsTask = httpClient.GetStringAsync("http://microsoft.com"); // async method!

            ResultTextView.Text += "DownloadHomepage method continues after Async() call, until await is used\n";

            // await! control returns to the caller and the task continues to run on another thread
            string contents = await contentsTask;

            // After contentTask completes, you can calculate the length of the string.
            int length = contents.Length;
            ResultTextView.Text += "Downloaded the html and found out the length.\n\n";

            //
            // download image bytes
           
            ResultTextView.Text += "Start downloading image.\n";

          
            byte[] imageBytes = await httpClient.GetByteArrayAsync(new Uri(downlodPicLink)); // async method!  
            ResultTextView.Text += "Downloaded the image.\n";
            await SaveBytesToFileAsync(imageBytes, "original.jpg");
            ResultTextView.Text+= localPath.ToString();

 

      




            DownloadedImageView.Image = UIImage.FromFile(localPath);

           

            byte[] xmlBytes = await httpClient.GetByteArrayAsync("http://mobelsis.net/mobelsis.xml"); // async method!  
            ResultTextView.Text += " xml verisi indi indi.\n";
            await SaveBytesToFileAsync(xmlBytes, "mobelsis.xml");

            byte[] docxBytes = await httpClient.GetByteArrayAsync("http://mobelsis.net/a.doc"); // async method!  
            ResultTextView.Text += " döküman indi.\n";
            await SaveBytesToFileAsync(docxBytes, "a.doc");
            dokumanadresi= localPath;




            ResultTextView.Text+= localPath.ToString();
            linkBilgisi= localPath.ToString();
//
            //
            // save image to Photo Album using async-ified iOS API
            ALAssetsLibrary library = new ALAssetsLibrary();     
            var dict = new NSDictionary();
            var assetUrl = await library.WriteImageToSavedPhotosAlbumAsync(DownloadedImageView.Image.CGImage, dict);
            ResultTextView.Text += "Saved to album assetUrl = " + assetUrl + "\n";

            //
            // download multiple images
            // http://blogs.msdn.com/b/pfxteam/archive/2012/08/02/processing-tasks-as-they-complete.aspx
            Task<byte[]> task1 = httpClient.GetByteArrayAsync("http://xamarin.com/images/tour/amazing-ide.png"); // async method!
            Task<byte[]> task2 = httpClient.GetByteArrayAsync("http://xamarin.com/images/how-it-works/chalkboard2.jpg"); // async method!
            Task<byte[]> task3 = httpClient.GetByteArrayAsync("http://cdn1.xamarin.com/webimages/images/features/shared-code-2.pngXXX"); // ERROR async method!

            List<Task<byte[]>> tasks = new List<Task<byte[]>>();
            tasks.Add(task1);
            tasks.Add(task2);
            tasks.Add(task3);

            while (tasks.Count > 0)
            { 
               var t = await Task.WhenAny(tasks);
               tasks.Remove(t); 

               try
               { 
                  await t; 
                  ResultTextView.Text += "** Downloaded " +  t.Result.Length + " bytes\n";
               }
               catch (OperationCanceledException)
               {
               }
               catch (Exception exc)
               { 
                  ResultTextView.Text += "-- Download ERROR: " + exc.Message + "\n";
               } 
            }

            // this doesn't happen until the image has downloaded as well
            //    ResultTextView.Text += "\n\n\n" + contents; // just dump the entire HTML
            return length; // Task<TResult> returns an object of type TResult, in this case int
         }
         catch (Exception ex)
         {
            Console.WriteLine("Centralized exception handling!");
            return -1;
         }
      }

      // HACK: do not try this at home! just a demo of what happens when you DO block the UI thread :)
//      partial void Naysync_TouchUpInside(UIButton sender)
//      {
//         Thread.Sleep(5000);
//      }

      public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
      {
         // Return true for supported orientations
         return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
      }







   }
}

