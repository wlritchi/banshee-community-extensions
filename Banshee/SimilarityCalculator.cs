/*
 * Mirage - High Performance Music Similarity and Automatic Playlist Generator
 * http://hop.at/mirage
 * 
 * Copyright (C) 2007 Dominik Schnitzer <dominik@schnitzer.at>
 *           (C) 2008 Bertrand Lorentz <bertrand.lorentz@gmail.com>
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor,
 * Boston, MA  02110-1301, USA.
 */

using System;

using Hyena;
using Mirage;

namespace Banshee.Mirage
{
    public class SimilarityCalculator
    {
        int[] trackId;
        int[] excludeTrackId;
        PlaylistGeneratorSource.UpdatePlaylistDelegate play;
        int length;
        Db db;
        
        public SimilarityCalculator(int[] trackId, int[] excludeTrackId,
                Db db, PlaylistGeneratorSource.UpdatePlaylistDelegate playlist, int length)
        {
            this.trackId = trackId;
            this.play = playlist;
            this.length = length;
            this.excludeTrackId = excludeTrackId;
            this.db = db;
        }
        
        public void Compute()
        {
            int[] playlist;
            try {
                playlist = Mir.SimilarTracks(trackId, excludeTrackId, db);
                play(playlist, length);
            } catch (DbTrackNotFoundException) {
                Log.Error ("Mirage: ERROR. Track not found in Mirage DB");
                play(null, 0);
            }
        }
    }
}
