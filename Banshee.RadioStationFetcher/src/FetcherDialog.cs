//
// FetcherDialog.cs
//
// Author:
//   Akseli Mantila <aksu@paju.oulu.fi>
//
// Copyright (C) 2009 Akseli Mantila
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Threading;

using Gtk;

using Banshee.I18n;
using Banshee.Collection.Database;
using Banshee.Sources;

namespace Banshee.RadioStationFetcher
{

    public abstract class FetcherDialog : Gtk.Dialog
    {
        public List<string> genre_list = new List<string> ();
        public static int genre_search = 0;
        public static int freeText_search = 1;
        
        private Button close_button;
        private ComboBoxEntry genre_entry;
        private Entry freeText_entry;
        private Button genre_button;
        private Button freeText_button;
        private Alignment message_container;
        private Label message;
        private Table table;
        protected Statusbar statusbar;
        
        public FetcherDialog ()
        {
            InitializeDialog ();
        }
        
        public void ShowDialog () 
        {
            ShowAll ();
        }
        
        public void HideDialog () 
        {
            HideAll ();
        }
        
        public void InitializeDialog () 
        {
            FillGenreList ();
            
            AccelGroup accel_group = new AccelGroup ();
            AddAccelGroup (accel_group);
            
            Title = String.Empty;
            SkipTaskbarHint = true;
            Modal = true;
                        
            BorderWidth = 6;
            HasSeparator = false;
            DefaultResponse = ResponseType.Ok;
            Modal = true;
            
            VBox.Spacing = 6;
            
            HBox split_box = new HBox ();
            split_box.Spacing = 12;
            split_box.BorderWidth = 6;
            
            Image image = new Image ();
            image.IconSize = (int)IconSize.Dialog;
            image.IconName = "radio";
            image.Yalign = 0.0f;
            image.Show ();
            
            VBox main_box = new VBox ();
            main_box.BorderWidth = 5;
            main_box.Spacing = 10;
            
            Label header = new Label ();

            header.Xalign = 0.0f;
            header.Show ();

            Label message = new Label ();
            message.Text = Catalog.GetString ("Choose a genre or enter a text that you wish to be queried. Then press Search-button");
            message.Xalign = 0.0f;
            message.Wrap = true;
            message.Show ();
            
            table = new Table (5, 2, false);
            table.RowSpacing = 6;
            table.ColumnSpacing = 6;
                        
            genre_entry = ComboBoxEntry.NewText ();
            freeText_entry = new Entry ();
          
            genre_button = new Button("Search");
            freeText_button = new Button("Search");

            genre_button.CanDefault = true;
            genre_button.UseStock = true;
            genre_button.Clicked += OnGenreQueryButtonClick;
            genre_button.Show ();


            freeText_button.CanDefault = true;
            freeText_button.UseStock = true;
            freeText_button.Clicked += OnFreetextQueryButtonClick;
            freeText_button.Show ();

            foreach (string genre in genre_list) {
                if (!String.IsNullOrEmpty (genre))
                    genre_entry.AppendText (genre);
            }

            genre_entry.Entry.Sensitive = false;
            
            if (this is IGenreSearchable) {
                AddRow (Catalog.GetString ("Query by genre:"), genre_entry, genre_button);    
            }
            
            if (this is IFreetextSearchable) {
                AddRow (Catalog.GetString ("Query by free text:"), freeText_entry, freeText_button);
            }
            
            table.ShowAll ();
            
            main_box.PackStart (header, false, false, 0);
            main_box.PackStart (message, false, false, 0);
            main_box.PackStart (table, false, false, 0);
            main_box.Show ();
            
            split_box.PackStart (image, false, false, 0);
            split_box.PackStart (main_box, true, true, 0);
            split_box.Show ();
            
            VBox.PackStart (split_box, true, true, 0);

            close_button = new Button (Stock.Close);
            close_button.CanDefault = true;
            close_button.UseStock = true;
            close_button.Show ();
            AddActionWidget (close_button, ResponseType.Close);
            
            close_button.AddAccelerator ("activate", accel_group, (uint)Gdk.Key.Escape, 
                0, Gtk.AccelFlags.Visible);
            
            close_button.Clicked += OnCloseButtonClick;
            
            statusbar = new Statusbar ();
            
            statusbar.HasResizeGrip = false;
            main_box.PackEnd (statusbar, false, false, 0);
            
            table.Attach (message_container, 0, 2, 4, 5, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 0, 0);
        }
 
        public void OnCloseButtonClick (object o, EventArgs args) 
        {
            HideDialog ();
        } 
        
        private void OnGenreQueryButtonClick (object o, EventArgs args) 
        {
            ThreadPool.QueueUserWorkItem (DoGenreQuery, null);
        }
        
        private void OnFreetextQueryButtonClick (object o, EventArgs args) 
        {
            ThreadPool.QueueUserWorkItem (DoFreetextQuery, null);
        }
        
        private void DoGenreQuery (object o) 
        {
            statusbar.Push (0, Catalog.GetString ("Querying genre \"" +  Genre + "\""));
            List<DatabaseTrackInfo> fetched_stations = (this as IGenreSearchable).FetchStationsByGenre (Genre);
            SaveFetchedTracksToDatabase (fetched_stations);
        } 
        
        private void DoFreetextQuery (object o) 
        {
            statusbar.Push (0, Catalog.GetString ("Querying freetext \"" + Freetext + "\""));
            List<DatabaseTrackInfo> fetched_stations = (this as IFreetextSearchable).FetchStationsByFreetext (Freetext);
            SaveFetchedTracksToDatabase (fetched_stations);
        }

        private void SaveFetchedTracksToDatabase (List<DatabaseTrackInfo> fetched_stations) 
        {
            if (fetched_stations == null) {
                statusbar.Push (0, Catalog.GetString ("Error fetching stations."));
                return;
            }
            
            foreach (DatabaseTrackInfo track in fetched_stations) {
                track.Save ();
            }
            
            statusbar.Push (0, Catalog.GetString ("Query done. Fetched " + fetched_stations.Count + " stations."));
        } 
        
        public abstract void FillGenreList ();
        
        private uint row = 0;
        private void AddRow (string title, Widget entry, Widget button)
        {
            Label label = new Label (title);
            label.Xalign = 0.0f;
            table.Attach (label, 0, 1, row, row + 1, AttachOptions.Fill, AttachOptions.Fill | AttachOptions.Expand, 0, 0);
            table.Attach (entry, 1, 2, row, row + 1, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
            table.Attach (button, 2, 3, row, row + 1, AttachOptions.Fill | AttachOptions.Expand, AttachOptions.Shrink, 0, 0);
            row++;
        }
    
        public string Genre {
            get { return genre_entry.Entry.Text.Trim (); }
        }

        public string Freetext {
            get { return freeText_entry.Text.Trim (); }
        }
        
        public string ErrorMessage {
            set { 
                if (value == null) {
                    message_container.Hide ();
                } else {
                    message.Text = value; 
                    message_container.Show ();
                }
            }
        }
        
        protected PrimarySource GetInternetRadioSource () 
        {
            Console.WriteLine ("[FetcherDialog] <GetInternetRadioSource> Start");
            
            foreach (Source source in Banshee.ServiceStack.ServiceManager.SourceManager.Sources) {
                Console.WriteLine ("[FetcherDialog] <GetInternetRadioSource> Source: " + source.GenericName);
                
                if (source.GenericName.Equals ("Radio")) {
                    return (PrimarySource) source;
                }
            }
    
            Console.WriteLine ("[FetcherDialog] <GetInternetRadioSource> Not found returning null");
            return null;
        }
        
        protected override bool OnDeleteEvent (Gdk.Event evnt)
        {
            HideAll ();
            return true;
        }

        protected override void OnClose()
        {
            HideAll ();
        }
    }
}
