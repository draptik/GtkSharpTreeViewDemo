using System;
using Gtk;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{	
	Gdk.Pixbuf defaultIcon;
	Gdk.Pixbuf anotherIcon;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		defaultIcon = new Gdk.Pixbuf ("resources/img/TreeViewRupertIcon.png");
		anotherIcon = new Gdk.Pixbuf ("resources/img/TreeViewRupertIcon.png");

		Build ();
		InitAll ();
		this.ShowAll ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	private void InitAll ()
	{
		var width = 500;
		var height = 800;
		this.SetSizeRequest (width, height);

		var tvIcon = InitTreeViewWithIcon ();
		var tvCollapse = InitTreeViewCollapsibleRows ();
		var tvCollapsibleWithIcon = InitTreeViewCollapsibleRowsWithIcon ();
		var tvComboBox = InitTreeViewWithComboBox ();

		var vbox = new Gtk.VBox ();
		vbox.PackStart (tvIcon, true, true, 0);
		vbox.PackStart (tvCollapse, true, true, 0);
		vbox.PackStart (tvCollapsibleWithIcon, true, true, 0);
		vbox.PackStart (tvComboBox, true, true, 0);

		this.Add (vbox);
	}

	private TreeView InitTreeViewWithIcon ()
	{
		// Model:
		Gtk.ListStore musicListStore = new Gtk.ListStore (typeof(Gdk.Pixbuf), typeof(string), typeof(string));

		// View:
		Gtk.TreeView tree = new Gtk.TreeView ();
		tree.AppendColumn ("Icon", new Gtk.CellRendererPixbuf (), "pixbuf", 0);  
		tree.AppendColumn ("Artist", new Gtk.CellRendererText (), "text", 1);
		tree.AppendColumn ("Title", new Gtk.CellRendererText (), "text", 2);
 
		// Controller:
		musicListStore.AppendValues (anotherIcon, "Pixies", "Gigantic");

		// --> MVP:
		tree.Model = musicListStore;
		return tree;
	}

	private TreeView InitTreeViewCollapsibleRows ()
	{
		// Model:
		Gtk.TreeStore musicListStore = new Gtk.TreeStore (typeof(string), typeof(string));
		Gtk.TreeIter iter;

		// View:
		Gtk.TreeView tree = new Gtk.TreeView ();
		tree.AppendColumn ("Artist", new Gtk.CellRendererText (), "text", 0);
		tree.AppendColumn ("Song", new Gtk.CellRendererText (), "text", 1);

		// Controller:
		iter = musicListStore.AppendValues ("Country");
		musicListStore.AppendValues (iter, "Johnny Cash", "Ring of fire");
 
		iter = musicListStore.AppendValues ("Punk Rock");
		musicListStore.AppendValues (iter, "Iggy Pop", "The passenger");

		// --> MVP:
		tree.Model = musicListStore;
		return tree;
	}

	private TreeView InitTreeViewCollapsibleRowsWithIcon ()
	{
		// Model:
		Gtk.TreeStore musicListStore = new Gtk.TreeStore (typeof(Gdk.Pixbuf), typeof(string), typeof(string));
		Gtk.TreeIter iter;

		// View:
		Gtk.TreeView tree = new Gtk.TreeView ();
		tree.AppendColumn ("Icon", new Gtk.CellRendererPixbuf (), "pixbuf", 0);  
		tree.AppendColumn ("Artist", new Gtk.CellRendererText (), "text", 1);
		tree.AppendColumn ("Song", new Gtk.CellRendererText (), "text", 2);

		// Controller:
		iter = musicListStore.AppendValues (defaultIcon, "Country");
		musicListStore.AppendValues (iter, "Johnny Cash", "Ring of fire");
 
		iter = musicListStore.AppendValues ("Punk Rock"); // <- 'Punk Rock' will not be displayed, because column 0 is of type "pixbuf"
		musicListStore.AppendValues (iter, defaultIcon, "Iggy Pop", "The passenger");

		iter = musicListStore.AppendValues ("", defaultIcon, "BLA"); // <- 'defaultIcon' will not be displayed, because column 1 is of type "text"
		musicListStore.AppendValues (iter, defaultIcon, "Stooges", "Dog food");

		// --> MVP:
		tree.Model = musicListStore;
		return tree;
	}


	// adopted from http://learngtk.org/pygtk-tutorial/cellrenderercombo.html
	private TreeView InitTreeViewWithComboBox ()
	{
		var liststore_manufacturers = new Gtk.ListStore(typeof (string));
		var manufacturers = new List<string> {"Sony", "LG", "Panasonic", "Toshiba", "Nokia", "Samsung"};
        	foreach (var item in manufacturers) {
			liststore_manufacturers.AppendValues (item);
		}

		var liststore_hardware = new Gtk.ListStore(typeof (string), typeof (string));
		liststore_hardware.AppendValues ("Television", "Samsung");
		liststore_hardware.AppendValues ("Mobile Phone", "LG");
		liststore_hardware.AppendValues ("DVD Player", "Sony");

		var treeview = new Gtk.TreeView ();
		treeview.Model = liststore_hardware;

		var column_text = new TreeViewColumn { Title = "Text" };
		var column_combo = new TreeViewColumn { Title = "Combo" };
		treeview.AppendColumn (column_text);
		treeview.AppendColumn (column_combo);

		var cellrenderer_text = new CellRendererText ();
		column_text.PackStart (cellrenderer_text, false);
		column_text.AddAttribute (cellrenderer_text, "text", 0);


		var cellrenderer_combo = new CellRendererCombo ();
		cellrenderer_combo.Editable = true;
		cellrenderer_combo.Model = liststore_manufacturers;
		cellrenderer_combo.TextColumn = 0;
		column_combo.PackStart (cellrenderer_combo, false);
		column_combo.AddAttribute (cellrenderer_combo, "text", 1);

		return treeview;
	}
}