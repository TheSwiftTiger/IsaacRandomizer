using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsaacRandomizer
{
    public class Picture
    {
        public bool Rotation1 { get; set; }
        public bool Rotation2 { get; set; }
        public bool Rotation3 { get; set; }
        public string ItemID { get; set; }
        public int PictureID { get; set; }

        public Picture(bool rotation1, bool rotation2, bool rotation3, string itemID, int pictureID)
        {
            Rotation1 = rotation1;
            Rotation2 = rotation2;
            Rotation3 = rotation3;
            ItemID = itemID;
            PictureID = pictureID;
        }
    }
}
