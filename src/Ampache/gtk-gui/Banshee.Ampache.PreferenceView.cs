
// This file has been generated by the GUI designer. Do not modify.
namespace Banshee.Ampache
{
	public partial class PreferenceView
	{
		private global::Gtk.HBox hbox1;

		private global::Gtk.Table table1;

		private global::Gtk.Entry entPassword;

		private global::Gtk.Entry entUrl;

		private global::Gtk.Entry entUser;

		private global::Gtk.Label lblPassword;

		private global::Gtk.Label lblUrl;

		private global::Gtk.Label lblUser;

		private global::Gtk.VBox vbox1;

		private global::Gtk.Button btnSave;

		private global::Gtk.Button btnClear;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Banshee.Ampache.PreferenceView
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Banshee.Ampache.PreferenceView";
			// Container child Banshee.Ampache.PreferenceView.Gtk.Container+ContainerChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.entPassword = new global::Gtk.Entry ();
			this.entPassword.CanFocus = true;
			this.entPassword.Name = "entPassword";
			this.entPassword.IsEditable = true;
			this.entPassword.Visibility = false;
			this.entPassword.InvisibleChar = '*';
			this.table1.Add (this.entPassword);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1[this.entPassword]));
			w1.TopAttach = ((uint)(2));
			w1.BottomAttach = ((uint)(3));
			w1.LeftAttach = ((uint)(1));
			w1.RightAttach = ((uint)(2));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entUrl = new global::Gtk.Entry ();
			this.entUrl.CanFocus = true;
			this.entUrl.Name = "entUrl";
			this.entUrl.IsEditable = true;
			this.entUrl.InvisibleChar = '*';
			this.table1.Add (this.entUrl);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1[this.entUrl]));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entUser = new global::Gtk.Entry ();
			this.entUser.CanFocus = true;
			this.entUser.Name = "entUser";
			this.entUser.IsEditable = true;
			this.entUser.InvisibleChar = '*';
			this.table1.Add (this.entUser);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1[this.entUser]));
			w3.TopAttach = ((uint)(1));
			w3.BottomAttach = ((uint)(2));
			w3.LeftAttach = ((uint)(1));
			w3.RightAttach = ((uint)(2));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.lblPassword = new global::Gtk.Label ();
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Xalign = 0f;
			this.lblPassword.LabelProp = global::Mono.Addins.AddinManager.CurrentLocalizer.GetString ("Password:");
			this.lblPassword.Justify = ((global::Gtk.Justification)(1));
			this.lblPassword.SingleLineMode = true;
			this.table1.Add (this.lblPassword);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1[this.lblPassword]));
			w4.TopAttach = ((uint)(2));
			w4.BottomAttach = ((uint)(3));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.lblUrl = new global::Gtk.Label ();
			this.lblUrl.Name = "lblUrl";
			this.lblUrl.Xalign = 0f;
			this.lblUrl.LabelProp = global::Mono.Addins.AddinManager.CurrentLocalizer.GetString ("Ampache Server Name:");
			this.lblUrl.Justify = ((global::Gtk.Justification)(1));
			this.lblUrl.SingleLineMode = true;
			this.table1.Add (this.lblUrl);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1[this.lblUrl]));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.lblUser = new global::Gtk.Label ();
			this.lblUser.Name = "lblUser";
			this.lblUser.Xalign = 0f;
			this.lblUser.LabelProp = global::Mono.Addins.AddinManager.CurrentLocalizer.GetString ("User Name:");
			this.lblUser.Justify = ((global::Gtk.Justification)(1));
			this.lblUser.SingleLineMode = true;
			this.table1.Add (this.lblUser);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1[this.lblUser]));
			w6.TopAttach = ((uint)(1));
			w6.BottomAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			this.hbox1.Add (this.table1);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.table1]));
			w7.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.btnSave = new global::Gtk.Button ();
			this.btnSave.CanFocus = true;
			this.btnSave.Name = "btnSave";
			this.btnSave.UseUnderline = true;
			this.btnSave.Label = global::Mono.Addins.AddinManager.CurrentLocalizer.GetString ("Save");
			this.vbox1.Add (this.btnSave);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.btnSave]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.btnClear = new global::Gtk.Button ();
			this.btnClear.CanFocus = true;
			this.btnClear.Name = "btnClear";
			this.btnClear.UseUnderline = true;
			this.btnClear.Label = global::Mono.Addins.AddinManager.CurrentLocalizer.GetString ("Clear");
			this.vbox1.Add (this.btnClear);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.btnClear]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			this.hbox1.Add (this.vbox1);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vbox1]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			this.Add (this.hbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.btnSave.Clicked += new global::System.EventHandler (this.Save_OnClicked);
			this.btnClear.Clicked += new global::System.EventHandler (this.Clean_OnClicked);
		}
	}
}
