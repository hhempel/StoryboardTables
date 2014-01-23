// This file has been autogenerated from a class added in the UI designer.

using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MonoTouch.QuickLook;
using System.Collections.Generic;
using System.IO;


namespace StoryboardTables
{
	public partial class docViewerX : UIViewController
	{
      public string dokumAdres{ get; set;}
      public docViewerX () : base ("MainController", null)
		{
		}


      private QLPreviewController previewController;
      private List<PreviewItem> previewItems;
      public override void ViewDidLoad ()
      {
         base.ViewDidLoad ();

         string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
         string localFilename = "text.docx";
         string localPath = Path.Combine(documentsPath, localFilename);


         this.previewItems = new List<PreviewItem>() {

            new PreviewItem("PDF", NSUrl.FromFilename("docs/pdfdoc.pdf")),
            new PreviewItem("DOC", NSUrl.FromFilename(dokumAdres)),

            new PreviewItem("DOCX", NSUrl.FromFilename(localPath)),
            new PreviewItem("XLSX", NSUrl.FromFilename("docs/spreadsheet.xlsx"))

         };

         this.previewController = new QLPreviewController();
         this.previewController.DataSource = new PreviewDataSource(this.previewItems);



         this.buttonPreviewDocs.TouchUpInside += delegate {

            this.PresentModalViewController(this.previewController, true);
         };

      }



      private class PreviewDataSource : QLPreviewControllerDataSource
      {

         public PreviewDataSource (List<PreviewItem> items)
         {

            this.previewItems = items;

         }//end ctor
         private List<PreviewItem> previewItems;




         public override int PreviewItemCount (QLPreviewController controller)
         {
            return this.previewItems.Count;
         }




         public override QLPreviewItem GetPreviewItem (QLPreviewController controller, int index)
         {
            return this.previewItems[index];
         }





      }//end PreviewDatasource





      private class PreviewItem : QLPreviewItem
      {

         public PreviewItem (string title, NSUrl url)
         {

            this.pItemTitle = title;
            this.pItemUrl = url;

         }//end ctor
         private string pItemTitle;
         private NSUrl pItemUrl;



         public override string ItemTitle 
         {
            get 
            {
               return this.pItemTitle;
            }
         }




         public override NSUrl ItemUrl 
         {
            get 
            {
               return this.pItemUrl;
            }
         }

      }//end class PreviewItem


   }
}




