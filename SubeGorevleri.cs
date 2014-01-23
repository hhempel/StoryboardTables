// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace StoryboardTables
{
   public class TableSource : UITableViewSource 
   {
      string[] tableItems;
      string cellIdentifier = "TableCell";
      public TableSource(String[] items)
      {
         tableItems = items;

      }

      public override int RowsInSection(UITableView tableview, int section)
      {
         return tableItems.Length;
      }


      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
      {

         UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
         // if there are no cells to reuse, create a new one
         if (cell == null)
            cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
         cell.TextLabel.Text = tableItems[indexPath.Row];
         return cell;
      }


   }

	public partial class SubeGorevleri : UIViewController
	{
      public string Subex{get;set;}
      UITableView view;

		public SubeGorevleri (IntPtr handle) : base (handle)
		{
		}
      public override void ViewDidLoad()
      {
         base.ViewDidLoad();

         gosterX.TouchUpInside+= delegate
         {
            bilgiF.Text+= Subex;
         };

//         view = new UITableView(view.Bounds);
//
//
//         tSource = new UITableView(view.Bounds);
//         string[] data = { "haluk", "yılamz" };
//         tSource.Source = new TableSource(data);
//
//         Add(tSource);

      }
	}
}
