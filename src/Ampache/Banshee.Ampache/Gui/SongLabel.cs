// 
// SongLabel.cs
//  
// Author:
//       John Moore <jcwmoore@gmail.com>
// 
// Copyright (c) 2010 John Moore
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Gtk;
using System.Linq;

namespace Banshee.Ampache
{
	[TreeNode(ListOnly=true)]
	internal class SongLabel : TreeNode
	{
		public SongLabel (AmpacheSong song)
		{
			AmpacheId = song.Id;
			AlbumId = song.AlbumId;
			ArtistId = song.ArtistId;
			Url = song.Uri.AbsoluteUri;
			name = song.TrackTitle;
			_artistName = song.ArtistName;
			albumName = song.AlbumTitle;
		}
		public int AmpacheId {get; set;}
		public int AlbumId {get; set;}
		public int ArtistId {get; set;}
		public string Url {get;set;}
		string name;
		[TreeNodeValue(Column=0)]
		public string Name {get { return new string((name ?? string.Empty).Take(50).ToArray()); } set { name = value; }}
		string _artistName;
		[TreeNodeValue(Column=1)]
		public string ArtistName {get { return new string((_artistName ?? string.Empty).Take(35).ToArray()); } set { _artistName = value; }}
		string albumName;
		[TreeNodeValue(Column=2)]
		public string AlbumName {get { return new string((albumName ?? string.Empty).Take(35).ToArray()); } set { albumName = value; }}
	}
}
