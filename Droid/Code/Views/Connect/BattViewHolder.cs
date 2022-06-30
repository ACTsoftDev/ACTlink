using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace actchargers.Droid.Code.Views.Connect
{
    public class BattViewHolder : RecyclerView.ViewHolder
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public TextView name { get; private set; }
        /// <summary>
        /// Gets the inner identifier.
        /// </summary>
        /// <value>The inner identifier.</value>
        public TextView innerID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.Droid.Code.Views.Connect.BattViewHolder"/> class.
        /// </summary>
        /// <param name="itemView">Item view.</param>
        /// <param name="listener">Listener.</param>
        public BattViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            name = itemView.FindViewById<TextView>(Resource.Id.name);
            innerID = itemView.FindViewById<TextView>(Resource.Id.innerID);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}