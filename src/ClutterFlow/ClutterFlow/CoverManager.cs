// 
// CoverManager.cs
//  
// Author:
//       Mathijs Dumon <mathijsken@hotmail.com>
// 
// Copyright (c) 2010 Mathijs Dumon
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
using System.Collections.Generic;

using Clutter;
using Gtk;
using GLib;

namespace ClutterFlow
{
    
	public delegate void CoverEventHandler(ClutterFlowBaseActor actor, EventArgs e);
	
	public class CoverManager : Clutter.Group {

        #region Events
		public event CoverEventHandler CoverActivated;
		internal void InvokeCoverActivated (ClutterFlowBaseActor cover)
        {
			if (CoverActivated!=null) CoverActivated (cover, EventArgs.Empty);
		}
		
		public event CoverEventHandler NewCurrentCover;
		protected void InvokeNewCurrentCover (ClutterFlowBaseActor cover)
        {
			if (NewCurrentCover!=null) NewCurrentCover(cover, EventArgs.Empty);
		}
		
		public event EventHandler<EventArgs> CoversChanged;
		protected void InvokeCoversChanged ()
        {
			if (CoversChanged!=null) CoversChanged(this, EventArgs.Empty);
		}
		
		public event EventHandler<EventArgs> TargetIndexChanged;
		protected void InvokeTargetIndexChanged ()
        {
			if (TargetIndexChanged!=null) TargetIndexChanged(this, EventArgs.Empty);
		}

        public event EventHandler TextureSizeChanged;
        protected void InvokeTextureSizeChanged () {
             //TODO use a timeout here, if the function is called mutliple times shortly after another, we don't get endless recalculation
            if (TextureSizeChanged!=null) TextureSizeChanged (this, EventArgs.Empty);
        }

        public EventHandler<EventArgs> VisibleCoversChanged;
        protected void InvokeVisibleCoversChanged () {
            if (VisibleCoversChanged!=null) VisibleCoversChanged(this, EventArgs.Empty);;
        }
        
        #endregion

        #region Fields
		protected ClutterFlowTimeline timeline;
		public ClutterFlowTimeline Timeline {
			get {
                if (timeline==null) {
                    timeline = new ClutterFlowTimeline(this);
                    timeline.TargetMarkerReached += HandleTargetMarkerReached;
                }
				return timeline; 
			}
		}

		protected IActorLoader actorLoader;	//Loads the actors (detaches ClutterFlow from Banshee related dependencies)
		public IActorLoader ActorLoader {
			get { return actorLoader; }
			internal set { actorLoader = value; }
		}
		
		protected FlowBehaviour behaviour;
		public FlowBehaviour Behaviour {
			get { return behaviour; }
		}
        #endregion
        
        #region Cover-related fields
		protected int maxCoverWidth = 256;
		public int MaxCoverWidth {
			get { return maxCoverWidth; }
			set { 
				if (maxCoverWidth!=value) {
					maxCoverWidth = value;
					UpdateBehaviour(); //TODO use a flag instead. Multiple update calls may cause slowdowns?
				}
			}
		}
        
		protected int minCoverWidth = 64;
		public int MinCoverWidth {
			get { return minCoverWidth; }
			set { 
				if (minCoverWidth!=value) {
					minCoverWidth = value;
					UpdateBehaviour(); //TODO use a flag instead. Multiple update calls may cause slowdowns?
				}
			}
		}

		protected int textureSize = 128;
		public int TextureSize {
			get { return textureSize; }
			set {
				if (textureSize!=value) {
					textureSize = value;
                    InvokeTextureSizeChanged ();
				}
			}
		}
		
		protected int visibleCovers = 17;
		public int VisibleCovers {
			get { return visibleCovers; }
			set {
				if (value!=visibleCovers) {
					visibleCovers = value;
                    InvokeVisibleCoversChanged ();
				}
			}
		}
		public int HalfVisCovers {
			get { return (int) ((visibleCovers-1) * 0.5); }
		}
        #endregion

        #region Animation duration limits
		protected static uint maxAnimationSpan = 250; //maximal length of a single step in the animations in ms
		public static uint MaxAnimationSpan {
			get { return maxAnimationSpan; }
		}
		
		protected static uint minAnimationSpan = 10; //minimal length of a single step in the animations in ms
		public static uint MinAnimationSpan {
			get { return minAnimationSpan; }
		}
        #endregion

        #region Target/Current index handling
		private int target_index = 0;			// curent targetted cover index
		public int TargetIndex {
			get { return target_index; }
			set {
				if (value >= TotalCovers) value = TotalCovers-1;
				if (value < 0) value = 0;
				if (value!=target_index) {
                    //Console.WriteLine ("TargetIndex_set to " + value);
                    target_index = value;
                    current_cover = null; //to prevent clicks to load the old centered cover!
                    InvokeTargetIndexChanged();
				}
			}
		}

		public ClutterFlowBaseActor TargetActor {
			get {
				if (covers.Count > TargetIndex)
					return covers[TargetIndex];
				else
					return null;
			}
		}

        protected ClutterFlowFixedActor empty_actor;
        public ClutterFlowFixedActor EmptyActor {
            get {
                if (empty_actor==null)
                    empty_actor = new ClutterFlowFixedActor (this);
                return empty_actor;
            }
        }
        
        internal List<ClutterFlowBaseActor> covers;     // list with cover actors
        public int TotalCovers  {               // number of covers or zero if null
            get { return (covers != null) ? covers.Count : 0; }
        }
        
        private ClutterFlowBaseActor current_cover = null; // currently centered cover
        public ClutterFlowBaseActor CurrentCover {
            get { return current_cover; }
            set {
                if (value!=current_cover) {
                    current_cover = value;
                    InvokeNewCurrentCover (current_cover);
                }
            }
        }
        #endregion
        
		protected bool needs_reloading = false;
		public bool NeedsReloading {
			get { return needs_reloading; }
			internal set { needs_reloading = value; }
		}

        #region Initialisation
		public CoverManager () : base ()
		{
			behaviour = new FlowBehaviour (this);			
		}

        public override void Dispose ()
        {
            ActorLoader.Dispose ();
            Behaviour.Dispose ();
            timeline.Dispose ();
            
            covers.Clear ();
            covers = null;
            current_cover = null;
            
            base.Dispose ();
        }
        #endregion

        /*new public ClutterFlowChildMeta GetChildMeta (Clutter.Actor actor)
        {
            return base.GetChildMeta(actor) as ClutterFlowChildMeta;
        }*/

        
		public void UpdateBehaviour () 
		{
			if (behaviour!=null && Stage!=null) {
				behaviour.Height = Stage.Height;
				behaviour.Width = Stage.Width;
				behaviour.CoverWidth = Math.Max(Math.Min(Stage.Height*0.5f, maxCoverWidth), minCoverWidth);;
			}
		}

		void HandleTargetMarkerReached(object sender, TargetReachedEventArgs args)
		{
			if (args.Target==TargetIndex) {
				if (covers.Count > args.Target)
                    CurrentCover = covers[(int) args.Target];
                else
                    CurrentCover = null;
			}
		}
		
		internal void ReloadCovers () 
		{
			if (Timeline!=null) Timeline.Pause ();
            HideAll (); Show ();
			if (covers!=null && covers.Count!=0) {
				Console.WriteLine("Reloading Covers");

                /* Bugs:
                 *  PARTIALLY FIXED - when a search is cleared (or reduced) the TargetIndex is set to 4, never to the correct cover
                 *      --> It jumps to an album near to it, sya four indexes away...?
                 *  FIXED - when searching fast visible covers stay visible
                 *  - when no results are found, cluterflow icon should be displayed
                 *      --> create dummy ClutterFlowBaseActor with:
                 *              - no click event
                 *              - Index always zero
                 *              - Label set to "No Matches"
                 */
                
				int old_current_index = CurrentCover!=null ? covers.IndexOf (CurrentCover) : 0;
				List<ClutterFlowBaseActor> old_covers = new List<ClutterFlowBaseActor>(SafeGetRange(covers, old_current_index - HalfVisCovers - 1, visibleCovers + 2));
				foreach (ClutterFlowBaseActor actor in covers) {
                    if (actor.Data.ContainsKey ("isOldCover")) actor.Data.Remove ("isOldCover");
                    actor.Index = -1;  
                    if (old_covers.Contains (actor))
                        actor.Data.Add ("isOldCover", true);
				}

				bool keep_current = false;
                List<ClutterFlowBaseActor> persistent_covers = new List<ClutterFlowBaseActor>();
				covers = actorLoader.GetActors (delegate (ClutterFlowBaseActor actor) {
                    if (actor.Data.ContainsKey ("isOldCover"))
                        persistent_covers.Add (actor);
                    if (CurrentCover==actor) keep_current = true;
				});
                if (covers.Count==0) {
                    ShowEmptyActor ();
                    EmptyActor.Show ();
                }
                
				//recalculate timeline progression and the target index
				int new_target_index = 0;
				if (covers.Count > 1) {
					if (keep_current)
						new_target_index = CurrentCover.Index;
					else {
                        if (persistent_covers.Count==0)
                            new_target_index = (int) Math.Round(Timeline.Progress * (covers.Count-1));
                        else if (persistent_covers.Count==1)
                            new_target_index = persistent_covers[0].Index;
                        else
                            new_target_index = persistent_covers[(int) (((float) persistent_covers.Count * 0.5f) - 1.0f)].Index;
                    }
				}
                TargetIndex = new_target_index;
                Timeline.JumpToTarget ();

                Console.WriteLine ("Timeline progress set to " + Timeline.Progress + " Timeline.RelativeTarget is " + Timeline.RelativeTarget);

                List<ClutterFlowBaseActor> truly_pers = new List<ClutterFlowBaseActor> ();
				List<ClutterFlowBaseActor> new_covers = new List<ClutterFlowBaseActor>(SafeGetRange(covers, new_target_index - HalfVisCovers - 1, visibleCovers + 2));
                foreach (ClutterFlowBaseActor actor in persistent_covers) {
                    if (actor!=null) {
                        if (actor.Data.ContainsKey ("isOldCover"))
                            actor.Data.Remove ("isOldCover");
                        if (new_covers.Contains (actor)) {
                            truly_pers.Add (actor);
                            new_covers.Remove (actor);
                            old_covers.Remove (actor);
                            actor.Show ();
                        }
                    }
                }
                foreach (ClutterFlowBaseActor actor in old_covers) {
                    if (actor!=null) actor.Data.Remove ("isOldCover");
                }
                
                Console.WriteLine ("old_covers          contains " + old_covers.Count + " elements:");
                foreach (ClutterFlowBaseActor cover in old_covers)
                    Console.WriteLine("\t- " + cover.Label.Replace("\n", " - "));
                Console.WriteLine ("persistent_covers   contains " + truly_pers.Count + " elements");
                foreach (ClutterFlowBaseActor cover in truly_pers)
                    Console.WriteLine("\t- " + cover.Label.Replace("\n", " - "));
                Console.WriteLine ("new_covers          contains " + new_covers.Count + " elements");
                foreach (ClutterFlowBaseActor cover in new_covers)
                    Console.WriteLine("\t- " + cover.Label.Replace("\n", " - "));

                EventHandler update_target = delegate (object o, EventArgs e) {
                    Behaviour.FadeInActor (EmptyActor);
                    EmptyActor.Show ();
                    CurrentCover = EmptyActor;
                    Timeline.Play ();
                    InvokeCoversChanged();
                };
                
                Behaviour.FadeCoversInAndOut (old_covers, truly_pers, new_covers, Timeline.Progress, update_target);
			} else {
				Console.WriteLine("Loading Covers");
				covers = actorLoader.GetActors (null);
                TargetIndex = 0;
                Timeline.JumpToTarget ();
                if (covers==null || covers.Count==0) {
                    ShowEmptyActor ();
                    Behaviour.FadeInActor (EmptyActor);
                }
                Timeline.Play ();
				InvokeCoversChanged ();
			}
		}

        private void ShowEmptyActor ()
        {
            covers = new List<ClutterFlowBaseActor> ();
            covers.Add (EmptyActor);
            CurrentCover = EmptyActor;
        }
		

		private IEnumerable<ClutterFlowBaseActor> SafeGetRange(List<ClutterFlowBaseActor> list, int index, int count) {
			for (int i = index; i < index + count; i++) {
				ClutterFlowBaseActor cover;
				try {
					cover = list[i];
                    if (cover==null) throw new NullReferenceException();
				} catch {
                    continue;
				}
                yield return cover;
			}
			yield break;
		}
		
		public void ForSomeCovers(System.Action<ClutterFlowBaseActor> method_call, int lBound, int uBound) {
			if (covers!=null && lBound <= uBound && lBound >= 0 && uBound < covers.Count) {
				IEnumerator<ClutterFlowBaseActor> enumerator = covers.GetRange(lBound, uBound-lBound + 1).GetEnumerator();
				while (enumerator.MoveNext())
					method_call(enumerator.Current); 
			}
		}
		
		public void ForEachCover(System.Action<ClutterFlowBaseActor> method_call) {
			if (covers!=null) {
				IEnumerator<ClutterFlowBaseActor> enumerator = covers.GetEnumerator();
				while (enumerator.MoveNext())
					method_call(enumerator.Current);
			}
		}
	}
}